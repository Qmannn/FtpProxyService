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
            _executingCommand = GetFirstCommand();
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
            } while ((_executingCommand = _executingCommand.GetNextCommand(State)) != null);
        }

        private ICommand GetFirstCommand()
        {
            return State.CommandFactory.CreateCommand(null, State);
        }
    }
}