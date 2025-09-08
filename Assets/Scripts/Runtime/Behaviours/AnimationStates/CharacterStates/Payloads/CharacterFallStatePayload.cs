using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.CharacterStates.Payloads
{
    public struct CharacterFallStatePayload
    {
        public readonly Character Character;
        public readonly float FallSpeed;
        public readonly Vector3 FallToPosition;

        public CharacterFallStatePayload(Character character, Vector3 fallToPosition, float fallSpeed)
        {
            Character = character;
            FallToPosition = fallToPosition;
            FallSpeed = fallSpeed;
        }
    }
}