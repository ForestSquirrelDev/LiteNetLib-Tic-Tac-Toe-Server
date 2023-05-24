using System.Collections.Generic;
using Game.Components;
using Game.Entities;
using PoorMansECS.Systems;
using Server.Game.Components;
using Server.Game.Entities;
using Server.Shared.Network;
using ServerShared.Shared.Network;

namespace Server.Game.Systems {
    public class TurnFinishHandlerSystem : SystemBase, ISystemsEventListener {
        private List<List<(int row, int column)>> _winningCombinations;
        private OutgoingPacketsPipe _outgoingPacketsPipe;

        public TurnFinishHandlerSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(OutgoingPacketsPipe outgoingPacketsPipe) {
            _outgoingPacketsPipe = outgoingPacketsPipe;
        }

        protected override void OnStart() {
            _context.EventBus.Subscribe<TurnFinishedEvent>(this);
            
            var grid = _context.World.Entities.GetFirst<Grid>();
            var gridSize = grid.GetComponent<GridParametersComponent>().XSize;
            _winningCombinations = ConstructWinningCombinations(gridSize);
        }
        
        public void ReceiveEvent<T>(T systemEvent) where T : ISystemEvent {
            if (systemEvent is not TurnFinishedEvent turnFinishedEvent)
                return;

            var player = turnFinishedEvent.AssociatedPlayer;
            var cell = turnFinishedEvent.Cell;
        }

        protected override void OnUpdate(float delta) { }

        protected override void OnStop() { }

        private List<List<(int row, int column)>> ConstructWinningCombinations(int uniformGridSize) {
            var winningCombinations = new List<List<(int, int)>>();
            for (int row = 0; row < uniformGridSize; row++)
            {
                var rowCombination = new List<(int, int)>();
                for (int column = 0; column < uniformGridSize; column++) 
                    rowCombination.Add((row, column));
                winningCombinations.Add(rowCombination);
            }
            
            for (int column = 0; column < uniformGridSize; column++)
            {
                var columnCombination = new List<(int, int)>();
                for (int row = 0; row < uniformGridSize; row++) 
                    columnCombination.Add((row, column));
                winningCombinations.Add(columnCombination);
            }
            
            var diagonal1 = new List<(int, int)>();
            var diagonal2 = new List<(int, int)>();
            for (int i = 0; i < uniformGridSize; i++)
            {
                diagonal1.Add((i, i));
                diagonal2.Add((i, uniformGridSize - i - 1));
            }
            winningCombinations.Add(diagonal1);
            winningCombinations.Add(diagonal2);

            return winningCombinations;
        }
    }
    public readonly struct TurnFinishedEvent : ISystemEvent {
        public GridCell Cell { get; }
        public Player AssociatedPlayer { get; }

        public TurnFinishedEvent(GridCell cell, Player associatedPlayer) {
            Cell = cell;
            AssociatedPlayer = associatedPlayer;
        }
    }
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
            var gridCells = grid.GetComponent<GridCellsComponent>().CellsRowColumnWise;
            var cell = gridCells[playerInput.CellRow, playerInput.CellColumn];
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
    public class InputHandlerSystem : SystemBase, INetMessageListener {
        private IncomingPacketsPipe _incomingPacketsPipe;

        public InputHandlerSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(IncomingPacketsPipe incomingPacketsPipe) {
            _incomingPacketsPipe = incomingPacketsPipe;
        }

        protected override void OnStart() {
            _incomingPacketsPipe.Register(MessageType.InputMessage, this);
        }

        public void ReceiveMessage(MessageWrapper requestMessage) {
            var inputMessage = (InputMessage)requestMessage.Message;
            var associatedPlayer = _context.World.Entities.GetFirst<Player>(
                player => player.GetComponent<AssociatedPeerComponent>().Peer.Id == requestMessage.AssociatedPeer.Id);
            var gameSide = associatedPlayer.GetComponent<GameSideComponent>();
            _context.EventBus.SendEvent(new PlayerInputEvent(gameSide.GameSide, inputMessage.Row, inputMessage.Column, requestMessage));
        }

        protected override void OnUpdate(float delta) { }

        protected override void OnStop() { }
    }
}