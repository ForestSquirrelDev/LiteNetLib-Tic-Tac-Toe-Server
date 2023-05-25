using LiteNetLib.Utils;
using ServerShared.Shared.Network;

namespace Server.Shared.Network {
    public struct GameOverMessage : IMessage {
        public MessageType Type => MessageType.GameOverMessage;
        public byte GameSide { get; private set; }

        public GameOverMessage(byte gameSide) {
            GameSide = gameSide;
        }
        
        public void Serialize(NetDataWriter writer) {
            writer.Put(GameSide);
        }

        public void Deserialize(NetDataReader reader) {
            GameSide = reader.GetByte();
        }
    }
}
