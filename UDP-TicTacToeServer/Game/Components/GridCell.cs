using ServerShared.Shared.Network;

namespace Game.Components
{
    public struct GridCell
    {
        public int Row { get; }
        public int Column { get; }
        public CellOccupationInfo OccupationInfo { get; private set; }

        public GridCell(int row, int column)
        {
            Row = row;
            Column = column;
            OccupationInfo = default;
        }

        public void SetOccupationInfo(GameSide occupator) {
            OccupationInfo = new CellOccupationInfo(true, occupator);
        }
    }
}
