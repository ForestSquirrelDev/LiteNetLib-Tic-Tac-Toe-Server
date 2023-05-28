using System;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using PoorMansECS.Systems;
using Server.Shared.Network;
using ServerShared.Shared.Network;

namespace Server.Connection
{
    public class ConnectionManager : INetEventListener, IUpdateable {
        private readonly NetManager _server;
        private readonly IncomingPacketsPipe _incomingPacketsPipe;
        private readonly OutgoingPacketsPipe _outgoingPacketsPipe;

        public ConnectionManager(IncomingPacketsPipe incomingPacketsPipe, OutgoingPacketsPipe outgoingPacketsPipe) {
            _server = new NetManager(this);
            _incomingPacketsPipe = incomingPacketsPipe;
            _outgoingPacketsPipe = outgoingPacketsPipe;
        }

        public void Start() {
            _server.Start(9050);
        }

        public void OnPeerConnected(NetPeer peer) {
            Console.WriteLine($"New connection: {peer.EndPoint.Address}");
            _outgoingPacketsPipe.SendOneWay(peer, new ConnectionEstablishedMessage(), DeliveryMethod.ReliableOrdered);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            Console.WriteLine($"Peer disconnected: {peer.EndPoint.Address}. Reason: {disconnectInfo.Reason}");
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
            Console.WriteLine($"OnNetworkError: {socketError.ToString()}");
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod) {
            _incomingPacketsPipe.ProcessMessage(peer, reader, deliveryMethod);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
        }

        public void OnConnectionRequest(ConnectionRequest request) {
            request.Accept();
        }

        public void Update(float delta) {
            _server.PollEvents();
        }

        public void Dispose() {
            _server.Stop(true);
        }
    }
}
