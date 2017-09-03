using FtpProxy.Connections;

namespace FtpProxy.Core.Builders
{
    public interface IServerConnectionBuilder
    {
        IServerConnectionBuilder BuildConnectionSecurity();
        IServerConnectionBuilder BuildPass(string pass);
        IServerConnectionBuilder BuildRemoteConnection(string url, int port);
        IServerConnectionBuilder BuildUser(string user);
        IConnection GetResult();
    }
}