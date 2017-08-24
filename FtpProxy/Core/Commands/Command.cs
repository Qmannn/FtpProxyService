using System.Collections.Generic;
using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public abstract class Command : ICommand
    {
        private readonly List<string> _errors; 

        protected readonly IExecutorState ExecutorState;
        protected readonly IFtpMessage FtpMessage;

        protected Command(IExecutorState executorState, IFtpMessage ftpMessage)
        {
            ExecutorState = executorState;
            FtpMessage = ftpMessage;

            _errors = new List<string>();
        }

        public abstract void Execute();

        public virtual IExecutorState GetExecutorState()
        {
            return ExecutorState;
        }

        public virtual ICommand GetNextCommand()
        {
            IFtpMessage ftpMessage = ExecutorState.ClientConnection.GetMessage();
            return ftpMessage != null
                ? ExecutorState.CommandFactory.CreateCommand(ftpMessage, ExecutorState)
                : null;
        }

        protected void AddError(string error)
        {
            _errors.Add(error);
        }
    }
}