using FtpProxy.Core.Commands;
using FtpProxy.Entity;

namespace FtpProxy.Core.Factory
{
    public class RemoteCommandFactory : ICommandFactory
    {
        public ICommand CreateCommand(IFtpMessage ftpMessage, IExecutorState executorState)
        {
            throw new System.NotImplementedException();
        }
    }
}