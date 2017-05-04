using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UsersLib.Checkers;
using UsersLib.Checkers.Results;
using UsersLib.Factories;
using FtpProxy.Connections;
using FtpProxy.Entity;
using FtpProxy.Log;
using FtpProxy.Service.Builders;
using FtpProxy.Service.Resolvers;

namespace FtpProxy.Service
{
    public class CommandExecutor
    {
        // соденинеия для передачи команд
        private readonly Connection _clientConnection;
        private Connection _serverConnection;

        // соединения данных
        private Connection _clientDataConnection;
        private Connection _serverDataConnection;

        /// <summary>
        /// Тип соединения данных
        /// </summary>
        private DataConnectionType _dataConnectionType = DataConnectionType.None;

        /// <summary>
        /// Слушатель для соединения данных
        /// </summary>
        private TcpListener _dataConnectionListener;

        private ICheckersFactory _checkersFactory;

        private readonly CommandArgsResolver _argsResolver = new CommandArgsResolver();

        private ICheckersFactory CheckersFactory
        {
            get { return _checkersFactory ?? ( _checkersFactory = new CheckersFactory() ); }
        }

        public CommandExecutor( Connection clientConnection )
        {
            _clientConnection = clientConnection;
            _serverConnection = null;
        }

        public Command Execute( Command clientCommand )
        {
            try
            {
                if( _serverConnection != null && _serverConnection.IsConnected )
                {
                    return ExecuteWithRemouteServer( clientCommand );
                }
            }
            catch ( IOException e )
            {
                _serverConnection = null;
                Logger.Log.Info( "Remote server disconnected", e );
                return new Command( "421 ssss", Encoding.UTF8 );
            }
            
            return ExecuteAsServer( clientCommand );
        }

        private Command ExecuteWithRemouteServer( Command clientCommand )
        {
            switch( clientCommand.CommandName )
            {
                case ProcessingClientCommand.Port:
                    clientCommand = Port( clientCommand );
                    break;
                case ProcessingClientCommand.Eprt:
                    clientCommand = Eprt( clientCommand );
                    break;
            }

            _serverConnection.SendCommand( clientCommand );
            var serverCommand = _serverConnection.GetCommand();

            switch( serverCommand.CommandType )
            {
                case ServerCommandType.Waiting:
                    if( _dataConnectionType != DataConnectionType.None )
                    {
                        StartDataConnectionOperation();
                    }
                    else
                    {
                        _clientConnection.SendCommand( serverCommand );
                        serverCommand = _serverConnection.GetCommand();
                    }
                    break;
            }

            switch( clientCommand.CommandName )
            {
                case ProcessingClientCommand.Auth:
                    Command clientResponce = Auth( clientCommand );
                    _clientConnection.SendCommand( clientResponce );
                    _clientConnection.SetUpSecureConnectionAsServer();
                    if( serverCommand.CommandType != ServerCommandType.Error )
                    {
                        _serverConnection.SetUpSecureConnectionAsClient();
                    }
                    serverCommand = null;
                    break;
                case ProcessingClientCommand.Pasv:
                    serverCommand = Pasv( serverCommand );
                    break;
                case ProcessingClientCommand.Epsv:
                    break;
                case ProcessingClientCommand.Prot:
                    serverCommand = Prot( clientCommand, serverCommand );
                    break;
                case ProcessingClientCommand.Pbsz:
                    serverCommand = Pbsz( clientCommand );
                    break;
            }
            return serverCommand;
        }

        private Command ExecuteAsServer( Command clientCommand )
        {
            Command asServerCommand;
            try
            {
                switch ( clientCommand.CommandName )
                {
                    case ProcessingClientCommand.Auth:
                        asServerCommand = Auth( clientCommand );
                        _clientConnection.SendCommand( asServerCommand );
                        _clientConnection.SetUpSecureConnectionAsServer();
                        asServerCommand = null;
                        break;
                    case ProcessingClientCommand.User:
                        asServerCommand = User( clientCommand );
                        break;
                    case ProcessingClientCommand.Pass:
                        asServerCommand = Pass( clientCommand );
                        break;
                    default:
                        asServerCommand = new Command( "530 Please login with USER and PASS.",
                            _clientConnection.Encoding );
                        break;
                }
            }
            catch ( ArgumentException e )
            {
                Logger.Log.Error( String.Format( "Invalid USER command: {0}", clientCommand.Args ), e );
                asServerCommand = new Command( "504 invalid command", _clientConnection.Encoding );
            }
            return asServerCommand;
        }

        public void Close()
        {
            if( _clientConnection != null && _clientConnection.IsConnected )
            {
                _clientConnection.CloseConnection();
            }
            if( _serverConnection != null && _serverConnection.IsConnected )
            {
                _serverConnection.CloseConnection();
            }
        }
        
        #region FTP Commands

        private Command User( Command clientCommand )
        {
            Command responce = new Command( "331 Password required",
                _clientConnection.Encoding );

            _clientConnection.UserChanged = true;

            _clientConnection.ConnectionData[ ConnectionDataType.User ] =
                _argsResolver.ResolveUserLogin( clientCommand.Args );
            _clientConnection.ConnectionData[ ConnectionDataType.RemoteSiteIdentifier ] =
                _argsResolver.ResolveServerIdentifier( clientCommand.Args );

            return responce;
        }

        private Command Pass( Command clientCommand )
        {
            Command responce = new Command( "230 user connected. Welcome", _clientConnection.Encoding );

            _clientConnection.ConnectionData[ ConnectionDataType.Pass ] =
                _argsResolver.ResolvePassword( clientCommand.Args );

            if ( !_clientConnection.ConnectionData.ContainsKey( ConnectionDataType.User )
                 || !_clientConnection.ConnectionData.ContainsKey( ConnectionDataType.Pass )
                 || !_clientConnection.ConnectionData.ContainsKey( ConnectionDataType.RemoteSiteIdentifier ) )
            {
                return new Command( "503 incorrect command sequence", _clientConnection.Encoding );
            }

            IUserChecker userChecker = CheckersFactory.CreateUserChecker();
            IUserCheckerResult checkerResult = userChecker.Check( _clientConnection.ConnectionData[ ConnectionDataType.User ],
                _clientConnection.ConnectionData[ ConnectionDataType.Pass ],
                _clientConnection.ConnectionData[ ConnectionDataType.RemoteSiteIdentifier ] );

            if ( checkerResult == null )
            {
                return new Command( "530 Login incorrect", _clientConnection.Encoding );
            }

            ServerConnectionBuilder connectionBuilder = new ServerConnectionBuilder(
                checkerResult.UrlAddress, 
                checkerResult.Port, 
                checkerResult.Login,
                checkerResult.Pass );

            try
            {
                connectionBuilder.BuildRemoteConnection();
            }
            catch ( Exception e )
            {
                Logger.Log.Error( String.Format( "BuildRemoteConnection: {0}", e.Message ), e );
                return new Command( "434 Remote host not available", _clientConnection.Encoding );
            }

            try
            {
                connectionBuilder.BuildConnectionSecurity();
            }
            catch( Exception e )
            {
                Logger.Log.Error( String.Format( "BuildConnectionSecurity: {0}", e.Message ), e );
            }

            try
            {
                connectionBuilder.BuildUser();
                connectionBuilder.BuildPass();
            }
            catch( Exception e )
            {
                Logger.Log.Error( String.Format( "BuildUserPass: {0}", e.Message ), e );
                return new Command( "425 remote server data is incorrect", _clientConnection.Encoding );
            }

            _serverConnection = connectionBuilder.GetResult();
            _serverConnection.ConnectionClosed += Close;

            return responce;
        }

        private Command Auth( Command clientCommand )
        {
            if ( !clientCommand.Args.StartsWith( "TLS" ) )
            {
                return new Command( "504 only TLS supported", _clientConnection.Encoding );
            }
            return new Command( "234 Enabling TLS Connection", _clientConnection.Encoding );
        }

        private Command Prot( Command clientCommand, Command serverResponce )
        {
            if ( clientCommand.Args.StartsWith( "P" ) )
            {
                _clientConnection.DataEncryptionEnabled = true;
                if ( serverResponce.CommandType == ServerCommandType.Success )
                {
                    _serverConnection.DataEncryptionEnabled = true;
                }
                return new Command( "200 success, data connection was protected", _serverConnection.Encoding );
            }
            if ( clientCommand.Args.StartsWith( "C" ) )
            {
                _clientConnection.DataEncryptionEnabled = false;
                _serverConnection.DataEncryptionEnabled = false;
                if ( serverResponce.CommandType == ServerCommandType.Success )
                {
                    return new Command( "200 success, data connection was not protected", _serverConnection.Encoding );
                }
            }
            return new Command( "501 command not understood", _serverConnection.Encoding );
        }

        private Command Pasv( Command serverCommand )
        {
            Match pasvInfo = Regex.Match( serverCommand.Args,
                @"(?<quad1>\d+),(?<quad2>\d+),(?<quad3>\d+),(?<quad4>\d+),(?<port1>\d+),(?<port2>\d+)" );

            if ( !pasvInfo.Success || pasvInfo.Groups.Count != 7 )
            {
                Logger.Log.Error( String.Format( "Malformed PASV response: {0}", serverCommand ) );
                _serverConnection.SendCommand( "ABOR can't parse pasv address" );
                _serverConnection.GetCommand();
                return new Command( "451 can't open passive mode", _clientConnection.Encoding );
            }
            byte[] host =
            {
                Convert.ToByte( pasvInfo.Groups[ "quad1" ].Value ),
                Convert.ToByte( pasvInfo.Groups[ "quad2" ].Value ),
                Convert.ToByte( pasvInfo.Groups[ "quad3" ].Value ),
                Convert.ToByte( pasvInfo.Groups[ "quad4" ].Value )
            };

            int port = ( Convert.ToInt32( pasvInfo.Groups[ "port1" ].Value ) << 8 ) +
                       Convert.ToInt32( pasvInfo.Groups[ "port2" ].Value );

            _serverDataConnection = new Connection( new IPAddress( host ), port );

            _dataConnectionListener = new TcpListener( _clientConnection.LocalEndPoint.Address, 0 );
            _dataConnectionListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint) _dataConnectionListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short clientPort = (short) passiveListenerEndpoint.Port;

            byte[] clientPortArray = BitConverter.GetBytes( clientPort );
            if ( BitConverter.IsLittleEndian )
                Array.Reverse( clientPortArray );

            _dataConnectionType = DataConnectionType.Passive;

            return
                new Command(
                    String.Format( "227 Entering!! Passive Mode ({0},{1},{2},{3},{4},{5})", address[ 0 ], address[ 1 ],
                        address[ 2 ], address[ 3 ], clientPortArray[ 0 ], clientPortArray[ 1 ] ),
                    _clientConnection.Encoding );
        }

        private Command Port( Command clientCommand )
        {
            Match portInfo = Regex.Match( clientCommand.Args,
                @"(?<quad1>\d+),(?<quad2>\d+),(?<quad3>\d+),(?<quad4>\d+),(?<port1>\d+),(?<port2>\d+)" );
            byte[] host =
            {
                Convert.ToByte( portInfo.Groups[ "quad1" ].Value ),
                Convert.ToByte( portInfo.Groups[ "quad2" ].Value ),
                Convert.ToByte( portInfo.Groups[ "quad3" ].Value ),
                Convert.ToByte( portInfo.Groups[ "quad4" ].Value )
            };

            int port = ( Convert.ToInt32( portInfo.Groups[ "port1" ].Value ) << 8 ) +
                       Convert.ToInt32( portInfo.Groups[ "port2" ].Value );

            _clientDataConnection = new Connection( new IPAddress( host ), port );

            _dataConnectionListener = new TcpListener( _serverConnection.LocalEndPoint.Address, 0 );
            _dataConnectionListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint) _dataConnectionListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short clientPort = (short) passiveListenerEndpoint.Port;

            byte[] clientPortArray = BitConverter.GetBytes( clientPort );
            if ( BitConverter.IsLittleEndian )
                Array.Reverse( clientPortArray );

            _dataConnectionType = DataConnectionType.Active;

            return new Command(
                String.Format( "PORT {0},{1},{2},{3},{4},{5}", address[ 0 ], address[ 1 ],
                    address[ 2 ], address[ 3 ], clientPortArray[ 0 ], clientPortArray[ 1 ] ),
                _clientConnection.Encoding );
        }

        private Command Eprt( Command clientCommand )
        {
            _dataConnectionType = DataConnectionType.Active;

            string hostPort = clientCommand.Args;

            _dataConnectionType = DataConnectionType.Active;

            char delimiter = hostPort[ 0 ];

            string[] rawSplit = hostPort.Split( new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries );

            string ipAddress = rawSplit[ 1 ];
            string port = rawSplit[ 2 ];

            _clientDataConnection = new Connection( IPAddress.Parse( ipAddress ), int.Parse( port ) );

            _dataConnectionListener = new TcpListener( _serverConnection.LocalEndPoint.Address, 0 );
            _dataConnectionListener.Start();
            IPEndPoint passiveListenerEndpoint = (IPEndPoint) _dataConnectionListener.LocalEndpoint;
            int ipver;
            switch ( passiveListenerEndpoint.AddressFamily )
            {
                case AddressFamily.InterNetwork:
                    ipver = 1;
                    break;
                case AddressFamily.InterNetworkV6:
                    ipver = 2;
                    break;
                default:
                    throw new InvalidOperationException( "The IP protocol being used is not supported." );
            }

            return new Command(
                String.Format( "EPRT |{0}|{1}|{2}|", ipver, passiveListenerEndpoint.Address,
                    passiveListenerEndpoint.Port ), _clientConnection.Encoding );
        }

        private Command Pbsz( Command clientCommand )
        {
            if ( clientCommand.Args != "0" )
            {
                return new Command( "501 Server cannot accept argument", _clientConnection.Encoding );
            }

            return new Command( "200 PBSZ command successful", _clientConnection.Encoding );
        }

        #endregion FTPCommands

        #region DataConnectionOperations

        private void DoDataConnectionOperation( IAsyncResult result )
        {
            CheckDataConnectionsAccess();

            try
            {
                PrepareDataConnections( result );
            }
            catch ( Exception ex )
            {
                Logger.Log.Error( "Error starting data connection operation", ex );
                return;
            }

            try
            {
                while ( true )
                {
                    if ( _serverDataConnection.DataAvailable )
                    {
                        _serverDataConnection.CopyDataTo( _clientDataConnection );
                        break;
                    }
                    if ( _clientDataConnection.DataAvailable )
                    {
                        _clientDataConnection.CopyDataTo( _serverDataConnection );
                        break;
                    }
                    Thread.Sleep( 150 );
                }
            }
            catch ( Exception ex )
            {
                Logger.Log.Error( "Data connection closed when copy operation was running", ex );
            }

            _serverDataConnection.CloseConnection();
            Command command = _serverConnection.GetCommand();
            if ( command != null )
            {
                _clientConnection.SendCommand( command );
            }
            _clientDataConnection.CloseConnection();

            _dataConnectionType = DataConnectionType.None;
            _serverDataConnection = null;
            _clientDataConnection = null;
        }

        private void PrepareDataConnections( IAsyncResult result )
        {
            CheckDataConnectionsAccess();

            if ( _dataConnectionType == DataConnectionType.Active )
            {
                _serverDataConnection = new Connection( _dataConnectionListener.EndAcceptTcpClient( result ) );
                if ( _serverConnection.DataEncryptionEnabled )
                {
                    _serverDataConnection.SetUpSecureConnectionAsClient();
                }
                _clientDataConnection.Connect();
                if ( _clientConnection.DataEncryptionEnabled )
                {
                    _clientDataConnection.SetUpSecureConnectionAsServer();
                }

            }
            else if ( _dataConnectionType == DataConnectionType.Passive )
            {
                _clientDataConnection = new Connection( _dataConnectionListener.EndAcceptTcpClient( result ) );
                if ( _clientConnection.DataEncryptionEnabled )
                {
                    _clientDataConnection.SetUpSecureConnectionAsServer();
                }
                _serverDataConnection.Connect();
                if ( _serverConnection.DataEncryptionEnabled )
                {
                    _serverDataConnection.SetUpSecureConnectionAsClient();
                }
            }
            else
            {
                Logger.Log.Error( String.Format( "Not implemented data connection type {0} ", _dataConnectionType ) );
            }
        }

        private void StartDataConnectionOperation()
        {
            if ( _dataConnectionListener == null )
            {
                Logger.Log.Error( "_dataConnectionListener not initializing" );
                throw new MemberAccessException( "_dataConnectionListener is null" );
            }

            _dataConnectionListener.BeginAcceptTcpClient( DoDataConnectionOperation, null );
        }

        /// <summary>
        /// Проверяет возмоность выполнения операции соединения данных
        /// </summary>
        private void CheckDataConnectionsAccess()
        {
            if( _serverDataConnection == null && _dataConnectionType == DataConnectionType.Passive )
            {
                Logger.Log.Error( "Data connection with remote server was not established (_serverDataConnection)" );
                throw new MemberAccessException( "Access to data connection obj before initializing" );
            }

            if( _clientDataConnection == null && _dataConnectionType == DataConnectionType.Active )
            {
                Logger.Log.Error( "Data connection with remote server was not established (_clientDataConnection)" );
                throw new MemberAccessException( "Access to data connection obj before initializing" );
            }

            //if ( !_serverDataConnection.IsConnected )
            //{
            //    Logger.Log.Error( "Data connection with remote server was not established (not connected) (_serverDataConnection)" );
            //    throw new MemberAccessException( "Access to data connection obj before setup connection" );
            //}

            //if ( _dataConnectionType == DataConnectionType.Active
            //     && _clientDataConnection != null
            //     && !_clientDataConnection.IsConnected )
            //{
            //    Logger.Log.Error(
            //        "Data connection with remote server was not established  (not connected) (_clientDataConnection)" );
            //    throw new MemberAccessException( "Access to data connection obj before setup connection" );
            //}
        }

        #endregion
    }
}