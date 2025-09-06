using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Behaviours.AnimationStates;
using Runtime.Models;
using UnityEngine;


namespace Runtime.Behaviours
{
    public sealed class AnimationScreenBehaviour : MonoBehaviour
    {
        [SerializeField] private PrefabOf<Character> characterPrefab;
        [SerializeField] private PrefabOf<FlagBehaviour> flagPrefab;

        [SerializeField] private MountainController mountain;
        [SerializeField] private SerializableRow[] rows;

        [SerializeField] private AnimationSettings animationSettings;

        private CancellationTokenSource _cts;
        public ReleaseInfo ReleaseInfo { get; set; }

        private readonly List<IDisposable> _animationStates = new List<IDisposable>();

        /*
         * *текущая позиция = последний заполненный факт. Следующий либо 0, либо его нет (план выполнен)
         *
         * +++
         * Находим точку А (0% или предыдущая позиция)
         * Находим точку Б (текущая позиция)
         *
         *
         * Ставим точку А (персонажа)
         * Ставим точку Б (флаг План)
         *
         */
        public async UniTask PlayAnimation(CancellationToken cancellationToken)
        {
            _animationStates.ForEach(x => x.Dispose());
            _animationStates.Clear();

            try
            {
                await CreateAnimationPipeline(cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                UnityEngine.Debug.Log("operation cancelled");
            }
        }


        private async UniTask CreateAnimationPipeline(CancellationToken cancellationToken)
        {
            if (ReleaseInfo == null)
                throw new System.NullReferenceException(nameof(ReleaseInfo));


            var total = ReleaseInfo.TotalTasks;
            ReleaseDataRow[] table = ReleaseInfo.Table;

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

            await flagsState.Animate(new SpawnFlagsAnimationState.SpawnFlagPayload(table, currentRowIndex),
                cancellationToken);

            await characterSpawnState.Animate(startedPosition, cancellationToken);
            var character = characterSpawnState.Result;
            await characterMoveState.Animate(
                new CharacterWalkAnimationState.CharacterMovePayload(character, planPosition,
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
                    new RotateFlagAnimationState.RotateFlagPayload(flagPlan, hideFlagRotation),
                    cancellationToken);

                flagPlan.SetColor(animationSettings.factColorEqualsPlan);

                flagRotateState.Animate(new RotateFlagAnimationState.RotateFlagPayload(flagPlan, flagPlanRotation),
                    cancellationToken).Forget();

                await characterHappyState.Animate(
                    new CharacterHappyAnimationState.CharacterHappyPayload(character, 3), cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }

            // when fact > plan
            else if (nextPlan < nextFact)
            {
                var rotatePlanPayload = new RotateFlagAnimationState.RotateFlagPayload(
                    flagPlan, hideFlagRotation);

                flagRotateState.Animate(rotatePlanPayload, cancellationToken).Forget();

                var characterMoveToTarget =
                    new CharacterWalkAnimationState.CharacterMovePayload(character, factPosition,
                        animationSettings.characterSpeed);

                await characterMoveState.Animate(characterMoveToTarget, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();


                var flagPayload = new FlagInstantiateAnimationState.FlagPayload(
                    factPosition, animationSettings.factColorMorePlan, nextFact, 4, LayerMask.GetMask("Default"));


                await flagSpawnState.Animate(flagPayload, cancellationToken);

                await characterHappyState.Animate(new CharacterHappyAnimationState.CharacterHappyPayload(character, 3),
                    cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
            }

            // when plan > fact
            else
            {
                var rotatePayload = new RotateFlagAnimationState.RotateFlagPayload(flagPlan, hideFlagRotation);

                var characterFallPayload = new CharacterFallAnimationState.CharacterFallPayload(
                    character, factPosition, animationSettings.characterFallSpeed);

                flagRotateState.Animate(rotatePayload, cancellationToken).Forget();
                await characterFallState.Animate(characterFallPayload, cancellationToken);

                var flagPayload = new FlagInstantiateAnimationState.FlagPayload(factPosition,
                    animationSettings.factColorLessPlan, nextFact, 4, LayerMask.GetMask("Default"));

                await flagSpawnState.Animate(flagPayload, cancellationToken);
            }
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

        private Vector3 MountainPositionFactory(float percentage) => mountain.GetPositionAt(percentage);


        [ContextMenu(nameof(ShowDebug))]
        private void ShowDebug()
        {
            CancelDebug();
            _cts = new CancellationTokenSource();
            ReleaseInfo = GenerateDebugReleaseInfo();
            PlayAnimation(_cts.Token).Forget();
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

        [System.Serializable]
        public class SerializableRow
        {
            public string date;
            public int plan;
            public int fact;

            public ReleaseDataRow ToReleaseDataRow()
            {
                return new ReleaseDataRow()
                {
                    Date = date,
                    Plan = plan,
                    Fact = fact
                };
            }
        }
    }
}