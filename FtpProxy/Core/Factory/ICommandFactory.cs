using FtpProxy.Core.Commands;
using FtpProxy.Entity;

namespace FtpProxy.Core.Factory
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(IFtpMessage ftpMessage, IExecutorState executorState);
    }
}