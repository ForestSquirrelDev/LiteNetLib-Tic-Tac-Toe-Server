using LiteNetLib;
using PoorMansECS.Components;
using PoorMansECS.Systems;
using Server.Game.Components;
using Server.Game.Entities;
using Server.Shared.Network;
using ServerShared.Shared.Network;

namespace Server.Game.Systems {
    public class RoomJoinHandlerSystem : SystemBase, INetMessageListener {
        private IncomingPacketsPipe _incomingPacketsPipe;
        private OutgoingPacketsPipe _outgoingPacketsPipe;

        public RoomJoinHandlerSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(IncomingPacketsPipe incomingPacketsPipe, OutgoingPacketsPipe outgoingPacketsPipe) {
            _incomingPacketsPipe = incomingPacketsPipe;
            _outgoingPacketsPipe = outgoingPacketsPipe;
        }
        
        protected override void OnStart() {
            _incomingPacketsPipe.Register(MessageType.JoinRequestMessage, this);
        }

        protected override void OnUpdate(float delta) { }

        protected override void OnStop() {
            _incomingPacketsPipe.Unregister(MessageType.JoinRequestMessage, this);
        }

        public void ReceiveMessage(MessageWrapper message) {
            if (message.Message.Type == MessageType.JoinRequestMessage)
                ProcessJoin(message);
        }

        private void ProcessJoin(MessageWrapper joinMessage) {
            var room = _context.Entities.GetFirst<Room>();
            var joinedPlayers = room.GetComponent<JoinedPlayersComponent>();
            if (joinedPlayers.JoinedPlayers.Count >= 2) {
                Console.WriteLine("Error: 2 players already joined");
                return;
            }

            var player = CreateNewPlayer(joinedPlayers.JoinedPlayers.Count, joinMessage.AssociatedPeer);
            joinedPlayers.JoinedPlayers.Add(player);
            _context.Entities.Add(player);

            Console.WriteLine($"Player {joinMessage.AssociatedPeer.Id} joined");

            if (RoomFilled(joinedPlayers.JoinedPlayers.Count))
                _context.EventBus.SendEvent(new RoomFilledEvent());

            _outgoingPacketsPipe.SendResponse(joinMessage.AssociatedPeer, joinMessage, 
                new AssignGameSideMessage((byte)player.GetComponent<GameSideComponent>().GameSide));
        }

        private Player CreateNewPlayer(int joinedPlayersCount, NetPeer peer) {
            var playerComponents = new List<IComponentData> {
                new GameSideComponent(joinedPlayersCount == 0 ? GameSide.Cross : GameSide.Nought),
                new AssociatedPeerComponent(peer)
            };
            var player = new Player(playerComponents);
            return player;
        }

        private bool RoomFilled(int playersCount) {
            return playersCount >= 2;
        }
    }
}