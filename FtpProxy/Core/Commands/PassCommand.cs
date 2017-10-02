using System;
using System.Security.Authentication;
using FtpProxy.Connections;
using FtpProxy.Core.Builders;
using FtpProxy.Core.Factory;
using FtpProxy.Core.Resolvers;
using FtpProxy.Entity;
using FtpProxy.Log;
using UsersLib.Service.Checkers;
using UsersLib.Service.Checkers.Results;

namespace FtpProxy.Core.Commands
{
    public class PassCommand : Command
    {
        private readonly ICommandArgsResolver _commandArgsResolver;
        private readonly IServerConnectionBuilder _serverConnectionBuilder;
        private readonly ICommandFactory _remoteCommandFactory;
        private readonly IUserChecker _userChecker;

        public PassCommand(IExecutorState executorState, IFtpMessage ftpMessage, IUserChecker userChecker, ICommandArgsResolver commandArgsResolver, IServerConnectionBuilder serverConnectionBuilder, ICommandFactory remoteCommandFactory) : base(executorState, ftpMessage)
        {
            _userChecker = userChecker;
            _commandArgsResolver = commandArgsResolver;
            _serverConnectionBuilder = serverConnectionBuilder;
            _remoteCommandFactory = remoteCommandFactory;
        }

        public override void Execute()
        {
            FtpMessage responce = new FtpMessage("230 успешная авторизация", ClientConnection.Encoding);

            ClientConnection.Password =
                _commandArgsResolver.ResolvePassword(FtpMessage.Args);

            if (String.IsNullOrEmpty(ClientConnection.UserLogin)
                 || String.IsNullOrEmpty(ClientConnection.Password)
                 || String.IsNullOrEmpty(ClientConnection.RemoteServerIdentifier))
            {
                //TODO ctor
                SendToClient(new FtpMessage("530 неверная последовательность команд", ClientConnection.Encoding));
                return;
            }

            IUserCheckerResult checkerResult = _userChecker.Check(ClientConnection.UserLogin,
                ClientConnection.Password,
                ClientConnection.RemoteServerIdentifier);

            if (checkerResult == null)
            {
                SendToClient(new FtpMessage("530 Неверная комбинация логин-пароль", ClientConnection.Encoding));
                return;
            }
            try
            {
                _serverConnectionBuilder.BuildRemoteConnection(checkerResult.UrlAddress, checkerResult.Port);
            }
            catch (AuthenticationException)
            {
                Logger.Log.Error(String.Format("remote server anavailable: {0}| USER: {1}", checkerResult.UrlAddress,
                    ClientConnection.UserLogin));
                SendToClient(new FtpMessage("434 remote server anavailable", ClientConnection.Encoding));
                return;
            }

            try
            {
                _serverConnectionBuilder.BuildConnectionSecurity();
            }
            catch (AuthenticationException e)
            {
                Logger.Log.Error(String.Format("BuildConnectionSecurity: {0}, Host: {1}, User: {2}", e.Message,
                    checkerResult.UrlAddress, ClientConnection.UserLogin));
            }

            try
            {
                _serverConnectionBuilder.BuildUser(checkerResult.Login);
                _serverConnectionBuilder.BuildPass(checkerResult.Pass);
            }
            catch (AuthenticationException e)
            {
                Logger.Log.Error(String.Format("BuildUserPass: {0}; Host: {1}, User: {2}", e.Message,
                    checkerResult.UrlAddress, ClientConnection.UserLogin));
                SendToClient(new FtpMessage("425 incorrect remote server auth data. Please contact the administrator",
                    ClientConnection.Encoding));
                return;
            }
            SendToClient(responce);
        }

        public override IExecutorState GetExecutorState()
        {
            IConnection serverConnection = _serverConnectionBuilder.GetResult();

            // Если не удалось установить соедиение с севрером - вернем то же состояние без изменений
            if (serverConnection == null)
            {
                return base.GetExecutorState();
            }

            IExecutorState executorState = new ExecutorState
            {
                ClientConnection = ClientConnection,
                ServerConnection = _serverConnectionBuilder.GetResult(),
                CommandFactory = _remoteCommandFactory
            };
            executorState.ClientConnection.ConnectionClosed += executorState.CloseConnections;
            executorState.ServerConnection.ConnectionClosed += executorState.CloseConnections;

            return executorState;
        }
    }
}