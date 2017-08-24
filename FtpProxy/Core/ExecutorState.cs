using System.Net.Sockets;
using FtpProxy.Connections;
using FtpProxy.Core.Factory;
using FtpProxy.Entity;

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
    }
}