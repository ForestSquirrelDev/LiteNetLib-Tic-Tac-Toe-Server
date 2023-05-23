using PoorMansECS.Components;

namespace Game.Components {
    public class GridCellsComponent : IComponentData
    {
        public GridCell[,] Cells { get; }

        public GridCellsComponent(int gridSizeX, int gridSizeY)
        {
            Cells = new GridCell[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Cells[x, y] = new GridCell(x, y);
                }
            }
        }
    }
}
