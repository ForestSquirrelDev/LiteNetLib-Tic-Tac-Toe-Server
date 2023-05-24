using LiteNetLib.Utils;

namespace ServerShared.Shared.Network
{
    public struct CommunicationInfo : INetSerializable
    {
        public CommunicationDirection Direction { get; private set; }
        public int RandomPacketId { get; private set; }

        public CommunicationInfo(int id, CommunicationDirection direction)
        {
            RandomPacketId = id;
            Direction = direction;
        }

        public void Deserialize(NetDataReader reader)
        {
            Direction = (CommunicationDirection)reader.GetByte();
            RandomPacketId = reader.GetInt();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Direction);
            writer.Put(RandomPacketId);
        }
    }
}