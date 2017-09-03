using System.Net.Sockets;
using FtpProxy.Connections;
using FtpProxy.Core.Factory;

namespace FtpProxy.Core
{
    public interface IExecutorState
    {
        IConnection ClientConnection { get; }
        IDataConnection ClientDataConnection { get; set; }
        TcpListener DataConnectionListener { get; set; }
        IConnection ServerConnection { get; }
        IDataConnection ServerDataConnection { get; set; }
        ICommandFactory CommandFactory { get; }

        void CloseConnections();
    }
}