﻿using LiteNetLib.Utils;

namespace ServerShared.Shared.Network {
    public readonly struct JoinRequestMessage : IMessage
    {
        public MessageType Type => MessageType.JoinRequestMessage;
        public void Deserialize(NetDataReader reader) { }
        public void Serialize(NetDataWriter writer) { }
    }
}