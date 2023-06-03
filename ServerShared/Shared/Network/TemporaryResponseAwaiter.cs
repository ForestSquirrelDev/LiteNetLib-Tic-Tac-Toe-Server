using ServerShared.Shared.Network;

namespace Server.Shared.Network
{
    public class TemporaryResponseAwaiter : INetMessageListener {
        private readonly long _responseId;
        private readonly IncomingMessagesPipe _incomingMessagesPipe;
        private readonly MessageType _responseMessageType;

        private (bool, MessageWrapper) _responseMessage;

        public TemporaryResponseAwaiter(long responseId, MessageType responseMessageType, IncomingMessagesPipe incomingMessagesPipe) {
            _responseId = responseId;
            _incomingMessagesPipe = incomingMessagesPipe;
            _responseMessageType = responseMessageType;
        }

        public void ReceiveMessage(MessageWrapper message) {
            if (message.CommunicationInfo.RandomPacketId == _responseId && message.CommunicationInfo.Direction == CommunicationDirection.ContainsResponse) {
                _responseMessage = (true, message);
            }
        }

        public void StartWaiting() {
            _incomingMessagesPipe.Register(_responseMessageType, this);
        }

        public (bool receivedResponse, MessageWrapper message) GetResponseMessage() {
            return _responseMessage;
        }

        public void Dispose() {
            _incomingMessagesPipe.Unregister(_responseMessageType, this);
        }
    }
}
