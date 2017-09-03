using FtpProxy.Core.Commands;
using FtpProxy.Core.DataConnection;
using FtpProxy.Core.Helpers;
using FtpProxy.Entity;

namespace FtpProxy.Core.Factory
{
    public class RemoteCommandFactory : IRemoteCommandFactory
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ICommandExecutorHelper _commandExecutorHelper;
        private readonly IDataOperationExecutor _dataOperationExecutor;

        public RemoteCommandFactory(IConnectionFactory connectionFactory, ICommandExecutorHelper commandExecutorHelper, IDataOperationExecutor dataOperationExecutor)
        {
            _connectionFactory = connectionFactory;
            _commandExecutorHelper = commandExecutorHelper;
            _dataOperationExecutor = dataOperationExecutor;
        }


        public ICommand CreateCommand(IFtpMessage ftpMessage, IExecutorState executorState)
        {
            switch (ftpMessage.CommandName)
            {
                case ProcessingClientCommand.Prot:
                    return new ProtCommand(executorState, ftpMessage);
                case ProcessingClientCommand.Pbsz:
                    return new PbszCommand(executorState, ftpMessage);
                case ProcessingClientCommand.Rein: // Блок недоступных команд
                    return new UnavailableCommand(executorState, ftpMessage);
                case ProcessingClientCommand.Port:
                case ProcessingClientCommand.Eprt:
                    return new ActiveDataConnectionCommand(executorState, ftpMessage, _connectionFactory,
                        _commandExecutorHelper);
                case ProcessingClientCommand.Pasv:
                case ProcessingClientCommand.Epsv:
                    return new PassiveDataConnectionCommand(executorState, ftpMessage, _connectionFactory,
                        _commandExecutorHelper);

            }
            return new OtherCommand(executorState, ftpMessage, _dataOperationExecutor);
        }
    }
}