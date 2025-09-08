using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Behaviours.AnimationStates.FlagStates.Payloads;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.FlagStates
{
    public sealed class SpawnFlagsAnimationState
        : AnimationStateWithResult<SpawnFlagStatePayload, FlagBehaviour>
    {
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

        protected override async UniTask<FlagBehaviour> AnimateWithResult(SpawnFlagStatePayload payload,
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

                var statePayload = new FlagInstantiateStatePayload(flagPos, flagColor, value);

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
}