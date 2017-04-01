using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FtpProxy.Entity;

namespace FtpProxy.Connections
{
    public interface IConnection
    {
        Dictionary<ConnectionDataType, string> ConnectionData { get; }

        bool UserChanged { get; set; }

        bool DataAvailable { get; }

        bool IsConnected { get; }

        Encoding Encoding { get; set; }

        void Connect();

        IAsyncResult BeginConnect( AsyncCallback callback, object state );

        void SendCommand( Command command );

        void SendCommand( string command );

        Command GetCommand();

        void SetUpSecureConnectionAsClient();

        void SetUpSecureConnectionAsServer();

        void CopyDataTo( Connection targetConnection );

        void CloseConnection();

        void Write( byte[] buffer, int offset, int count );
    }
}