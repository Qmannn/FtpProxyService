using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public class UnauthorizedCommand : Command
    {
        public UnauthorizedCommand(IExecutorState executorState, IFtpMessage ftpMessage) 
            : base(executorState, ftpMessage)
        {
        }

        public override void Execute()
        {
            IFtpMessage ftpMessage = new FtpMessage(ServerMessageCode.Unauthorized, "Please login with USER and PASS.",
                ExecutorState.ClientConnection.Encoding);
            ExecutorState.ClientConnection.SendMessage(ftpMessage);
        }
    }
}