using System;
using LiteNetLib;
using PoorMansECS.Systems;
using Server.Game.Components;
using Server.Game.Entities;
using Server.Shared.Network;
using ServerShared.Shared.Network;

namespace Server.Game.Systems {
    public class RoomJoinHandlerSystem : SystemBase, INetMessageListener {
        private IncomingMessagesPipe _incomingMessagesPipe;
        private OutgoingMessagesPipe _outgoingMessagesPipe;

        public RoomJoinHandlerSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(IncomingMessagesPipe incomingMessagesPipe, OutgoingMessagesPipe outgoingMessagesPipe) {
            _incomingMessagesPipe = incomingMessagesPipe;
            _outgoingMessagesPipe = outgoingMessagesPipe;
        }
        
        protected override void OnStart() {
            _incomingMessagesPipe.Register(MessageType.JoinRequestMessage, this);
        }

        protected override void OnUpdate(float delta) { }

        protected override void OnStop() {
            _incomingMessagesPipe.Unregister(MessageType.JoinRequestMessage, this);
        }

        public void ReceiveMessage(MessageWrapper message) {
            if (message.Message.Type == MessageType.JoinRequestMessage)
                ProcessJoin(message);
        }

        private void ProcessJoin(MessageWrapper joinMessage) {
            var room = _context.World.Entities.GetFirst<Room>();
            var joinedPlayers = room.GetComponent<JoinedPlayersComponent>();
            if (joinedPlayers.JoinedPlayers.Count >= 2) {
                Console.WriteLine("Error: 2 players already joined");
                return;
            }
            if (joinedPlayers.JoinedPlayers.ContainsKey(joinMessage.AssociatedPeer.Id)) {
                Console.WriteLine("Join error: player already joined");
                return;
            }

            var player = CreateNewPlayer(joinedPlayers.JoinedPlayers.Count, joinMessage.AssociatedPeer, _context);
            joinedPlayers.JoinedPlayers.Add(joinMessage.AssociatedPeer.Id, player);

            Console.WriteLine($"Player {joinMessage.AssociatedPeer.Id} joined. Game side: {player.GetComponent<GameSideComponent>().GameSide}");

            if (RoomFilled(joinedPlayers.JoinedPlayers.Count))
                _context.EventBus.SendEvent(new RoomFilledEvent());

            _outgoingMessagesPipe.SendResponse(joinMessage.AssociatedPeer, joinMessage, 
                new AcceptJoinMessage((byte)player.GetComponent<GameSideComponent>().GameSide));
        }

        private Player CreateNewPlayer(int joinedPlayersCount, NetPeer peer, SystemsContext context) {
            var player = context.World.CreateEntity<Player>();
            player.SetComponent(new GameSideComponent(joinedPlayersCount == 0 ? GameSide.Cross : GameSide.Nought));
            player.SetComponent(new AssociatedPeerComponent(peer));
            return player;
        }

        private bool RoomFilled(int playersCount) {
            return playersCount >= 2;
        }
    }
}