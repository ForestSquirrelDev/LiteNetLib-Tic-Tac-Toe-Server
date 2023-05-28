using LiteNetLib.Utils;
using ServerShared.Shared.Network;

namespace Server.Shared.Network {
    public struct GameOverMessage : IMessage {
        public MessageType Type => MessageType.GameOverMessage;
        public byte Winner { get; private set; }

        public GameOverMessage(byte winner) {
            Winner = winner;
        }
        
        public void Serialize(NetDataWriter writer) {
            writer.Put(Winner);
        }

        public void Deserialize(NetDataReader reader) {
            Winner = reader.GetByte();
        }
    }
}
