using System.Collections.Generic;
using System.Linq;
using PoorMansECS.Components;

namespace Game.Components {
    public class GridCellsComponent : IComponentData {
        private readonly GridCell[,] _cellsRowColumnWise;

        public GridCellsComponent(int gridSizeX, int gridSizeY)
        {
            _cellsRowColumnWise = new GridCell[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    _cellsRowColumnWise[x, y] = new GridCell(x, y);
                }
            }
        }

        public GridCell GetCell(int row, int column) {
            return _cellsRowColumnWise[row, column];
        }

        public GridCell[,] GetCellsCopy() {
            return (GridCell[,])_cellsRowColumnWise.Clone();
        }
    }
}
