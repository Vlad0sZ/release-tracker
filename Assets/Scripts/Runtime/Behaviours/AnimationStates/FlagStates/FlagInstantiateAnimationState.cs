using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Behaviours.AnimationStates.FlagStates.Payloads;
using Runtime.Models;
using Runtime.Models.Parameters;
using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.FlagStates
{
    public sealed class FlagInstantiateAnimationState
        : AnimationStateWithResult<FlagInstantiateStatePayload, FlagBehaviour>
    {
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

        protected override async UniTask<FlagBehaviour> AnimateWithResult(FlagInstantiateStatePayload payload,
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
}