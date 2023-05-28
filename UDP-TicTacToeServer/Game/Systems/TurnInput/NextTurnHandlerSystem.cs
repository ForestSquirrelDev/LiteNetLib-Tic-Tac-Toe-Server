using System.Linq;
using Game.Components;
using Game.Entities;
using LiteNetLib;
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
        
        protected override void OnStart() {
            _context.EventBus.Subscribe<PlayerInputEvent>(this);
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
            var associatedPeer = associatedPlayer.GetComponent<AssociatedPeerComponent>().Peer;
            if (playerInput.GameSide != nextTurn.NextTurnSide) {
                var responseMessage = new InputResponseMessage(false, InputResponseMessage.Reason.WrongTurn);
                _outgoingPacketsPipe.SendResponse(associatedPeer, requestMessage, responseMessage);
                return;
            }

            var gameState = room.GetComponent<GameStateComponent>().State;
            if (gameState != GameStateComponent.GameState.Ongoing) {
                var responseMessage = new InputResponseMessage(false, gameState == GameStateComponent.GameState.Idle 
                    ? InputResponseMessage.Reason.GameNotStarted 
                    : InputResponseMessage.Reason.GameFinished);
                _outgoingPacketsPipe.SendResponse(associatedPeer, requestMessage, responseMessage);
                return;
            }

            var grid = _context.World.Entities.GetFirst<Grid>();
            var hasCell = grid.GetComponent<GridCellsComponent>().TryGetCell(playerInput.CellRow, playerInput.CellColumn, out var cell);
            if (!hasCell) {
                var responseMessage = new InputResponseMessage(false, InputResponseMessage.Reason.OutOfBounds);
                _outgoingPacketsPipe.SendResponse(associatedPeer, requestMessage, responseMessage);
                return;
            }
            
            FinalizeTurn(cell, playerInput, room, requestMessage, associatedPlayer);
        }
        
        private void FinalizeTurn(GridCell cell, PlayerInputEvent playerInput, Room room, MessageWrapper requestMessage, Player associatedPlayer) {
            cell.SetOccupationInfo(playerInput.GameSide);
            room.SetComponent(new NextTurnComponent(playerInput.GameSide == GameSide.Cross ? GameSide.Nought : GameSide.Cross));
            _outgoingPacketsPipe.SendResponse(requestMessage.AssociatedPeer, requestMessage, new InputResponseMessage(true, InputResponseMessage.Reason.None));
            var inputFinishedMessage = new TurnFinishedMessage(playerInput.CellRow, playerInput.CellColumn, (byte)playerInput.GameSide);
            _outgoingPacketsPipe.SendToAllOneWay(_context.World.Entities.GetAll<Player>().Select(p => p.GetComponent<AssociatedPeerComponent>().Peer),
                inputFinishedMessage, DeliveryMethod.ReliableOrdered);
            _context.EventBus.SendEvent(new TurnFinishedEvent(cell, associatedPlayer));
        }

        protected override void OnStop() { }
        protected override void OnUpdate(float delta) { }
    }
}
