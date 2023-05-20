using System;

namespace Server.Shared.Network {
    public static class PacketsCount {
        public static readonly int Count = Enum.GetValues<PacketType>().Length;
    }
}
