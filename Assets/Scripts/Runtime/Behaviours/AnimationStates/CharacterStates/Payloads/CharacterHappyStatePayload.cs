namespace Runtime.Behaviours.AnimationStates.CharacterStates.Payloads
{
    public struct CharacterHappyStatePayload
    {
        public readonly Character Character;
        public readonly int NumberOfHappy;

        public CharacterHappyStatePayload(Character character, int numberOfHappy)
        {
            Character = character;
            NumberOfHappy = numberOfHappy;
        }
    }
}