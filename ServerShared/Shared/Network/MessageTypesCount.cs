using System;

namespace ServerShared.Shared.Network
{
    public static class MessageTypesCount
    {
        public static readonly int Count = Enum.GetValues(typeof(MessageType)).Length;
    }
}
