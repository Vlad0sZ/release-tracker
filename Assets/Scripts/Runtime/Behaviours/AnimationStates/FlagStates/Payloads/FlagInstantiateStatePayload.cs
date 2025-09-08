using UnityEngine;

namespace Runtime.Behaviours.AnimationStates.FlagStates.Payloads
{
    public struct FlagInstantiateStatePayload
    {
        public readonly Vector3 FlagPosition;
        public readonly FlagBehaviour.FlagColor FlagColor;
        public readonly int FlagNumber;
        public readonly int FlagSorting;
        public readonly LayerMask GroundMask;

        public FlagInstantiateStatePayload(Vector3 flagPosition, FlagBehaviour.FlagColor flagColor, int flagNumber, int flagSorting,
            LayerMask groundMask)
        {
            FlagPosition = flagPosition;
            FlagColor = flagColor;
            FlagNumber = flagNumber;
            FlagSorting = flagSorting;
            GroundMask = groundMask;
        }

        public FlagInstantiateStatePayload(Vector3 flagPosition, FlagBehaviour.FlagColor flagColor, int flagNumber)
        {
            FlagPosition = flagPosition;
            FlagColor = flagColor;
            FlagNumber = flagNumber;
            FlagSorting = 1;
            GroundMask = LayerMask.GetMask("Default");
        }
    }
}