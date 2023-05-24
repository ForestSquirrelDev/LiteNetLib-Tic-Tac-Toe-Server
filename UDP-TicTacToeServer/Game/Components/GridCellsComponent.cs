using PoorMansECS.Components;

namespace Game.Components {
    public class GridCellsComponent : IComponentData
    {
        public GridCell[,] CellsRowColumnWise { get; }

        public GridCellsComponent(int gridSizeX, int gridSizeY)
        {
            CellsRowColumnWise = new GridCell[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    CellsRowColumnWise[x, y] = new GridCell(x, y);
                }
            }
        }
    }
}
