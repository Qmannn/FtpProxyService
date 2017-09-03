using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public class PbszCommand : Command
    {
        public PbszCommand(IExecutorState executorState, IFtpMessage ftpMessage) : base(executorState, ftpMessage)
        {
        }

        public override void Execute()
        {
            if (FtpMessage.Args != "0")
            {
                SendToClient(new FtpMessage("501 Server cannot accept argument", ClientConnection.Encoding));
                return;
            }
            IFtpMessage serverResponce = SendWithResponceServer(FtpMessage);
            SendToClient(serverResponce);
        }
    }
}