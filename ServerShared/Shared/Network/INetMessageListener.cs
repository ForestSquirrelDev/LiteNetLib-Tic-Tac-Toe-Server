namespace ServerShared.Shared.Network
{
    public interface INetMessageListener
    {
        void ReceiveMessage(MessageWrapper messageWrapper);
    }
}