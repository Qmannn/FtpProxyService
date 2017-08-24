using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public class UserCommand : Command
    {
        public UserCommand(IExecutorState executorState, IFtpMessage ftpMessage) : base(executorState, ftpMessage)
        {
        }

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}