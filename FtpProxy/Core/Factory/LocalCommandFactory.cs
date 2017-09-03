using FtpProxy.Core.Builders;
using FtpProxy.Core.Commands;
using FtpProxy.Entity;
using FtpProxy.Service.Resolvers;
using UsersLib.Service.Checkers;

namespace FtpProxy.Core.Factory
{
    public class LocalCommandFactory : ICommandFactory
    {
        private readonly ICommandArgsResolver _commandArgsResolver;
        private readonly IUserChecker _userChecker;
        private readonly IServerConnectionBuilder _serverConnectionBuilder;
        private readonly ICommandFactory _remoteCommandFactory;

        public LocalCommandFactory(ICommandArgsResolver commandArgsResolver, 
            IUserChecker userChecker, 
            IServerConnectionBuilder serverConnectionBuilder, 
            IRemoteCommandFactory remoteCommandFactory)
        {
            _commandArgsResolver = commandArgsResolver;
            _userChecker = userChecker;
            _serverConnectionBuilder = serverConnectionBuilder;
            _remoteCommandFactory = remoteCommandFactory;
        }

        public ICommand CreateCommand(IFtpMessage ftpMessage, IExecutorState executorState)
        {
            if (ftpMessage == null)
            {
                return new WelcomeCommand(executorState);
            }

            switch (ftpMessage.CommandName)
            {
                case ProcessingClientCommand.Auth:
                    return new AuthCommand(executorState, ftpMessage);
                case ProcessingClientCommand.User:
                    return new UserCommand(executorState, ftpMessage, _commandArgsResolver);
                case ProcessingClientCommand.Pass:
                    return new PassCommand(executorState,
                        ftpMessage,
                        _userChecker,
                        _commandArgsResolver,
                        _serverConnectionBuilder,
                        _remoteCommandFactory);
                default:
                    return new UnauthorizedCommand(executorState, ftpMessage);
            }
        }
    }
}