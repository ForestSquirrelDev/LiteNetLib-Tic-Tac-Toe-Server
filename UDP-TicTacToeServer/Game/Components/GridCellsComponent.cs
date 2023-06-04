using System;
using PoorMansECS.Components;

namespace Game.Components {
    public class GridCellsComponent : IComponentData {
        private readonly GridCell[,] _cellsRowColumnWise;

        public GridCellsComponent(int gridSizeX, int gridSizeY)
        {
            _cellsRowColumnWise = new GridCell[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = gridSizeY - 1; y >= 0; y--)
                {
                    _cellsRowColumnWise[x, y] = new GridCell(x, y);
                }
            }
        }

        public bool TryGetCell(int row, int column, out GridCell cell) {
            if (row < 0 || row >= _cellsRowColumnWise.GetLength(0)) {
                Console.WriteLine($"Row was out of bounds: {row}");
                cell = default;
                return false;
            }
            if (column < 0 || column >= _cellsRowColumnWise.GetLength(1)) {
                Console.WriteLine($"Column was out of bounds: {column}");
                cell = default;
                return false;
            }
            cell = _cellsRowColumnWise[row, column];
            return true;
        }

        public GridCell GetCell(int row, int column) {
            return _cellsRowColumnWise[row, column];
        }

        public GridCell[,] GetCellsCopy() {
            return (GridCell[,])_cellsRowColumnWise.Clone();
        }

        public void SetCell(GridCell cell, int x, int y) {
            if (x < 0 || x >= _cellsRowColumnWise.Length)
                return;
            if (y < 0 || y >= _cellsRowColumnWise.Length)
                return;
            _cellsRowColumnWise[x, y] = cell;
        }
    }
}
