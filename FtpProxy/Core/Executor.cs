using System;
using FtpProxy.Connections;
using FtpProxy.Core.Commands;
using FtpProxy.Core.Dependency;
using FtpProxy.Core.Factory;
using FtpProxy.Log;
using Microsoft.Practices.Unity;

namespace FtpProxy.Core
{
    public class Executor
    {
        private bool _isExecuting;
        private ICommand _executingCommand;

        private IExecutorState State { get; set; }

        public Executor(IConnection clientConnection)
        {
            UnityContainer container = DependencyResolver.Container;
            LocalCommandFactory localCommandFactory = container.Resolve<LocalCommandFactory>();
            State = new ExecutorState
            {
                ClientConnection = clientConnection,
                CommandFactory = localCommandFactory
            };
            clientConnection.ConnectionClosed += State.CloseConnections;
            _executingCommand = GetFirstCommand();
            _isExecuting = true;
        }

        public void StartExecuting(object obj)
        {
            do
            {
                try
                {
                    _executingCommand.Execute();
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message, ex);
                }
                State = _executingCommand.GetExecutorState();
            } while ((_executingCommand = _executingCommand.GetNextCommand(State)) != null && _isExecuting);
        }

        public void StopExecuting()
        {
            _isExecuting = false;
            if (State == null)
            {
                return;
            }

            if (State.ClientConnection != null)
            {
                State.ClientConnection.CloseConnection();
            }

            if (State.ServerConnection != null)
            {
                State.ServerConnection.CloseConnection();
            }

            if (State.ClientDataConnection != null && State.ClientDataConnection.Connection != null)
            {
                State.ClientDataConnection.Connection.CloseConnection();
            }

            if (State.ServerDataConnection != null && State.ServerDataConnection.Connection != null)
            {
                State.ServerDataConnection.Connection.CloseConnection();
            }
        }

        private ICommand GetFirstCommand()
        {
            return State.CommandFactory.CreateCommand(null, State);
        }
    }
}