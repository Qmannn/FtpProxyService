using System;
using System.Security.Cryptography.X509Certificates;
using FtpProxy.Configuration;
using FtpProxy.Connections;
using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public class AuthCommand : Command
    {
        public AuthCommand(IExecutorState executorState, IFtpMessage ftpMessage) 
            : base(executorState, ftpMessage)
        {
        }

        public override void Execute()
        {
            IFtpMessage ftpMessage;
            IConnection clientConnection = ExecutorState.ClientConnection;
            bool needProtectConnection = true;
            if (!FtpMessage.Args.StartsWith("TLS"))
            {
                ftpMessage = new FtpMessage("504 поддерживается только TLS протокол", clientConnection.Encoding);
                needProtectConnection = false;
            }
            else
            {
                ftpMessage = new FtpMessage("234 открытие TLS соединения", clientConnection.Encoding);
            }
            clientConnection.SendMessage(ftpMessage);
            if (needProtectConnection)
            {
                clientConnection.SetUpSecureConnectionAsServer(GetClientCertificate());
            }
        }

        private X509Certificate GetClientCertificate()
        {
            return String.IsNullOrEmpty(Config.CertificatePassword)
                ? new X509Certificate(Config.CertificatePath)
                : new X509Certificate2(Config.CertificatePath, Config.CertificatePassword);
        }
    }
}