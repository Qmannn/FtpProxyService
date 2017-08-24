using System.Text;
using FtpProxy.Connections;
using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public class WelcomeCommand : Command
    {
        public WelcomeCommand(IExecutorState executorState) : base(executorState, null)
        {
        }

        public override void Execute()
        {
            IFtpMessage welcomeMessage = GetWelcomeFtpMessage();
            ExecutorState.ClientConnection.SendMessage(welcomeMessage);
        }

        private IFtpMessage GetWelcomeFtpMessage()
        {
            return new FtpMessage("220 Welcome!");
        }
    }
}