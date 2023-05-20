using LiteNetLib;

namespace Server.Shared.Network {
    public class PacketsPipe {
        private readonly Dictionary<PacketType, HashSet<IPacketReceiver>> _receivers = new();

        public void ProcessMessage(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) {
            var packetTypeShort = reader.GetShort();
            if (packetTypeShort >= PacketsCount.Count) {
                Console.WriteLine("Error: packet type exceeds packet types count");
                return;
            }
            var packetType = (PacketType)packetTypeShort;
            if (!_receivers.TryGetValue(packetType, out var receivers)) return;
            
            foreach (var receiver in receivers)
                receiver.Receive(peer, reader, packetType, deliveryMethod);
        }

        public void Register(PacketType packetType, IPacketReceiver receiver) {
            if (!_receivers.ContainsKey(packetType))
                _receivers[packetType] = new HashSet<IPacketReceiver>();
            _receivers[packetType].Add(receiver);
        }
    }
}