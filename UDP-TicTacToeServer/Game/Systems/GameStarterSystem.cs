using Game.Components;
using Game.Entities;
using LiteNetLib;
using PoorMansECS.Systems;
using Server.Game.Components;
using Server.Game.Entities;
using Server.Shared.Network;
using ServerShared.Shared.Network;

namespace Server.Game.Systems {
    public class NextTurnHandlerSystem : SystemBase, INetMessageListener {
        private IncomingPacketsPipe _incomingPacketsPipe;
        private OutgoingPacketsPipe _outgoingPacketsPipe;

        public NextTurnHandlerSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(IncomingPacketsPipe incomingPacketsPipe, OutgoingPacketsPipe outgoingPacketsPipe) {
            _incomingPacketsPipe = incomingPacketsPipe;
            _outgoingPacketsPipe = outgoingPacketsPipe;
        }

        protected override void OnStart() {
            _incomingPacketsPipe.Register(MessageType.InputMessage, this);
        }

        public void ReceiveMessage(MessageWrapper messageWrapper) {
            var inputMessage = (InputMessage)messageWrapper.Message;
            var associatedPlayer = _context.Entities.GetFirst<Player>(
                player => player.GetComponent<AssociatedPeerComponent>().Peer.Id == messageWrapper.AssociatedPeer.Id);
            var gameSide = associatedPlayer.GetComponent<GameSideComponent>();
            var room = _context.Entities.GetFirst<Room>();
            var nextTurn = room.GetComponent<NextTurnComponent>();
            if (gameSide.GameSide != nextTurn.NextTurnSide) {
                var responseMessage = new InputResponseMessage(false, InputResponseMessage.Reason.WrongTurn);
                SendResponse(associatedPlayer.GetComponent<AssociatedPeerComponent>().Peer, _outgoingPacketsPipe, messageWrapper, responseMessage);
                return;
            }
        }

        protected override void OnUpdate(float delta) { }

        protected override void OnStop() { }

        private void SendResponse(NetPeer peer, OutgoingPacketsPipe outgoingPacketsPipe, MessageWrapper request, IMessage response) {

        }
    }
    public class GameStarterSystem : SystemBase, ISystemsEventListener {
        private OutgoingPacketsPipe _outgoingPacketsPipe;

        public GameStarterSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(OutgoingPacketsPipe outgoingPacketsPipe) {
            _outgoingPacketsPipe = outgoingPacketsPipe;
        }

        protected override void OnStart() {
            _context.EventBus.Subscribe<RoomFilledEvent>(this);
        }

        protected override void OnUpdate(float delta) { }

        protected override void OnStop() {
            _context.EventBus.Unsubscribe<RoomFilledEvent>(this);
        }

        public void ReceiveEvent<T>(T systemEvent) where T : ISystemEvent {
            if (systemEvent is not RoomFilledEvent)
                return;

            var room = _context.Entities.GetFirst<Room>();
            var joinedPlayers = room.GetComponent<JoinedPlayersComponent>();
            var grid = CreateGrid();
            _context.Entities.Add(grid);

            var initialRandomTurn = RandomizeFirstTurn();
            room.SetComponent(new NextTurnComponent((GameSide)initialRandomTurn));
            BroadcastStartToPeers(grid, joinedPlayers, initialRandomTurn);
        }

        private Grid CreateGrid() {
            var grid = new Grid();
            grid.SetComponent(new GridCellsComponent(3, 3));
            grid.SetComponent(new GridParametersComponent(3, 3));
            return grid;
        }

        private void BroadcastStartToPeers(Grid grid, JoinedPlayersComponent joinedPlayers, byte initialRandomTurn) {
            var associatedPeers = joinedPlayers.JoinedPlayers.Select(player => player.GetComponent<AssociatedPeerComponent>().Peer);
            var communicationInfo = new CommunicationInfo(-1, CommunicationDirection.OneWay);
            var gridParameters = grid.GetComponent<GridParametersComponent>();
            var message = new GameStartedMessage(gridParameters.XSize, gridParameters.YSize, initialRandomTurn);
            _outgoingPacketsPipe.SendToAllOneWay(associatedPeers, message, LiteNetLib.DeliveryMethod.ReliableOrdered);
        }

        private byte RandomizeFirstTurn() {
            var dt = System.DateTime.Now.Ticks;
            return (byte)(dt % 2 == 0 ? 0 : 1);
        }
    }
}