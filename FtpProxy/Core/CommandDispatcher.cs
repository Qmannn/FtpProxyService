using System;
using FtpProxy.Core.Commands;
using FtpProxy.Entity;

namespace FtpProxy.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        public ICommand DispatchClientCommand(IFtpMessage ftpMessage, IExecutorState executorState)
        {
            switch (ftpMessage.CommandName)
            {
                case ProcessingClientCommand.Eprt:
                case ProcessingClientCommand.Port:
                    break;
                case ProcessingClientCommand.Pasv:
                case ProcessingClientCommand.Epsv:
                    break;
                case ProcessingClientCommand.Auth:
                    break;
                case ProcessingClientCommand.Pass:
                    break;
                case ProcessingClientCommand.Pbsz:
                    break;
                case ProcessingClientCommand.Quit:
                    break;
                case ProcessingClientCommand.User:
                    break;
            }
            return new OtherCommand(executorState, ftpMessage);
        }

        public ICommand DispatchServerCommand(IFtpMessage ftpMessage, IExecutorState executorState)
        {
            if (ftpMessage.CommandType == ServerCommandType.Unknown)
            {
                throw new ArgumentException("Invalid server ftp message", "ftpMessage");
            }

            return new OtherCommand(executorState, ftpMessage);
        }
    }
}