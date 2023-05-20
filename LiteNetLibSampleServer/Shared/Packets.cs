using System;
using LiteNetLibSampleServer.Connection;

namespace LiteNetLibSampleServer.Shared {
    public static class Packets {
        public readonly struct InputPacket {
            public int Row { get; }
            public int Column { get; }
            
            public InputPacket(int row, int column) {
                Row = row;
                Column = column;
            }
        }

        public readonly struct JoinPacket { }
    }

    public static class PacketsCount {
        public static readonly int Count = Enum.GetValues<PacketType>().Length;
    }
}
