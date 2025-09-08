using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.CharacterStates.Payloads
{
    public struct CharacterSpawnStatePayload
    {
        public readonly Vector3 Position;

        public CharacterSpawnStatePayload(Vector3 position)
        {
            Position = position;
        }
    }
}