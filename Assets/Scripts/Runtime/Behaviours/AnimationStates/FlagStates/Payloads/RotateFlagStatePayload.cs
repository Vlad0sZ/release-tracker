namespace Runtime.Behaviours.AnimationStates.FlagStates.Payloads
{
    public struct RotateFlagStatePayload
    {
        public readonly FlagBehaviour Flag;
        public readonly float Angle;

        public RotateFlagStatePayload(FlagBehaviour flag, float angle)
        {
            Flag = flag;
            Angle = angle;
        }
    }
}