namespace Game.Components
{
    public struct GridCell
    {
        public int Row { get; }
        public int Column { get; }
        public CellOccupationInfo OccupationInfo { get; set; }

        public GridCell(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
