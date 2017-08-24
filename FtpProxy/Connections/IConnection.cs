using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FtpProxy.Entity;

namespace FtpProxy.Connections
{
    public interface IConnection
    {
        Dictionary<ConnectionDataType, string> ConnectionData { get; }

        string UserLogin { get; set; }

        string RemoteServerIdentifier { get; set; }

        string Password { get; set; }

        bool UserChanged { get; set; }

        bool DataAvailable { get; }

        bool IsConnected { get; }

        Encoding Encoding { get; }

        void Connect();

        IAsyncResult BeginConnect( AsyncCallback callback, object state );

        void SendMessage( IFtpMessage message );

        IFtpMessage SendWithResponce(IFtpMessage ftpMessage);

        void SendCommand( string command );

        IFtpMessage GetMessage();

        void SetUpSecureConnectionAsClient();

        void SetUpSecureConnectionAsServer();

        void SetUpSecureConnectionAsServer(X509Certificate certificate);

        void CopyDataTo( Connection targetConnection );

        void CloseConnection();

        void Write( byte[] buffer, int offset, int count );

        /// <summary>
        /// True - шифрование канала разрешено, иначе - запрещено
        /// </summary>
        /// <param name="dataEncriptionStatus"></param>
        void SetDataEncryptionStatus(bool dataEncriptionStatus);
    }
}