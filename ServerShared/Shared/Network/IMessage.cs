using LiteNetLib.Utils;

namespace ServerShared.Shared.Network {
    public interface IMessage : INetSerializable {
        public MessageType Type { get; }
    }
}