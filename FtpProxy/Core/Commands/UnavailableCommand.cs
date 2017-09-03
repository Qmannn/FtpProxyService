using System;
using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public class UnavailableCommand : Command
    {
        public UnavailableCommand(IExecutorState executorState, IFtpMessage ftpMessage) : base(executorState, ftpMessage)
        {
        }

        public override void Execute()
        {
            string message = String.Format("{0} unavailable", FtpMessage.CommandName);
            SendToClient(new FtpMessage(ServerMessageCode.Unavailable, message));
        }
    }
}