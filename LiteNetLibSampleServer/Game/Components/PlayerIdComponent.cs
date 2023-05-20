using PoorMansECS.Components;

namespace Server.Game.Components {
    public readonly struct PlayerIdComponent : IComponentData {
        public int Id { get; }

        public PlayerIdComponent(int id) {
            Id = id;
        }
    }
}