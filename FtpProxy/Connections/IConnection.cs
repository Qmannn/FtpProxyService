using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FtpProxy.Entity;

namespace FtpProxy.Connections
{
    public interface IConnection
    {
        Dictionary<ConnectionDataType, string> ConnectionData { get; }

        string UserLogin { get; set; }

        event Connection.ConectionEventHandler ConnectionClosed;

        Stream AciveStream { get; }

        bool DataEncryptionEnabled { get; }

        object ConnectionOperationLocker { get; }

        string RemoteServerIdentifier { get; set; }

        string Password { get; set; }

        bool UserChanged { get; set; }

        bool DataAvailable { get; }

        IPAddress RemoteIpAddress { get; }

        IPAddress IpAddress { get; }

        bool IsConnected { get; }

        Encoding Encoding { get; }

        void Connect();

        IAsyncResult BeginConnect( AsyncCallback callback, object state );

        void SendMessage( IFtpMessage message );

        IFtpMessage SendWithResponce(IFtpMessage ftpMessage);

        IPEndPoint LocalEndPoint { get; }

        void SendCommand( string command );

        IFtpMessage GetMessage();

        void SetUpSecureConnectionAsClient();

        void SetUpSecureConnectionAsServer();

        void SetUpSecureConnectionAsServer(X509Certificate certificate);

        void CopyDataTo( IConnection targetConnection );

        void CloseConnection();

        void Write( byte[] buffer, int offset, int count );

        /// <summary>
        /// True - шифрование канала разрешено, иначе - запрещено
        /// </summary>
        /// <param name="dataEncriptionStatus"></param>
        void SetDataEncryptionStatus(bool dataEncriptionStatus);
    }
}