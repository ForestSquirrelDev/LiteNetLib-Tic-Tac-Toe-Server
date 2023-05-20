    using LiteNetLib;

namespace Server.Shared.Network {
    public interface IPacketReceiver {
        public void Receive(NetPeer peer, NetPacketReader reader, PacketType packetType, DeliveryMethod method);
    }
}