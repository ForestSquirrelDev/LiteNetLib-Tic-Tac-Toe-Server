using Game.Components;
using PoorMansECS.Systems;
using Server.Game.Entities;

namespace Server.Game.Systems.Events {
    public readonly struct TurnFinishedEvent : ISystemEvent {
        public GridCell Cell { get; }
        public Player AssociatedPlayer { get; }

        public TurnFinishedEvent(GridCell cell, Player associatedPlayer) {
            Cell = cell;
            AssociatedPlayer = associatedPlayer;
        }
    }
}
