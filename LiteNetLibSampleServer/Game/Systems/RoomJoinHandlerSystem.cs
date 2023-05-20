using LiteNetLib;
using PoorMansECS.Components;
using Server.Game.Components;
using Server.Game.Entities;
using Server.Shared.Network;

namespace Server.Game.Systems {
    public class RoomJoinHandlerSystem : SystemBase, IPacketReceiver {
        public RoomJoinHandlerSystem(SystemsContext context) : base(context) {
            context.PacketsPipe.Register(PacketType.JoinPacket, this);
        }
        
        protected override void OnStart() { }

        protected override void OnUpdate(float delta) { }

        public void Receive(NetPeer peer, NetPacketReader reader, PacketType packetType, DeliveryMethod method) {
            if (packetType == PacketType.JoinPacket)
                ProcessJoin(peer);
        }

        private void ProcessJoin(NetPeer peer) {
            var room = _context.Entities.GetFirst<Room>();
            var joinedPlayers = room.GetComponent<JoinedPlayersComponent>();
            if (joinedPlayers.JoinedPlayers.Count >= 2) {
                Console.WriteLine("Error: 2 players already joined");
                return;
            }

            var player = CreateNewPlayer(joinedPlayers.JoinedPlayers.Count, peer.Id);
            joinedPlayers.JoinedPlayers.Add(player);
            _context.Entities.Add(player);

            Console.WriteLine($"Player {peer.Id} joined");

            if (RoomFilled(joinedPlayers.JoinedPlayers.Count))
                _context.EventBus.SendEvent(new RoomFilledEvent());
        }

        private Player CreateNewPlayer(int joinedPlayersCount, int peerId) {
            var playerComponents = new List<IComponentData> {
                new GameSideComponent(joinedPlayersCount == 0 ? GameSide.Cross : GameSide.Nought),
                new PlayerIdComponent(peerId)
            };
            var player = new Player(playerComponents);
            return player;
        }

        private bool RoomFilled(int playersCount) {
            return playersCount >= 2;
        }
    }
}