namespace ServerShared.Shared.Network
{
    public interface INetMessageListener
    {
        public void ReceiveMessage(MessageWrapper messageWrapper);
    }
}