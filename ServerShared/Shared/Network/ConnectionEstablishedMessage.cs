using LiteNetLib.Utils;

namespace ServerShared.Shared.Network {
    public struct ConnectionEstablishedMessage : IMessage {
        public MessageType Type => MessageType.ConnectionEstablishedMessage;

        public void Serialize(NetDataWriter writer) { }

        public void Deserialize(NetDataReader reader) { }
    }
}
