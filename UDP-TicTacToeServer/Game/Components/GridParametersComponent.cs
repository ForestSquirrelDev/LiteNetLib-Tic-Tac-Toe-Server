using PoorMansECS.Components;

namespace Game.Components {
    public readonly struct GridParametersComponent : IComponentData {
        public int XSize { get; }
        public int YSize { get; }

        public GridParametersComponent(int xSize, int ySize) {
            XSize = xSize;  
            YSize = ySize;
        }
    }
}
