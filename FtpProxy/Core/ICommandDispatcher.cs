using FtpProxy.Core.Commands;
using FtpProxy.Entity;

namespace FtpProxy.Core
{
    public interface ICommandDispatcher
    {
        ICommand DispatchClientCommand(IFtpMessage ftpMessage, IExecutorState executorState);
        ICommand DispatchServerCommand(IFtpMessage ftpMessage, IExecutorState executorState);
    }
}