using Server.Game.Components;

namespace Game.Components
{
    public struct CellOccupationInfo
    {
        public bool IsOccupied { get; }
        public GameSide Occupator { get; }
    }
}
