using FtpProxy.Core.Commands;
using FtpProxy.Entity;

namespace FtpProxy.Core.Factory
{
    public class LocalCommandFactory : ICommandFactory
    {
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
                    break;
                case ProcessingClientCommand.Pass:
                    break;
                default:
                    return null;
            }
            return null;
        }
    }
}