using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Behaviours.AnimationStates.CharacterStates.Payloads;

namespace Runtime.Behaviours.AnimationStates.CharacterStates
{
    public sealed class CharacterHappyAnimationState :
        AnimationState<CharacterHappyStatePayload>
    {
        public override async UniTask Animate(CharacterHappyStatePayload payload, CancellationToken cancellationToken)
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
}