using FtpProxy.Connections;
using FtpProxy.Entity;

namespace FtpProxy.Core.Factory
{
    public interface IConnectionFactory
    {
        IConnection CreateConnection(string url, int port);

        IDataConnection CreateDataConnection(DataConnectionType type);
    }
}