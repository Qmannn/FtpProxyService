using System.Net.Sockets;
using FtpProxy.Connections;
using FtpProxy.Core.Factory;

namespace FtpProxy.Core
{
    public interface IExecutorState
    {
        IConnection ClientConnection { get; }
        IDataConnection ClientDataConnection { get; }
        TcpListener DataConnectionListener { get; }
        IConnection ServerConnection { get; }
        IDataConnection ServerDataConnection { get; }
        ICommandFactory CommandFactory { get; }
    }
}