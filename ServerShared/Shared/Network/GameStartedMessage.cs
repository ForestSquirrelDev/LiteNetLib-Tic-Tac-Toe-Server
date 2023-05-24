using LiteNetLib.Utils;

namespace ServerShared.Shared.Network {
    public struct GameStartedMessage : IMessage {
        public MessageType Type => MessageType.GameStartedMessage;
        public int GridSizeX { get; private set; }
        public int GridSizeY { get; private set; }
        public byte FirstTurnSide { get; private set; }

        public GameStartedMessage(int gridSizeX, int gridSizeY, byte firstTurnSide) {
            GridSizeX = gridSizeX;
            GridSizeY = gridSizeY;
            FirstTurnSide = firstTurnSide;
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put(GridSizeX);
            writer.Put(GridSizeY);
            writer.Put(FirstTurnSide);
        }

        public void Deserialize(NetDataReader reader) {
            GridSizeX = reader.GetInt();
            GridSizeY = reader.GetInt();
            FirstTurnSide = reader.GetByte();
        }
    }
}