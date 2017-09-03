using FtpProxy.Entity;

namespace FtpProxy.Connections
{
    public interface IDataConnection
    {
        DataConnectionType DataConnectionType { get; }

        IConnection Connection { get; set; }
    }
}