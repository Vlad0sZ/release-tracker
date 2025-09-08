using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Behaviours.AnimationStates.CharacterStates.Payloads;
using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.CharacterStates
{
    public sealed class CharacterWalkAnimationState : AnimationState<CharacterMoveStatePayload>
    {
        public override async UniTask Animate(CharacterMoveStatePayload payload, CancellationToken cancellationToken)
        {
            try
            {
                await AnimateInternal(payload, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                payload.Character.Walker.IsWalking = false;
                payload.Character.Animator.SetWalkingStateAsync(false, default).Forget();
            }
        }

        private static async UniTask AnimateInternal(CharacterMoveStatePayload payload,
            CancellationToken cancellationToken)
        {
            var character = payload.Character;
            var position = payload.FinishPosition;
            var cachedSpeed = character.Walker.Speed;

            if (payload.Speed.IsSet)
                character.Walker.Speed = payload.Speed.Value;

            await character.Animator.SetWalkingStateAsync(true, cancellationToken);

            character.Walker.IsWalking = true;

            while (cancellationToken.IsCancellationRequested == false &&
                   Vector3.Distance(character.transform.position, position) > 0.2f)
            {
                await UniTask.WaitForEndOfFrame(cancellationToken);
            }

            await character.Animator.SetWalkingStateAsync(false, cancellationToken);

            character.Walker.IsWalking = false;
            character.Walker.Speed = cachedSpeed;
        }
    }
}