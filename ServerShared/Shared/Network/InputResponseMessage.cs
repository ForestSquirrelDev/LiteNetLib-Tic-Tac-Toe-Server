using LiteNetLib.Utils;

namespace ServerShared.Shared.Network {
    public struct InputResponseMessage : IMessage {
        public MessageType Type => MessageType.InputResponseMessage;
        public bool Success { get; private set; }
        public Reason ResponseReason { get; private set; }

        public InputResponseMessage(bool success, Reason responseReason) {
            Success = success;
            ResponseReason = responseReason;
        }

        public void Deserialize(NetDataReader reader) {
            Success = reader.GetBool();
            ResponseReason = (Reason)reader.GetByte();
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put(Success);
            writer.Put((byte)ResponseReason);
        }

        public enum Reason {
            None = 0, WrongTurn = 1, OutOfBounds = 2, GameNotStarted = 3, GameFinished = 4
        }
    }
}
