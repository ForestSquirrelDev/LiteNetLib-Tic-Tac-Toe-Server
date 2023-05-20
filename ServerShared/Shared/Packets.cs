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

        public readonly struct GameStartedPacket {
            public int GridSizeX { get; }
            public int GridSizeY { get; }
            public int GameRole { get; }

            public GameStartedPacket(int gridSizeX, int gridSizeY, int gameRole) {
                GridSizeX = gridSizeX;
                GridSizeY = gridSizeY;
                GameRole = gameRole;
            }
        }

        public readonly struct JoinPacket { }
    }
}