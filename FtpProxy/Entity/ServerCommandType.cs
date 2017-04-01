namespace FtpProxy.Entity
{
    public enum ServerCommandType
    {
        Unknown,
        Waiting,
        WaitingForClient,
        Success,
        Error
    }
}