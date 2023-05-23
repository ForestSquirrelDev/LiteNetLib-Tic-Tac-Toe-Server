﻿using LiteNetLib;
using LiteNetLib.Utils;
using ServerShared.Shared.Network;

namespace Server.Shared.Network
{
    public class OutgoingPacketsPipe {
        private readonly NetDataWriter _writer;
        private readonly IncomingPacketsPipe _incomingPacketsPipe;
        private readonly Random _random;

        public OutgoingPacketsPipe(IncomingPacketsPipe incomingPacketsPipe) {
            _writer = new NetDataWriter();
            _incomingPacketsPipe = incomingPacketsPipe;
            _random = new Random();
        }

        public void SendOneWay(NetPeer peer, IMessage message, DeliveryMethod deliveryMethod) {
            var communicationInfo = new CommunicationInfo(_random.NextInt64(), CommunicationDirection.OneWay);
            var messageWrapper = new MessageWrapper(peer, communicationInfo, message, deliveryMethod);
            PrepareWriter(_writer, messageWrapper);
            peer.Send(_writer, deliveryMethod);
        }

        public void SendToAllOneWay(IEnumerable<NetPeer> peers, IMessage message, DeliveryMethod deliveryMethod) {
            var communicationInfo = new CommunicationInfo(_random.NextInt64(), CommunicationDirection.OneWay);
            var messageWrapper = new MessageWrapper(null, communicationInfo, message, deliveryMethod);
            PrepareWriter(_writer, messageWrapper);
            foreach (var peer in peers) {
                peer.Send(_writer, deliveryMethod);
            }
        }

        public async Task<(bool success, MessageWrapper response)> SendAndWaitForResponse(NetPeer peer, IMessage message, MessageType expectedResponse, int timeoutSeconds) {
            var communicationInfo = new CommunicationInfo(_random.NextInt64(), CommunicationDirection.AwaitsResponse);
            var messageWrapper = new MessageWrapper(peer, communicationInfo, message, DeliveryMethod.ReliableOrdered);
            PrepareWriter(_writer, messageWrapper);
            peer.Send(_writer, DeliveryMethod.ReliableOrdered);

            var temporaryAwaiter = new TemporaryResponseAwaiter(messageWrapper.CommunicationInfo.RandomPacketId, expectedResponse, _incomingPacketsPipe);
            temporaryAwaiter.StartWaiting();
            var result = await WaitForResult(timeoutSeconds, temporaryAwaiter);
            temporaryAwaiter.Dispose();

            return result;
        }

        public void SendResponse(NetPeer peer, MessageWrapper requestMessage, IMessage responseMessage) {
            var communicationInfo = new CommunicationInfo(requestMessage.CommunicationInfo.RandomPacketId, CommunicationDirection.ContainsResponse);
            var messageWrapper = new MessageWrapper(peer, communicationInfo, responseMessage, DeliveryMethod.ReliableOrdered);
            PrepareWriter(_writer, messageWrapper);
            peer.Send(_writer, DeliveryMethod.ReliableOrdered);
        }

        private async Task<(bool receivedResponse, MessageWrapper messageWrapper)> WaitForResult(int timeoutSeconds, TemporaryResponseAwaiter temporaryAwaiter) {
            int timeoutMilliseconds = timeoutSeconds * 1000;
            int passedMilliseconds = 0;
            var result = temporaryAwaiter.GetResponseMessage();
            while (!result.receivedResponse) {
                if (passedMilliseconds >= timeoutMilliseconds) {
                    return (false, default);
                }
                await Task.Delay(10);
                result = temporaryAwaiter.GetResponseMessage();
                passedMilliseconds += 10;
            }
            return (true, result.message);
        }
        
        private void PrepareWriter(NetDataWriter writer, MessageWrapper message) {
            writer.Reset();
            writer.Put((short)message.Message.Type);
            message.CommunicationInfo.Serialize(writer);
            message.Message.Serialize(writer);
        }
    }
}