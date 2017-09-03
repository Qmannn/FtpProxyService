using FtpProxy.Entity;

namespace FtpProxy.Connections
{
    public class DataConnection : IDataConnection
    {
        public DataConnectionType DataConnectionType { get; private set; }
        public IConnection Connection { get; set; }

        public DataConnection(DataConnectionType type)
        {
            DataConnectionType = type;
        }
    }
}