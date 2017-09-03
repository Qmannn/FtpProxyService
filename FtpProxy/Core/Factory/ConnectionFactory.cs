using FtpProxy.Connections;
using FtpProxy.Entity;

namespace FtpProxy.Core.Factory
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IConnection CreateConnection(string url, int port)
        {
            return new Connection(url, port);
        }

        public IDataConnection CreateDataConnection(DataConnectionType type)
        {
            return new Connections.DataConnection(type);
        }
    }
}