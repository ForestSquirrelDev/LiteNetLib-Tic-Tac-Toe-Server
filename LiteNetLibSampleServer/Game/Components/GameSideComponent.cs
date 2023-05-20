using PoorMansECS.Components;

namespace Server.Game.Components {
    public readonly struct GameSideComponent : IComponentData {
        public GameSide GameSide { get; }

        public GameSideComponent(GameSide gameSide) {
            GameSide = gameSide;
        }
    }
}