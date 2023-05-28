using LiteNetLib.Utils;
using ServerShared.Shared.Network;

namespace Server.Shared.Network {
    public struct AcceptJoinMessage : IMessage {
        public MessageType Type => MessageType.AcceptJoinMessage;
        public byte GameSide { get; private set; }

        public AcceptJoinMessage(byte gameSide) {
            GameSide = gameSide;
        }

        public void Deserialize(NetDataReader reader) {
            GameSide = reader.GetByte();
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put(GameSide);
        }
    }
}