using System.Net.Sockets;
using FtpProxy.Connections;
using FtpProxy.Core.Factory;

namespace FtpProxy.Core
{
    public class ExecutorState : IExecutorState
    {
        public IConnection ClientConnection { get; set; }
        public IConnection ServerConnection { get; set; }
        public IDataConnection ClientDataConnection { get; set; }
        public IDataConnection ServerDataConnection { get; set; }
        public TcpListener DataConnectionListener { get; set; }
        public ICommandFactory CommandFactory { get; set; }
        public void CloseConnections()
        {
            if (ClientConnection != null && ClientConnection.IsConnected)
            {
                ClientConnection.CloseConnection();
            }
            if (ServerConnection != null && ServerConnection.IsConnected)
            {
                ServerConnection.CloseConnection();
            }
        }
    }
}