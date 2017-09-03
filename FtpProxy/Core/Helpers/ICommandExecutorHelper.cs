using System.Net;
using FtpProxy.Connections;
using FtpProxy.Entity;

namespace FtpProxy.Core.Helpers
{
    public interface ICommandExecutorHelper
    {
        Connection GetActiveDataConnection(IFtpMessage clientCommand);
        Connection GetEpsvDataConnection(IFtpMessage serverResponce, IPAddress serverIpAddress);
        Connection GetPasvDataConnection(IFtpMessage serverResponce);
    }
}