using LiteNetLib;
using PoorMansECS.Components;

namespace Server.Game.Components {
    public readonly struct AssociatedPeerComponent : IComponentData {
        public NetPeer Peer { get; }

        public AssociatedPeerComponent(NetPeer peer) {
            Peer = peer;
        }
    }
}