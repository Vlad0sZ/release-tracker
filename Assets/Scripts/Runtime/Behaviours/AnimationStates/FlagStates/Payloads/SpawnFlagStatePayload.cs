using Runtime.Models;

namespace Runtime.Behaviours.AnimationStates.FlagStates.Payloads
{
    public struct SpawnFlagStatePayload
    {
        public readonly ReleaseDataRow[] Table;
        public readonly int CurrentRow;

        public SpawnFlagStatePayload(ReleaseDataRow[] table, int currentRow)
        {
            Table = table;
            CurrentRow = currentRow;
        }
    }
}