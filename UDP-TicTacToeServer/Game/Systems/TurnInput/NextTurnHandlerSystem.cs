using Game.Components;
using Game.Entities;
using PoorMansECS.Systems;
using Server.Game.Components;
using Server.Game.Entities;
using Server.Game.Systems.Events;
using Server.Shared.Network;
using ServerShared.Shared.Network;

namespace Server.Game.Systems.TurnInput {
    public class NextTurnHandlerSystem : SystemBase, ISystemsEventListener {
        private OutgoingPacketsPipe _outgoingPacketsPipe;

        public NextTurnHandlerSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(OutgoingPacketsPipe outgoingPacketsPipe) {
            _outgoingPacketsPipe = outgoingPacketsPipe;
        }

        public void ReceiveEvent<T>(T systemEvent) where T : ISystemEvent {
            if (systemEvent is not PlayerInputEvent playerInput) {
                return;
            }
            var requestMessage = playerInput.RequestMessage;

            var room = _context.World.Entities.GetFirst<Room>();
            var nextTurn = room.GetComponent<NextTurnComponent>();
            var associatedPlayer = _context.World.Entities.GetFirst<Player>(
                p => p.GetComponent<AssociatedPeerComponent>().Peer.Id == playerInput.RequestMessage.AssociatedPeer.Id);
            if (playerInput.GameSide != nextTurn.NextTurnSide) {
                var responseMessage = new InputResponseMessage(false, InputResponseMessage.Reason.WrongTurn);
                _outgoingPacketsPipe.SendResponse(associatedPlayer.GetComponent<AssociatedPeerComponent>().Peer, requestMessage, responseMessage);
                return;
            }

            var grid = _context.World.Entities.GetFirst<Grid>();
            var cell = grid.GetComponent<GridCellsComponent>().GetCell(playerInput.CellRow, playerInput.CellColumn);
            cell.SetOccupationInfo(playerInput.GameSide);

            _outgoingPacketsPipe.SendResponse(requestMessage.AssociatedPeer, requestMessage, new InputResponseMessage(true, InputResponseMessage.Reason.None));
            _context.EventBus.SendEvent(new TurnFinishedEvent(cell, associatedPlayer));
        }

        protected override void OnStart() {
            _context.EventBus.Subscribe<PlayerInputEvent>(this);
        }

        protected override void OnStop() { }

        protected override void OnUpdate(float delta) { }
    }
}
