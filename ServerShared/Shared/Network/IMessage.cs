using LiteNetLib.Utils;

namespace ServerShared.Shared.Network {
    public interface IMessage : INetSerializable {
        MessageType Type { get; }
    }
}