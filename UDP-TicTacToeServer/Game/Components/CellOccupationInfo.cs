using Server.Game.Components;

namespace Game.Components
{
    public readonly struct CellOccupationInfo
    {
        public bool IsOccupied { get; }
        public GameSide Occupator { get; }

        public CellOccupationInfo(bool isOccupied, GameSide occupator) {
            IsOccupied = isOccupied;
            Occupator = occupator;
        }
    }
}
