using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.Behaviours.AnimationStates
{
    public abstract class AnimationStateWithResult<TPayload, TResult> : AnimationState<TPayload>
    {
        public TResult Result { get; private set; }

        protected abstract UniTask<TResult> AnimateWithResult(TPayload payload, CancellationToken cancellationToken);

        public override async UniTask Animate(TPayload payload, CancellationToken cancellationToken) =>
            Result = await AnimateWithResult(payload, cancellationToken);
    }
}