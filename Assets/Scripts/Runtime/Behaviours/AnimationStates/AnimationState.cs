using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Models;
using Runtime.Models.Parameters;
using Unity.AppUI.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.Behaviours.AnimationStates
{
    public interface IAnimationState : IDisposable
    {
        UniTask Animate(object parameters, CancellationToken cancellationToken);
    }


    public abstract class AnimationState<TPayload> : IAnimationState
    {
        private readonly List<GameObject> _instantiateGameObjects = new();
        public abstract UniTask Animate(TPayload payload, CancellationToken cancellationToken);

        public async UniTask Animate(object parameters, CancellationToken cancellationToken)
        {
            if (parameters is TPayload payload)
                await Animate(payload, cancellationToken);
            else
                throw new InvalidCastException($"object {parameters.GetType()} is not a {typeof(TPayload)} type.");
        }


        protected async UniTask<T> Instantiate<T>(Transform parent, PrefabOf<T> prefab,
            CancellationToken cancellationToken)
            where T : MonoBehaviour
        {
            var obj = await prefab.InstantiateAsync(parent, cancellationToken: cancellationToken);
            _instantiateGameObjects.Add(obj.gameObject);
            return obj;
        }

        public virtual void Dispose()
        {
            foreach (var gameObject in _instantiateGameObjects.Where(gameObject => gameObject))
                Object.Destroy(gameObject);

            _instantiateGameObjects.Clear();
        }
    }


    public abstract class AnimationStateWithResult<TPayload, TResult> : AnimationState<TPayload>
    {
        public TResult Result { get; private set; }

        protected abstract UniTask<TResult> AnimateWithResult(TPayload payload, CancellationToken cancellationToken);

        public override async UniTask Animate(TPayload payload, CancellationToken cancellationToken) =>
            Result = await AnimateWithResult(payload, cancellationToken);
    }

    public sealed class FlagInstantiateAnimationState
        : AnimationStateWithResult<FlagInstantiateAnimationState.FlagPayload, FlagBehaviour>
    {
        public struct FlagPayload
        {
            public readonly Vector3 FlagPosition;
            public readonly FlagBehaviour.FlagColor FlagColor;
            public readonly int FlagNumber;
            public readonly int FlagSorting;
            public readonly LayerMask GroundMask;

            public FlagPayload(Vector3 flagPosition, FlagBehaviour.FlagColor flagColor, int flagNumber, int flagSorting,
                LayerMask groundMask)
            {
                FlagPosition = flagPosition;
                FlagColor = flagColor;
                FlagNumber = flagNumber;
                FlagSorting = flagSorting;
                GroundMask = groundMask;
            }

            public FlagPayload(Vector3 flagPosition, FlagBehaviour.FlagColor flagColor, int flagNumber)
            {
                FlagPosition = flagPosition;
                FlagColor = flagColor;
                FlagNumber = flagNumber;
                FlagSorting = 1;
                GroundMask = LayerMask.GetMask("Default");
            }
        }

        private readonly Transform _parent;
        private readonly PrefabOf<FlagBehaviour> _flagPrefab;
        private readonly AnimationSettings _animationSettings;

        public FlagInstantiateAnimationState(Transform parent,
            PrefabOf<FlagBehaviour> flagPrefab, AnimationSettings animationSettings)
        {
            _parent = parent;
            _flagPrefab = flagPrefab;
            _animationSettings = animationSettings;
        }

        protected override async UniTask<FlagBehaviour> AnimateWithResult(FlagPayload payload,
            CancellationToken cancellationToken)
        {
            var flag = await this.Instantiate(_parent, _flagPrefab, cancellationToken);

            var flagPosition = payload.FlagPosition;

            flag.transform.position = flagPosition + Vector3.up * _animationSettings.flagYSpawnOffset;
            flag.SetColor(payload.FlagColor);
            flag.SetNumber(payload.FlagNumber);
            flag.SetSorting(payload.FlagSorting);
            flag.RotateToGround(payload.GroundMask);

            await flag.MoveDownAsync(new MoveAnimationParameters(flagPosition, _animationSettings.movementDuration),
                cancellationToken);


            return flag;
        }
    }

    public sealed class RotateFlagAnimationState : AnimationState<RotateFlagAnimationState.RotateFlagPayload>
    {
        public struct RotateFlagPayload
        {
            public readonly FlagBehaviour Flag;
            public readonly float Angle;

            public RotateFlagPayload(FlagBehaviour flag, float angle)
            {
                Flag = flag;
                Angle = angle;
            }
        }

        private readonly AnimationSettings _animationSettings;

        public RotateFlagAnimationState(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }


        public override async UniTask Animate(RotateFlagPayload payload, CancellationToken cancellationToken)
        {
            var flag = payload.Flag;
            var angle = payload.Angle;

            await flag.RotateAsync(
                new RotateAroundAnimationParameters(Vector3.forward, angle,
                    _animationSettings.rotateDuration),
                cancellationToken: cancellationToken);
        }
    }

    public sealed class SpawnFlagsAnimationState
        : AnimationStateWithResult<SpawnFlagsAnimationState.SpawnFlagPayload, FlagBehaviour>
    {
        public struct SpawnFlagPayload
        {
            public readonly ReleaseDataRow[] Table;
            public readonly int CurrentRow;

            public SpawnFlagPayload(ReleaseDataRow[] table, int currentRow)
            {
                Table = table;
                CurrentRow = currentRow;
            }
        }

        private readonly Transform _parent;
        private readonly Func<float, Vector3> _positionFactory;
        private readonly PrefabOf<FlagBehaviour> _flagPrefab;
        private readonly AnimationSettings _animationSettings;

        private FlagInstantiateAnimationState _state;

        public SpawnFlagsAnimationState(
            Transform parent,
            Func<float, Vector3> positionFactory,
            PrefabOf<FlagBehaviour> flagPrefab,
            AnimationSettings animationSettings)
        {
            _parent = parent;
            _positionFactory = positionFactory;
            _flagPrefab = flagPrefab;
            _animationSettings = animationSettings;
        }

        protected override async UniTask<FlagBehaviour> AnimateWithResult(SpawnFlagPayload payload,
            CancellationToken cancellationToken)
        {
            var table = payload.Table;
            var currentRow = payload.CurrentRow;

            var totalRows = table.Length;
            var total = table.LastOrDefault()?.Plan ?? 0;

            _state = new FlagInstantiateAnimationState(
                _parent,
                _flagPrefab,
                _animationSettings);

            FlagBehaviour currentFlag = null;
            for (int i = 0; i < totalRows; i++)
            {
                var row = table[i];

                var isReached = i < currentRow;
                var value = isReached ? row.Fact : row.Plan;
                var percentageReached = 1f * value / total;
                var flagPos = GetPosition(percentageReached);


                FlagBehaviour.FlagColor flagColor = FlagBehaviour.FlagColor.Yellow;
                if (isReached)
                    flagColor = row.Fact >= row.Plan ? FlagBehaviour.FlagColor.Green : FlagBehaviour.FlagColor.Red;

                var statePayload = new FlagInstantiateAnimationState.FlagPayload(flagPos, flagColor, value);

                var task = _state.Animate(statePayload, cancellationToken);

                if (i == currentRow)
                {
                    await task;
                    currentFlag = _state.Result;
                }
                else
                {
                    await UniTask.WaitForSeconds(0.33f, cancellationToken: cancellationToken);
                }
            }

            return currentFlag;
        }


        private Vector3 GetPosition(float percentage) => _positionFactory(percentage);

        public override void Dispose()
        {
            base.Dispose();
            _state?.Dispose();
        }
    }

    public sealed class CharacterSpawnAnimationState : AnimationStateWithResult<Vector3, Character>
    {
        private readonly Transform _parent;
        private readonly PrefabOf<Character> _characterPrefab;
        private readonly AnimationSettings _animationSettings;

        public CharacterSpawnAnimationState(Transform parent,
            PrefabOf<Character> characterPrefab, AnimationSettings animationSettings)
        {
            _parent = parent;
            _characterPrefab = characterPrefab;
            _animationSettings = animationSettings;
        }


        protected override async UniTask<Character> AnimateWithResult(Vector3 payload,
            CancellationToken cancellationToken)
        {
            var character = await Instantiate(_parent, _characterPrefab, cancellationToken);
            character.transform.position = payload + Vector3.up * _animationSettings.characterYOffset;
            await character.Animator.JumpAsync(cancellationToken);
            await UniTask.NextFrame();

            return character;
        }
    }

    public sealed class CharacterWalkAnimationState : AnimationState<CharacterWalkAnimationState.CharacterMovePayload>
    {
        public struct CharacterMovePayload
        {
            public readonly Character Character;
            public readonly Vector3 FinishPosition;
            public readonly Optional<float> Speed;

            public CharacterMovePayload(Character character, Vector3 finishPosition,
                Optional<float> speed = default)
            {
                Character = character;
                FinishPosition = finishPosition;
                Speed = speed;
            }
        }

        public override async UniTask Animate(CharacterMovePayload payload, CancellationToken cancellationToken)
        {
            var character = payload.Character;
            var position = payload.FinishPosition;
            var cachedSpeed = character.Walker.Speed;

            if (payload.Speed.IsSet)
                character.Walker.Speed = payload.Speed.Value;

            await character.Animator.SetWalkingStateAsync(true, cancellationToken);

            character.Walker.IsWalking = true;

            // TODO walker stop when cancel
            while (cancellationToken.IsCancellationRequested == false &&
                   Vector3.Distance(character.transform.position, position) > 0.2f)
                await UniTask.WaitForEndOfFrame(cancellationToken);

            await character.Animator.SetWalkingStateAsync(false, cancellationToken);

            character.Walker.IsWalking = false;
            character.Walker.Speed = cachedSpeed;
        }
    }

    public sealed class CharacterHappyAnimationState :
        AnimationState<CharacterHappyAnimationState.CharacterHappyPayload>
    {
        public struct CharacterHappyPayload
        {
            public readonly Character Character;
            public readonly int NumberOfHappy;

            public CharacterHappyPayload(Character character, int numberOfHappy)
            {
                Character = character;
                NumberOfHappy = numberOfHappy;
            }
        }

        public override async UniTask Animate(CharacterHappyPayload payload, CancellationToken cancellationToken)
        {
            var n = payload.NumberOfHappy;
            var character = payload.Character;

            for (int i = 0; i < n; i++)
            {
                await character.Animator.JumpAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }


    public sealed class CharacterFallAnimationState : AnimationState<CharacterFallAnimationState.CharacterFallPayload>
    {
        public struct CharacterFallPayload
        {
            public readonly Character Character;
            public readonly float FallSpeed;
            public readonly Vector3 FallToPosition;

            public CharacterFallPayload(Character character, Vector3 fallToPosition, float fallSpeed)
            {
                Character = character;
                FallToPosition = fallToPosition;
                FallSpeed = fallSpeed;
            }
        }

        public override async UniTask Animate(CharacterFallPayload payload, CancellationToken cancellationToken)
        {
            var character = payload.Character;
            var walker = character.Walker;
            await character.Animator.SetFallStateAsync(true, cancellationToken);

            var speed = walker.Speed;
            walker.Speed = payload.FallSpeed;
            walker.Backward = true;
            walker.IsWalking = true;


            while (cancellationToken.IsCancellationRequested == false &&
                   Vector2.Distance(character.transform.position, payload.FallToPosition) > .2f)
                await UniTask.WaitForEndOfFrame(cancellationToken);

            walker.Backward = false;
            walker.IsWalking = false;
            walker.Speed = speed;

            await character.Animator.SetFallStateAsync(false, cancellationToken);
        }
    }
}