using PoorMansECS.Components;

namespace Server.Game.Components {
    public readonly struct NextTurnComponent : IComponentData {
        public GameSide NextTurnSide { get; }

        public NextTurnComponent(GameSide nextTurnSide) {
            NextTurnSide = nextTurnSide;
        }
    }
}
