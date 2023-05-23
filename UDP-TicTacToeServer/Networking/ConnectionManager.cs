using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using PoorMansECS.Systems;
using ServerShared.Shared.Network;

namespace Server.Connection
{
    public class ConnectionManager : INetEventListener, IUpdateable {
        private readonly NetManager _server;
        private readonly NetDataWriter _writer;
        private readonly IncomingPacketsPipe _packetsPipe;

        public ConnectionManager(IncomingPacketsPipe packetsPipe) {
            _server = new NetManager(this);
            _writer = new NetDataWriter(true);
            _packetsPipe = packetsPipe;
        }

        public void Start() {
            _server.Start(9050);
        }

        public void OnPeerConnected(NetPeer peer) {
            _writer.Reset();
            _writer.Put("Hello client");
            peer.Send(_writer, DeliveryMethod.ReliableOrdered);
            Console.WriteLine($"New connection: {peer.EndPoint.Address}");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            Console.WriteLine($"Peer disconnected: {peer.EndPoint.Address}. Reason: {disconnectInfo.Reason}");
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod) {
            _packetsPipe.ProcessMessage(peer, reader, deliveryMethod);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
        }

        public void OnConnectionRequest(ConnectionRequest request) {
            if (_server.ConnectedPeersCount >= 2) {
                _writer.Reset();
                _writer.Put("Two players are already connected");
                request.Reject(_writer);
                return;
            }
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
