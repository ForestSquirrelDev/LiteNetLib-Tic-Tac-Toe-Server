using PoorMansECS.Systems;
using Server.Game.Components;
using ServerShared.Shared.Network;

namespace Server.Game.Systems.Events {
    public readonly struct PlayerInputEvent : ISystemEvent {
        public GameSide GameSide { get; }
        public int CellRow { get; }
        public int CellColumn { get; }
        public MessageWrapper RequestMessage { get; }

        public PlayerInputEvent(GameSide gameSide, int cellRow, int cellColumn, MessageWrapper requestMessage) {
            GameSide = gameSide;
            CellRow = cellRow;
            CellColumn = cellColumn;
            RequestMessage = requestMessage;
        }
    }
}