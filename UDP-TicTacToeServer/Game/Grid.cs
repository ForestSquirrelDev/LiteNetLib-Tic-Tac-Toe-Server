using PoorMansECS.Components;
using PoorMansECS.Entities;
using Server.Game.Components;

namespace Server.Game {
    public struct CellOccupationInfo {
        public bool IsOccupied { get; }
        public GameSide Occupator { get; }
    }
    public struct GridCell {
        public int Row { get; }
        public int Column { get; }
        public CellOccupationInfo OccupationInfo { get; set; }

        public GridCell(int row, int column) {
            Row = row;
            Column = column;
        }
    }
    public class GridCellsComponent : IComponentData {
        public GridCell[,] Cells { get; }

        public GridCellsComponent(int gridSizeX, int gridSizeY) {
            Cells = new GridCell[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++) {
                for (int y = 0; y < gridSizeY; y++) {
                    Cells[x, y] = new GridCell(x, y);
                }
            }
        }
    }
    public class Grid : Entity { }
}
