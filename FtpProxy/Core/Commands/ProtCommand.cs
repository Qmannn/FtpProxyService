using FtpProxy.Entity;
using Microsoft.Practices.Unity.Configuration;

namespace FtpProxy.Core.Commands
{
    public class ProtCommand : Command
    {
        public ProtCommand(IExecutorState executorState, IFtpMessage ftpMessage) 
            : base(executorState, ftpMessage)
        {
        }

        public override void Execute()
        {
            IFtpMessage serverResponce = ExecutorState.ServerConnection.SendWithResponce(FtpMessage);
            IFtpMessage commandResult = null;
            if (FtpMessage.Args.StartsWith("P"))
            {
                ExecutorState.ClientConnection.SetDataEncryptionStatus(true);
                if (serverResponce.CommandType == ServerCommandType.Success)
                {
                    ExecutorState.ServerConnection.SetDataEncryptionStatus(true);
                }
                commandResult = new FtpMessage(ServerMessageCode.Success, "Data protection enabled",
                    ExecutorState.ClientConnection.Encoding);
            }
            if (FtpMessage.Args.StartsWith("C"))
            {
                ExecutorState.ClientConnection.SetDataEncryptionStatus(false);
                ExecutorState.ServerConnection.SetDataEncryptionStatus(false);
                commandResult = new FtpMessage(ServerMessageCode.Success, "Data protection not enabled",
                    ExecutorState.ClientConnection.Encoding);
            }
            commandResult = commandResult ??
                            new FtpMessage(ServerMessageCode.UnknownCommand, "Unknown command",
                                ExecutorState.ClientConnection.Encoding);
            ExecutorState.ClientConnection.SendMessage(commandResult);
        }
    }
}