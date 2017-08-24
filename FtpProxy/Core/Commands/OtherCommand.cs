using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public class OtherCommand : Command
    {
        public OtherCommand(IExecutorState executorState, IFtpMessage clientFtpMessage)
            : base(executorState, clientFtpMessage)
        {
        }

        public override void Execute()
        {
            ExecutorState.ServerConnection.SendMessage(FtpMessage);
            IFtpMessage serverResponece = ExecutorState.ServerConnection.GetMessage();
            ExecutorState.ClientConnection.SendMessage(serverResponece);
        }
    }
}