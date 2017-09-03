using System.Net.Sockets;
using FtpProxy.Connections;

namespace FtpProxy.Core.DataConnection
{
    public class DataConnectionExecutorState
    {
        public IConnection ClientConnection { get; set; }
        public IConnection ServerConnection { get; set; }
        public IDataConnection ServerDataConnection { get; set; }
        public IDataConnection ClientDataConnection { get; set; }
        public TcpListener DataConnetionListener { get; set; }
    }
}