using LiteNetLib;
using Server.Shared.Network;
using static ServerShared.Shared.Network.Packets;

namespace ServerShared.Shared.Network {
    public class IncomingPacketsPipe
    {
        private readonly Dictionary<MessageType, HashSet<INetMessageListener>> _listeners = new();

        public void ProcessMessage(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            var messageTypeRaw = reader.GetShort();
            if (messageTypeRaw >= MessageTypesCount.Count)
            {
                Console.WriteLine("Error: packet type exceeds packet types count");
                return;
            }
            var messageType = (MessageType)messageTypeRaw;
            if (!_listeners.TryGetValue(messageType, out var typeListeners)) return;

            var messageWrapper = ConstructMessageWrapper(peer, messageType, reader, deliveryMethod);

            foreach (var listener in typeListeners)
                listener.ReceiveMessage(messageWrapper);
        }

        public void Register(MessageType messageType, INetMessageListener listener)
        {
            if (!_listeners.ContainsKey(messageType))
                _listeners[messageType] = new HashSet<INetMessageListener>();
            _listeners[messageType].Add(listener);
        }

        public void Unregister(MessageType packetType, INetMessageListener listener)
        {
            if (_listeners.TryGetValue(packetType, out var typeListeners))
                typeListeners.Remove(listener);
        }

        private MessageWrapper ConstructMessageWrapper(NetPeer peer, MessageType messageType, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            var communicationInfo = new CommunicationInfo();
            communicationInfo.Deserialize(reader);

            IMessage? message = messageType switch
            {
                MessageType.JoinRequestMessage => new JoinRequestMessage(),
                MessageType.GameStartedMessage => new GameStartedMessage(),
                MessageType.AssignGameSideMessage => new AssignGameSideMessage(),
                _ => default
            };
            message.Deserialize(reader);


            return new MessageWrapper(peer, communicationInfo, message, deliveryMethod);
        }
    }
}