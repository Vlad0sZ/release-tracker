using Runtime.Behaviours;
using UnityEngine;

namespace Runtime.Models
{
    [System.Serializable]
    public sealed class AnimationSettings
    {
        [Header("Flag settings")] public FlagBehaviour.FlagColor planColor;
        public FlagBehaviour.FlagColor factColorEqualsPlan;
        public FlagBehaviour.FlagColor factColorMorePlan;
        public FlagBehaviour.FlagColor factColorLessPlan;

        [Min(0.1f)] public float flagYSpawnOffset = 1f;
        [Min(0.1f)] public float movementDuration = 1f;
        [Min(0.1f)] public float rotateDuration = 1f;


        [Header("Character settings")] 
        public float characterSpeed = 0.5f;
        public float characterFallSpeed = 0.35f;
        public float characterYOffset = 0.2f;
    }
}