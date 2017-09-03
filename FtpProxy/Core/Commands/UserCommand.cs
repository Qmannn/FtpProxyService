using FtpProxy.Connections;
using FtpProxy.Entity;
using FtpProxy.Service.Resolvers;

namespace FtpProxy.Core.Commands
{
    public class UserCommand : Command
    {
        private readonly ICommandArgsResolver _commandArgsResolver;

        public UserCommand(IExecutorState executorState, 
            IFtpMessage ftpMessage, 
            ICommandArgsResolver commandArgsResolver) 
            : base(executorState, ftpMessage)
        {
            _commandArgsResolver = commandArgsResolver;
        }

        public override void Execute()
        {
            IConnection clientConnection = ExecutorState.ClientConnection;

            FtpMessage responce = new FtpMessage("331 Password required",
                clientConnection.Encoding);

            clientConnection.UserChanged = true;

            clientConnection.UserLogin = _commandArgsResolver.ResolveUserLogin(FtpMessage.Args);
            clientConnection.RemoteServerIdentifier = _commandArgsResolver.ResolveServerIdentifier(FtpMessage.Args);

            SendToClient(responce);
        }
    }
}