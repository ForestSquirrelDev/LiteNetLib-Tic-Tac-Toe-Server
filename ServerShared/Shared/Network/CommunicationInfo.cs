using LiteNetLib.Utils;

namespace ServerShared.Shared.Network
{
    public struct CommunicationInfo : INetSerializable
    {
        public CommunicationDirection Direction { get; private set; }
        public long RandomPacketId { get; private set; }

        public CommunicationInfo(long id, CommunicationDirection direction)
        {
            RandomPacketId = id;
            Direction = direction;
        }

        public void Deserialize(NetDataReader reader)
        {
            Direction = (CommunicationDirection)reader.GetByte();
            RandomPacketId = reader.GetLong();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Direction);
            writer.Put(RandomPacketId);
        }
    }
}