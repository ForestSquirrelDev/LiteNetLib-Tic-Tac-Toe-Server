using PoorMansECS.Components;

namespace Server.Game.Components {
    public readonly struct GameStateComponent : IComponentData {
        public GameState State { get; }

        public GameStateComponent(GameState state) {
            State = state;
        }
        
        public enum GameState {
            Idle = 0, Ongoing = 1, Ended = 2
        }
    }
}
