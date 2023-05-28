namespace ServerShared.Shared.Network {
    public enum MessageType
    {
        None = -1, JoinRequestMessage = 0, GameStartedMessage = 1, InputMessage = 2, AcceptJoinMessage = 3, InputResponseMessage = 4, GameOverMessage = 5,
        ConnectionEstablishedMessage = 6, TurnFinished = 7
    }
}
