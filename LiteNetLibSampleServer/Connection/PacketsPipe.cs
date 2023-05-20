using System;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLibSampleServer.Shared;

namespace LiteNetLibSampleServer.Connection {
    public class PacketsPipe {
        private readonly Dictionary<PacketType, HashSet<IPacketReceiver>> _receivers = new();

        public void ProcessMessage(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) {
            var packetType = reader.GetShort();
            if (packetType >= PacketsCount.Count) {
                Console.WriteLine("!Packet type exceeds packet types count");
                return;
            }
            if (!_receivers.TryGetValue((PacketType)packetType, out var receivers)) return;
            
            foreach (var receiver in receivers)
                receiver.Receive(peer, reader, deliveryMethod);
        }

        public void Register(PacketType packetType, IPacketReceiver receiver) {
            if (!_receivers.ContainsKey(packetType))
                _receivers[packetType] = new HashSet<IPacketReceiver>();
            _receivers[packetType].Add(receiver);
        }
    }

    public interface IPacketReceiver {
        public void Receive(NetPeer peer, NetPacketReader reader, DeliveryMethod method);
    }

    public enum PacketType {
        
    }
}
