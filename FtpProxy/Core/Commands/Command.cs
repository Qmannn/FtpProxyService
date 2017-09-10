using System.Collections.Generic;
using FtpProxy.Connections;
using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public abstract class Command : ICommand
    {
        private readonly List<string> _errors; 

        protected readonly IExecutorState ExecutorState;
        protected readonly IFtpMessage FtpMessage;

        protected IConnection ClientConnection
        {
            get { return ExecutorState.ClientConnection; }
        }

        protected IConnection ServerConnection
        {
            get { return ExecutorState.ServerConnection; }
        }

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

        public virtual ICommand GetNextCommand(IExecutorState executorState)
        {
            IFtpMessage ftpMessage = executorState.ClientConnection.GetMessage();
            return ftpMessage != null
                ? executorState.CommandFactory.CreateCommand(ftpMessage, executorState)
                : null;
        }

        protected void SendToServer(IFtpMessage ftpMessage)
        {
            SendMessage(ExecutorState.ServerConnection, ftpMessage);
        }

        protected void SendToClient(IFtpMessage ftpMessage)
        {
            SendMessage(ExecutorState.ClientConnection, ftpMessage);
        }

        protected IFtpMessage GetServerResponce()
        {
            return GetResponce(ExecutorState.ServerConnection);
        }

        protected IFtpMessage GetClientResponce()
        {
            return GetResponce(ExecutorState.ClientConnection);
        }

        protected IFtpMessage SendWithResponceServer(IFtpMessage ftpMessage)
        {
            SendToServer(ftpMessage);
            return GetServerResponce();
        }

        protected IFtpMessage SendWithResponceClient(IFtpMessage ftpMessage)
        {
            SendToClient(ftpMessage);
            return GetClientResponce();
        }

        private void SendMessage(IConnection connection, IFtpMessage message)
        {
            if (connection != null && message != null)
            {
                connection.SendMessage(message);
            }
        }

        private IFtpMessage GetResponce(IConnection connection)
        {
            if (connection == null)
            {
                return null;
            }
            return connection.GetMessage();
        }
    }
}