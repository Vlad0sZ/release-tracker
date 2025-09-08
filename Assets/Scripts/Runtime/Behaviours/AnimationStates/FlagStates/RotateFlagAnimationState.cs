using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Behaviours.AnimationStates.FlagStates.Payloads;
using Runtime.Models;
using Runtime.Models.Parameters;
using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.FlagStates
{
    public sealed class RotateFlagAnimationState : AnimationState<RotateFlagStatePayload>
    {
        private readonly AnimationSettings _animationSettings;

        public RotateFlagAnimationState(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }


        public override async UniTask Animate(RotateFlagStatePayload payload, CancellationToken cancellationToken)
        {
            var flag = payload.Flag;
            var angle = payload.Angle;

            await flag.RotateAsync(
                new RotateAroundAnimationParameters(Vector3.forward, angle,
                    _animationSettings.rotateDuration),
                cancellationToken: cancellationToken);
        }
    }
}