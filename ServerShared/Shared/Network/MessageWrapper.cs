using LiteNetLib;

namespace ServerShared.Shared.Network {
    public readonly struct MessageWrapper
    {
        public NetPeer AssociatedPeer { get; }
        public CommunicationInfo CommunicationInfo { get; }
        public IMessage Message { get; }
        public DeliveryMethod DeliveryMethod { get; }

        public MessageWrapper(NetPeer associatedPeer, CommunicationInfo communicationInfo, IMessage message, DeliveryMethod deliveryMethod)
        {
            AssociatedPeer = associatedPeer;
            CommunicationInfo = communicationInfo;
            Message = message;
            DeliveryMethod = deliveryMethod;
        }
    }
}