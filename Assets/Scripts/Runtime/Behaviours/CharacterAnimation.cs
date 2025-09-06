using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Runtime.Behaviours
{
    public sealed class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animatorController;

        private ObservableStateMachineTrigger _stateMachine;

        private static readonly int Walk = Animator.StringToHash("walk");
        private static readonly int Run = Animator.StringToHash("run");
        private static readonly int Die = Animator.StringToHash("die");
        private static readonly int JumpTrigger = Animator.StringToHash("jump");

        private static readonly int WalkState = Animator.StringToHash("walk 30fps");
        private static readonly int IdleState = Animator.StringToHash("Idle 30fps");
        private static readonly int JumpState = Animator.StringToHash("jump 30 fps");
        private static readonly int DieState = Animator.StringToHash("die 30 fps");

        private ObservableStateMachineTrigger StateMachine =>
            _stateMachine ??= animatorController.GetBehaviour<ObservableStateMachineTrigger>();

        public async UniTask SetWalkingStateAsync(bool isWalking, CancellationToken cancellationToken)
        {
            animatorController.SetBool(Walk, isWalking);

            if (isWalking)
                await WaitEnterState(WalkState, cancellationToken);
            else
                await WaitExitState(WalkState, cancellationToken);
        }

        public async UniTask SetFallStateAsync(bool isFalling, CancellationToken cancellationToken)
        {
            animatorController.SetBool(Die, isFalling);

            if (isFalling)
            {
                var stateInfo = await WaitEnterState(DieState, cancellationToken);
                var animationLength = stateInfo.StateInfo.length;
                await UniTask.WaitForSeconds(animationLength, cancellationToken: cancellationToken);
            }
            else
            {
                await WaitEnterState(IdleState, cancellationToken);
            }
        }

        public async UniTask JumpAsync(CancellationToken cancellationToken)
        {
            // Set jump trigger;
            animatorController.SetTrigger(JumpTrigger);

            // wait while enter to jump state;
            await WaitEnterState(JumpState, cancellationToken);

            // wait while exit from jump state;
            await WaitExitState(JumpState, cancellationToken);
        }

        private async UniTask<ObservableStateMachineTrigger.OnStateInfo> WaitEnterState(int state,
            CancellationToken cancellationToken)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken,
                cancellationToken);

            return await StateMachine.OnStateEnterAsObservable()
                .FirstAsync(x => x.StateInfo.shortNameHash == state, linkedCts.Token)
                .AsUniTask();
        }

        private async UniTask<ObservableStateMachineTrigger.OnStateInfo> WaitExitState(int state,
            CancellationToken cancellationToken)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken,
                cancellationToken);

            return await StateMachine.OnStateExitAsObservable()
                .FirstAsync(x => x.StateInfo.shortNameHash == state, linkedCts.Token)
                .AsUniTask();
        }
    }
}