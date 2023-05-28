using PoorMansECS.Components;
using ServerShared.Shared.Network;

namespace Server.Game.Components {
    public readonly struct GameSideComponent : IComponentData {
        public GameSide GameSide { get; }

        public GameSideComponent(GameSide gameSide) {
            GameSide = gameSide;
        }
    }
}