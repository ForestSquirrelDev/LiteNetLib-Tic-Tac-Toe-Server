using PoorMansECS.Systems;
using ServerShared.Shared.Network;

namespace Server.Game.Systems.Events {
    public readonly struct TurnFinishedEvent : ISystemEvent {
        public GameSide FinishedSide { get; }

        public TurnFinishedEvent(GameSide gameSide) {
            FinishedSide = gameSide;
        }
    }
}
