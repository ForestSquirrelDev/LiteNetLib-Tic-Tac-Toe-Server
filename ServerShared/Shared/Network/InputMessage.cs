using LiteNetLib.Utils;

namespace ServerShared.Shared.Network
{
    public struct InputMessage : IMessage
    {
        public MessageType Type => MessageType.InputMessage;
        public int Row { get; private set; }
        public int Column { get; private set; }

        public InputMessage(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Row);
            writer.Put(Column);
        }

        public void Deserialize(NetDataReader reader)
        {
            Row = reader.GetInt();
            Column = reader.GetInt();
        }
    }
}