using System.Collections.Generic;
using LiteNetLib.Utils;
using ServerShared.Shared.Network;

namespace Server.Shared.Network {
    public struct GameOverMessage : IMessage {
        public MessageType Type => MessageType.GameOverMessage;
        public List<(int row, int column)> WinningCombination { get; private set; }
        public byte Winner { get; private set; }

        public GameOverMessage(byte winner, List<(int row, int column)> winningCombination) {
            Winner = winner;
            WinningCombination = winningCombination;
        }
        
        public void Serialize(NetDataWriter writer) {
            writer.Put(Winner);
            
            var combinationLength = (short)WinningCombination.Count;
            writer.Put(combinationLength);
            for (int i = 0; i < combinationLength; i++) {
                var (row, column) = WinningCombination[i];
                writer.Put(row);
                writer.Put(column);
            }
        }

        public void Deserialize(NetDataReader reader) {
            Winner = reader.GetByte();

            var combinationLength = reader.GetShort();
            var winningCombination = new List<(int row, int column)>();
            for (int i = 0; i < combinationLength; i++) {
                var row = reader.GetInt();
                var column = reader.GetInt();
                winningCombination.Add((row, column));
            }
            WinningCombination = winningCombination;
        }
    }
}
