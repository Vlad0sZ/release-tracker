using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using Runtime.Behaviours.AnimationStates.CharacterStates;
using Runtime.Behaviours.AnimationStates.CharacterStates.Payloads;
using Runtime.Behaviours.AnimationStates.FlagStates;
using Runtime.Behaviours.AnimationStates.FlagStates.Payloads;
using Runtime.Interfaces.Behaviours;
using Runtime.Models;
using UnityEngine;
using UnityEngine.UI;


namespace Runtime.Behaviours
{
    public sealed class AnimationScreenBehaviour : MonoBehaviour, IAnimationBehaviour
    {
        [SerializeField] private PrefabOf<Character> characterPrefab;
        [SerializeField] private PrefabOf<FlagBehaviour> flagPrefab;
        [SerializeField] private MountainController mountain;
        [SerializeField] private AnimationSettings animationSettings;
        [SerializeField] private Button backButton;
        [SerializeField] private Button reloadButton;

        [Header("Debug")] [SerializeField] private SerializableRow[] rows;

        private readonly List<IDisposable> _animationStates = new List<IDisposable>();


        // Linked cancellation token.
        private CancellationTokenSource _cts;

        private void Start()
        {
            reloadButton.transform.localScale = Vector3.zero;
            backButton.OnClickAsObservable()
                .Subscribe(_ => _cts?.Cancel())
                .AddTo(this);
        }

        public async UniTask PlayAnimation(ReleaseInfo releaseInfo, CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            try
            {
                while (_cts.Token.IsCancellationRequested == false)
                {
                    PrepareAnimation();

                    await CreateAnimationPipeline(releaseInfo, _cts.Token);

                    await reloadButton.transform
                        .DOScale(Vector3.one, 0.33f)
                        .ToUniTask(cancellationToken: _cts.Token);

                    var needToReload = await WaitForReloadOrCancelAsync(_cts.Token);

                    if (needToReload == false)
                        break;
                }
            }
            catch (TaskCanceledException)
            {
                Debug.Log("operation cancelled");
            }
            catch (OperationCanceledException)
            {
                Debug.Log("operation cancelled");
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }

        // Wait while reload button or back button is clicked.
        private async UniTask<bool> WaitForReloadOrCancelAsync(CancellationToken cancellationToken)
        {
            var tcs = new UniTaskCompletionSource<bool>();
            var reloadDisposable = reloadButton
                .OnClickAsObservable()
                .Subscribe(_ => tcs.TrySetResult(true));

            try
            {
                await using (cancellationToken.Register(() => tcs.TrySetCanceled()))
                    return await tcs.Task;
            }
            finally
            {
                reloadDisposable.Dispose();
            }
        }

        private async UniTask CreateAnimationPipeline(ReleaseInfo releaseInfo, CancellationToken cancellationToken)
        {
            if (releaseInfo == null)
                throw new NullReferenceException(nameof(releaseInfo));


            var total = releaseInfo.TotalTasks;
            ReleaseDataRow[] table = releaseInfo.Table;

            var currentRowIndex = GetCurrentRowIndex(table);
            var previousRow = currentRowIndex == 0 ? null : table[currentRowIndex - 1];
            var currentRow = table[currentRowIndex];

            var currentFact = previousRow?.Fact ?? 0;
            var nextFact = currentRow.Fact;
            var nextPlan = currentRow.Plan;

            var currentPercentage = ToPercentage(currentFact, total);
            var nextFactPercentage = ToPercentage(nextFact, total);
            var nextPlanPercentage = ToPercentage(nextPlan, total);

            var startedPosition = mountain.GetPositionAt(currentPercentage);
            var factPosition = mountain.GetPositionAt(nextFactPercentage);
            var planPosition = mountain.GetPositionAt(nextPlanPercentage);

            var parent = transform;

            var flagSpawnState = new FlagInstantiateAnimationState(parent, flagPrefab, animationSettings);
            var flagsState =
                new SpawnFlagsAnimationState(parent, MountainPositionFactory, flagPrefab, animationSettings);
            var characterSpawnState = new CharacterSpawnAnimationState(parent, characterPrefab, animationSettings);
            var characterMoveState = new CharacterWalkAnimationState();
            var characterHappyState = new CharacterHappyAnimationState();
            var flagRotateState = new RotateFlagAnimationState(animationSettings);
            var characterFallState = new CharacterFallAnimationState();

            _animationStates.Add(flagsState);
            _animationStates.Add(characterSpawnState);
            _animationStates.Add(characterMoveState);
            _animationStates.Add(characterHappyState);
            _animationStates.Add(flagSpawnState);
            _animationStates.Add(flagRotateState);
            _animationStates.Add(characterFallState);

            await flagsState.Animate(new SpawnFlagStatePayload(table, currentRowIndex),
                cancellationToken);

            await characterSpawnState.Animate(new CharacterSpawnStatePayload(startedPosition), cancellationToken);
            var character = characterSpawnState.Result;
            await characterMoveState.Animate(
                new CharacterMoveStatePayload(character, planPosition,
                    animationSettings.characterSpeed),
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var flagPlan = flagsState.Result;
            var flagPlanRotation = flagPlan.transform.eulerAngles.z;
            var hideFlagRotation = flagPlanRotation - 180;

            // when plan = fact
            if (nextPlan == nextFact && flagPlan != null)
            {
                await flagRotateState.Animate(
                    new RotateFlagStatePayload(flagPlan, hideFlagRotation),
                    cancellationToken);

                flagPlan.SetColor(animationSettings.factColorEqualsPlan);

                flagRotateState.Animate(new RotateFlagStatePayload(flagPlan, flagPlanRotation),
                    cancellationToken).Forget();

                await characterHappyState.Animate(
                    new CharacterHappyStatePayload(character, 3), cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }

            // when fact > plan
            else if (nextPlan < nextFact)
            {
                var rotatePlanPayload = new RotateFlagStatePayload(
                    flagPlan, hideFlagRotation);

                flagRotateState.Animate(rotatePlanPayload, cancellationToken).Forget();

                var characterMoveToTarget =
                    new CharacterMoveStatePayload(character, factPosition,
                        animationSettings.characterSpeed);

                await characterMoveState.Animate(characterMoveToTarget, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();


                var flagPayload = new FlagInstantiateStatePayload(
                    factPosition, animationSettings.factColorMorePlan, nextFact, 4, LayerMask.GetMask("Default"));


                await flagSpawnState.Animate(flagPayload, cancellationToken);

                await characterHappyState.Animate(new CharacterHappyStatePayload(character, 3),
                    cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
            }

            // when plan > fact
            else
            {
                var rotatePayload = new RotateFlagStatePayload(flagPlan, hideFlagRotation);

                var characterFallPayload = new CharacterFallStatePayload(
                    character, factPosition, animationSettings.characterFallSpeed);

                flagRotateState.Animate(rotatePayload, cancellationToken).Forget();
                await characterFallState.Animate(characterFallPayload, cancellationToken);

                var flagPayload = new FlagInstantiateStatePayload(factPosition,
                    animationSettings.factColorLessPlan, nextFact, 4, LayerMask.GetMask("Default"));

                await flagSpawnState.Animate(flagPayload, cancellationToken);
            }
        }

        private Vector3 MountainPositionFactory(float percentage) => mountain.GetPositionAt(percentage);

        private void PrepareAnimation()
        {
            _animationStates.ForEach(x => x.Dispose());
            _animationStates.Clear();

            reloadButton.transform.DOScale(Vector3.zero, 0.33f);
        }

        private static float ToPercentage(int current, int total) =>
            1f * current / total;

        private static int GetCurrentRowIndex(IReadOnlyList<ReleaseDataRow> table)
        {
            int rowCount = table.Count;

            int currentRowIndex = 0; // Текущая позиция

            // Всегда начинаем с 1 индекса
            for (int i = 1; i < rowCount; i++)
            {
                var nextRow = table[i];
                if (nextRow.Fact == 0)
                    break;

                currentRowIndex = i;
            }

            return currentRowIndex;
        }


        [ContextMenu(nameof(ShowDebug))]
        private void ShowDebug()
        {
            CancelDebug();
            _cts = new CancellationTokenSource();
            var release = GenerateDebugReleaseInfo();
            PlayAnimation(release, _cts.Token).Forget();
        }

        [ContextMenu(nameof(CancelDebug))]
        private void CancelDebug()
        {
            if (_cts == null) return;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }


        private ReleaseInfo GenerateDebugReleaseInfo()
        {
            return new ReleaseInfo()
            {
                TotalTasks = rows.Last().plan,
                Table = rows.Select(x => x.ToReleaseDataRow()).ToArray()
            };
        }

        [Serializable]
        public class SerializableRow
        {
            public int plan;
            public int fact;

            public ReleaseDataRow ToReleaseDataRow()
            {
                return new ReleaseDataRow()
                {
                    Date = DateTime.Now.ToString("d"),
                    Plan = plan,
                    Fact = fact
                };
            }
        }
    }
}