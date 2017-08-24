namespace FtpProxy.Entity
{
    public interface IFtpMessage
    {
        string Args { get; }
        byte[] Bytes { get; }
        string CommandName { get; }
        ServerCommandType CommandType { get; }
    }
}