using System.Collections.Generic;
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
            
            var gameSide = turnFinishedEvent.Cell.OccupationInfo.Occupator;
            var gridCells = _context.World.Entities.GetFirst<Grid>().GetComponent<GridCellsComponent>().GetCellsCopy();
            var isVictory = CheckWin(_winningCombinations, gameSide, gridCells);
            if (!isVictory) return;

            var playerAssociatedPeers = _context.World.Entities.GetAll<Player>().Select(
                player => player.GetComponent<AssociatedPeerComponent>().Peer);
            var message = new GameOverMessage((byte)gameSide);
            _outgoingPacketsPipe.SendToAllOneWay(playerAssociatedPeers, message, DeliveryMethod.ReliableOrdered);
            _context.World.Entities.GetFirst<Room>().SetComponent(new GameStateComponent(GameStateComponent.GameState.Ended));
            _context.EventBus.SendEvent(new GameOverEvent(gameSide));
        }

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
        
        private bool CheckWin(List<List<(int row, int column)>> winningCombinations, GameSide gameSide, GridCell[,] cells) {
            foreach (var combo in winningCombinations) {
                var streakCount = 0;
                foreach (var (row, column) in combo) {
                    var cell = cells[row, column];
                    if (cell.OccupationInfo.IsOccupied && cell.OccupationInfo.Occupator == gameSide)
                        streakCount++;
                    if (streakCount >= combo.Count - 1)
                        return true;
                }
            }
            return false;
        }
        
        protected override void OnUpdate(float delta) { }

        protected override void OnStop() { }
    }
}
