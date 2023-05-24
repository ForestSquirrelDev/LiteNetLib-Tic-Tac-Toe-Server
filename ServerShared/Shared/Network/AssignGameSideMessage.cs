using LiteNetLib.Utils;
using ServerShared.Shared.Network;

namespace Server.Shared.Network {
    public struct AssignGameSideMessage : IMessage {
        public MessageType Type => MessageType.AssignGameSideMessage;
        public byte GameSide { get; private set; }

        public AssignGameSideMessage(byte gameSide) {
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