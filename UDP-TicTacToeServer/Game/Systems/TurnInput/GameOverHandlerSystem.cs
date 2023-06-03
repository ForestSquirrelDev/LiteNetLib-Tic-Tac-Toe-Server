using System;
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
    public class GameOverHandlerSystem : SystemBase, ISystemsEventListener {
        private List<List<(int row, int column)>> _winningCombinations;
        private OutgoingMessagesPipe _outgoingMessagesPipe;

        public GameOverHandlerSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(OutgoingMessagesPipe outgoingMessagesPipe) {
            _outgoingMessagesPipe = outgoingMessagesPipe;
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
            
            var gameSide = turnFinishedEvent.FinishedSide;
            var gridCells = _context.World.Entities.GetFirst<Grid>().GetComponent<GridCellsComponent>().GetCellsCopy();
            var (isWin, combination) = CheckWin(_winningCombinations, gameSide, gridCells);
            if (isWin) {
                ModifyGameState(_context);
                BroadcastGameEnd(_context, _outgoingMessagesPipe, gameSide, combination);
                Console.WriteLine($"Broadcasted win of {gameSide}");
                return;
            }

            var isDraw = AllCellsOccupied(gridCells);
            if (isDraw) {
                ModifyGameState(_context);
                BroadcastGameEnd(_context, _outgoingMessagesPipe, GameSide.None, combination);
                Console.WriteLine("Broadcasted draw");
            }
        }

        private void ModifyGameState(SystemsContext context) {
            context.World.Entities.GetFirst<Room>().SetComponent(new GameStateComponent(GameStateComponent.GameState.Ended));
        }

        private void BroadcastGameEnd(SystemsContext context, OutgoingMessagesPipe outgoingMessagesPipe, GameSide winningSide, List<(int row, int column)> winningCombination) {
            var playerAssociatedPeers = context.World.Entities.GetAll<Player>().Select(player => player.GetComponent<AssociatedPeerComponent>().Peer);
            var message = new GameOverMessage((byte)winningSide, winningCombination);
            outgoingMessagesPipe.SendToAllOneWay(playerAssociatedPeers, message, DeliveryMethod.ReliableOrdered);
            context.EventBus.SendEvent(new GameOverEvent(winningSide));
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
        
        private (bool isWin, List<(int row, int column)> combination) CheckWin(List<List<(int row, int column)>> winningCombinations, GameSide gameSide, GridCell[,] cells) {
            var winningCombination = new List<(int row, int column)>();
            foreach (var combo in winningCombinations) {
                var streakCount = 0;
                foreach (var (row, column) in combo) {
                    var cell = cells[row, column];
                    if (cell.OccupationInfo.IsOccupied && cell.OccupationInfo.Occupator == gameSide) {
                        streakCount++;
                        winningCombination.Add((row, column));
                        Console.WriteLine($"Cell {cell.Row}/{cell.Column} occupied by {gameSide.ToString()}. Streak count: {streakCount}");
                    }
                        
                    if (streakCount >= combo.Count) {
                        Console.WriteLine($"Gameside {gameSide.ToString()}: win");
                        return (true, winningCombination);
                    }
                }
                winningCombination.Clear();
                Console.WriteLine($"Gameside: {gameSide.ToString()}. {combo[0].row}/{combo[0].column}, {combo[1].row}/{combo[1].column}, {combo[2].row}/{combo[2].column} streak count: {streakCount}");
            }
            return (false, winningCombination);
        }

        private bool AllCellsOccupied(GridCell[,] cells) {
            return cells.Cast<GridCell>().All(cell => cell.OccupationInfo.IsOccupied);
        }
        
        protected override void OnUpdate(float delta) { }

        protected override void OnStop() { }
    }
}
