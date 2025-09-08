using Unity.AppUI.Core;
using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.CharacterStates.Payloads
{
    public struct CharacterMoveStatePayload
    {
        public readonly Character Character;
        public readonly Vector3 FinishPosition;
        public readonly Optional<float> Speed;

        public CharacterMoveStatePayload(Character character, Vector3 finishPosition,
            Optional<float> speed = default)
        {
            Character = character;
            FinishPosition = finishPosition;
            Speed = speed;
        }
    }
}