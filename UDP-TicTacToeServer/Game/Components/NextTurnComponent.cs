using PoorMansECS.Components;
using ServerShared.Shared.Network;

namespace Server.Game.Components {
    public readonly struct NextTurnComponent : IComponentData {
        public GameSide NextTurnSide { get; }

        public NextTurnComponent(GameSide nextTurnSide) {
            NextTurnSide = nextTurnSide;
        }
    }
}
