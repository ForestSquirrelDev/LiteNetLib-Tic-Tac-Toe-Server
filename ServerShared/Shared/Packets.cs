namespace Server.Shared.Network {
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
}