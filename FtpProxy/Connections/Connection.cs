using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FixedSslLib;
using FtpProxy.Configuration;
using FtpProxy.Entity;
using FtpProxy.Log;

namespace FtpProxy.Connections
{
    public class Connection : IConnection
    {
        /// <summary>
        /// Размер буфера для чтения и записи команд
        /// </summary>
        private const int CommandBufferSize = 512;

        /// <summary>
        /// Размер буфера для чтения и записи данных
        /// </summary>
        private const int DataBufferSize = 4096;

        /// <summary>
        /// Флаг, указывающий на то, поддерживает ли соединение шифрование канала данных
        /// </summary>
        public bool DataEncryptionEnabled { get; set; }

        private TcpClient _controlClient;

        private FixedSslStream _sslStream;
        private X509Certificate _certificate;

        private NetworkStream _controlStream;

        private readonly IPAddress _ipAddress;
        private readonly int _port;

        private Dictionary<ConnectionDataType, string> _connectionData;
        /// <summary>
        /// Данные соединения типа логина, пароля и т.п.
        /// </summary>
        public Dictionary<ConnectionDataType, string> ConnectionData
        {
            get { return _connectionData ?? ( _connectionData = new Dictionary<ConnectionDataType, string>() ); }
        }

        /// <summary>
        /// Текущий активный стрим (SSl или Network)
        /// </summary>
        private Stream AciveStream
        {
            get { return (Stream) _sslStream ?? _controlStream; }
        }

        public DataConnectionType DataConnectionType { get; set; }

        public ConnectionType ConnectionType { get; private set; }

        public Encoding Encoding { get; set; }

        public IPEndPoint LocalEndPoint
        {
            get { return (IPEndPoint) _controlClient.Client.LocalEndPoint; }
        }

        public IPAddress IpAddress
        {
            get { return LocalEndPoint.Address; }
        }

        public bool DataAvailable
        {
            get { return _controlStream.DataAvailable; }
        }

        public bool IsConnected
        {
            get { return _controlClient != null && _controlClient.Connected; }
        }

        public User User { get; set; }

        public bool IsAuthorized { get; set; }

        /// <summary>
        /// Конструктор соединения на основе TcpClient.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="connectionType"></param>
        public Connection( TcpClient client, ConnectionType connectionType = ConnectionType.Client )
        {
            _controlClient = client;
            _controlStream = _controlClient.GetStream();
            Encoding = Encoding.UTF8;
            ConnectionType = connectionType;
        }

        public Connection( IPAddress ipAddress, int port, ConnectionType connectionType = ConnectionType.Server )
        {
            _ipAddress = ipAddress;
            _port = port;
            Encoding = Encoding.UTF8;
            ConnectionType = connectionType;
        }

        public void Connect()
        {
            _controlClient = new TcpClient();

            _controlClient.Connect( _ipAddress, _port );
            _controlStream = _controlClient.GetStream();

            UserChanged = true;
        }

        public IAsyncResult BeginConnect( AsyncCallback callback, object state )
        {
            _controlClient = new TcpClient();
            return _controlClient.BeginConnect( _ipAddress, _port, callback, state );
        }

        public void EndConnect( IAsyncResult asyncResult )
        {
            _controlClient.EndConnect( asyncResult );
        }

        public void SendCommand( Command command )
        {
            try
            {
                int position = 0;
                byte[] bytes = command.Bytes;
                while( position < bytes.Length )
                {
                    int length = bytes.Length + position > CommandBufferSize
                        ? CommandBufferSize
                        : bytes.Length;
                    AciveStream.Write( bytes, position, length );
                    position += length;
                }
            }
            catch ( Exception ex )
            {
                Logger.Log.Error( ex.Message, ex );
                throw;
            }
            
        }

        public void SendCommand( string stringCommand )
        {
            Command command = new Command( stringCommand, Encoding );
            SendCommand( command );
        }

        public Command GetCommand()
        {
            byte[] buffer = new byte[CommandBufferSize];
            try
            {
                using ( MemoryStream ms = new MemoryStream() )
                {
                    int count;
                    while ( ( count = AciveStream.Read( buffer, 0, buffer.Length ) ) > 0 )
                    {
                        ms.Write( buffer, 0, count );
                        if ( (char) buffer[ count - 1 ] == '\n'
                             && (char) buffer[ count - 2 ] == '\r' )
                        {
                            if ( ConnectionType == ConnectionType.Client )
                            {
                                break;
                            }
                            if ( Command.IsFullServerCommand( ms.ToArray(), Encoding ) )
                            {
                                break;
                            }
                        }
                    }

                    return new Command( ms.ToArray(), Encoding );
                }
            }
            catch ( Exception ex )
            {
                Logger.Log.Error( ex.Message, ex );
                throw;
            }
        }
        
        public void SetUpSecureConnectionAsServer()
        {
            _certificate = new X509Certificate2( Config.CertificatePath, Config.CertificatePassword );
            _sslStream = new FixedSslStream( _controlStream, true );
            _sslStream.AuthenticateAsServer( _certificate );
        }

        public void CopyDataTo( Connection targetConnection )
        {
            //AciveStream.CopyTo( targetConnection.AciveStream, 0 );
            byte[] buffer = new byte[ DataBufferSize ];
            int count;
            while( ( count = AciveStream.Read( buffer, 0, buffer.Length ) ) > 0 )
            {
                targetConnection.Write( buffer, 0, count );
            }
            targetConnection.AciveStream.Flush();
        }

        public void CloseConnection()
        {
            if( _sslStream != null )
            {
                _sslStream.Close();
            }
            if( _controlStream != null )
            {
                _controlStream.Close();
            }
            if ( _controlClient != null )
            {
                _controlClient.Close();
            }
            _sslStream = null;
            _controlStream = null;
            _certificate = null;
            _controlClient = null;
        }

        public void Write( byte[] buffer, int offset, int count )
        {
            AciveStream.Write( buffer, offset, count );
            AciveStream.Flush();
        }

        public void SetUpSecureConnectionAsClient()
        {
            _sslStream = new FixedSslStream( _controlStream, false, ( sender, certificate, chain, errors ) => true, null );
            _sslStream.AuthenticateAsClient( IpAddress.ToString() );
        }

        public bool UserChanged { get; set; }
    }
}