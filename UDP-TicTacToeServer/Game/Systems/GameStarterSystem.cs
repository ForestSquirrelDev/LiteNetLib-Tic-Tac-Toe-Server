using System.Linq;
using Game.Components;
using Game.Entities;
using PoorMansECS.Systems;
using Server.Game.Components;
using Server.Game.Entities;
using Server.Shared.Network;
using ServerShared.Shared.Network;

namespace Server.Game.Systems {
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

            var room = _context.World.Entities.GetFirst<Room>();
            var joinedPlayers = room.GetComponent<JoinedPlayersComponent>();

            var initialRandomTurn = RandomizeFirstTurn();
            room.SetComponent(new NextTurnComponent((GameSide)initialRandomTurn));
            room.SetComponent(new GameStateComponent(GameStateComponent.GameState.Ongoing));
            BroadcastStartToPeers(_context.World.Entities.GetFirst<Grid>(), joinedPlayers, initialRandomTurn);
        }

        private void BroadcastStartToPeers(Grid grid, JoinedPlayersComponent joinedPlayers, byte initialRandomTurn) {
            var associatedPeers = joinedPlayers.JoinedPlayers.Values.Select(player => player.GetComponent<AssociatedPeerComponent>().Peer);
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