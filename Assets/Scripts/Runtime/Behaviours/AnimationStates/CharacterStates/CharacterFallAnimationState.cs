using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Behaviours.AnimationStates.CharacterStates.Payloads;
using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.CharacterStates
{
    public sealed class CharacterFallAnimationState : AnimationState<CharacterFallStatePayload>
    {
        public override async UniTask Animate(CharacterFallStatePayload payload, CancellationToken cancellationToken)
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