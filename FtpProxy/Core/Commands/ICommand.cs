using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public interface ICommand
    {
        void Execute();
        IExecutorState GetExecutorState();
        ICommand GetNextCommand();
    }
}