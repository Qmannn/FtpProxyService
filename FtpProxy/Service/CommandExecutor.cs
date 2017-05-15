using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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
        #region Members and properties

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

        // Фабрика оздания различных чекеров для проверки прав пользователя
        private ICheckersFactory _checkersFactory;

        private readonly CommandArgsResolver _argsResolver = new CommandArgsResolver();
        private readonly CommandExecutorHelper _commandExecutorHelper;

        private ICheckersFactory CheckersFactory
        {
            get { return _checkersFactory ?? ( _checkersFactory = new CheckersFactory() ); }
        } 

        #endregion

        public CommandExecutor( Connection clientConnection )
        {
            _clientConnection = clientConnection;
            _serverConnection = null;
            _commandExecutorHelper = new CommandExecutorHelper();
        }

        #region CommandExecuting

        /// <summary>
        /// Выполняет команду клиента
        /// </summary>
        /// <param name="clientCommand">Команда к выполнению</param>
        /// <returns>Ответ, сформированный локально или пришедший от удаленного сервера</returns>
        public Command Execute( Command clientCommand )
        {
            try
            {
                if ( _serverConnection != null && _serverConnection.IsConnected )
                {
                    return ExecuteWithRemouteServer( clientCommand );
                }
            }
            catch ( Exception e )
            {
                _serverConnection = null;
                Logger.Log.Info( "Erorr executing with remote server", e );
                return new Command( "451 Локальная ошибка, операция прервана", _clientConnection.Encoding );
            }

            return ExecuteAsServer( clientCommand );
        }

        /// <summary>
        /// Выполнение команды на удаленном сервере
        /// Использовать только после успешного соединения с удаленным сервером
        /// </summary>
        /// <param name="clientCommand"></param>
        /// <returns></returns>
        private Command ExecuteWithRemouteServer( Command clientCommand )
        {
            // команды, требующие особой обработки (открытие режимов передачи данных)
            switch ( clientCommand.CommandName )
            {
                case ProcessingClientCommand.Port:
                case ProcessingClientCommand.Eprt:
                    return ActiveDataConnection( clientCommand );
                case ProcessingClientCommand.Pasv:
                case ProcessingClientCommand.Epsv:
                    return PassiveDataConnection( clientCommand );
            }

            // отправка команд на удаленный сервер
            _serverConnection.SendCommand( clientCommand );
            var serverCommand = _serverConnection.GetCommand();

            // обработка ответа сервера
            switch ( serverCommand.CommandType )
            {
                case ServerCommandType.Waiting:
                    if ( _dataConnectionType != DataConnectionType.None )
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

            switch ( clientCommand.CommandName )
            {
                case ProcessingClientCommand.Auth:
                    Command clientResponce = Auth( clientCommand );
                    _clientConnection.SendCommand( clientResponce );
                    _clientConnection.SetUpSecureConnectionAsServer();
                    if ( serverCommand.CommandType != ServerCommandType.Error )
                    {
                        _serverConnection.SetUpSecureConnectionAsClient();
                    }
                    serverCommand = null;
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

        /// <summary>
        /// Выполняет команду относительно своего функционала - 
        /// установить защиту канала и авторизовать пользователя
        /// </summary>
        /// <param name="clientCommand">Команда к выполнению</param>
        /// <returns>Команда для отправки клиенту</returns>
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
            catch ( ArgumentException )
            {
                Logger.Log.Error( String.Format( "Invalid USER command: {0}", clientCommand.Args ) );
                asServerCommand = new Command( "504 invalid command", _clientConnection.Encoding );
            }
            return asServerCommand;
        }

        #endregion

        /// <summary>
        /// Безопасно закрывает все активные соединения
        /// </summary>
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
            Command responce = new Command( "230 успешная авторизация", _clientConnection.Encoding );

            _clientConnection.ConnectionData[ ConnectionDataType.Pass ] =
                _argsResolver.ResolvePassword( clientCommand.Args );

            if ( !_clientConnection.ConnectionData.ContainsKey( ConnectionDataType.User )
                 || !_clientConnection.ConnectionData.ContainsKey( ConnectionDataType.Pass )
                 || !_clientConnection.ConnectionData.ContainsKey( ConnectionDataType.RemoteSiteIdentifier ) )
            {
                return new Command( "503 неверная последовательность команд", _clientConnection.Encoding );
            }

            IUserChecker userChecker = CheckersFactory.CreateUserChecker();
            IUserCheckerResult checkerResult = userChecker.Check( _clientConnection.ConnectionData[ ConnectionDataType.User ],
                _clientConnection.ConnectionData[ ConnectionDataType.Pass ],
                _clientConnection.ConnectionData[ ConnectionDataType.RemoteSiteIdentifier ] );

            if ( checkerResult == null )
            {
                return new Command( "530 Неверная комбинация логин-пароль", _clientConnection.Encoding );
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
                Logger.Log.Error( String.Format( "BuildRemoteConnection: {0}", e.Message ) );
                return new Command( "434 удаленный сервер недоступен", _clientConnection.Encoding );
            }

            try
            {
                connectionBuilder.BuildConnectionSecurity();
            }
            catch( Exception e )
            {
                Logger.Log.Error( String.Format( "BuildConnectionSecurity: {0}", e.Message ) );
            }

            try
            {
                connectionBuilder.BuildUser();
                connectionBuilder.BuildPass();
            }
            catch( Exception e )
            {
                Logger.Log.Error( String.Format( "BuildUserPass: {0}", e.Message ) );
                return new Command( "425 некорректные данные для подключения к удаленному серверу. Обратитесь к администратру", _clientConnection.Encoding );
            }

            _serverConnection = connectionBuilder.GetResult();
            _serverConnection.ConnectionClosed += Close;

            return responce;
        }

        private Command Auth( Command clientCommand )
        {
            if ( !clientCommand.Args.StartsWith( "TLS" ) )
            {
                return new Command( "504 поддерживается только TLS протокол", _clientConnection.Encoding );
            }
            return new Command( "234 открытие TLS соединения", _clientConnection.Encoding );
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
                return new Command( "200 канал данных успешно защищен", _serverConnection.Encoding );
            }
            if ( clientCommand.Args.StartsWith( "C" ) )
            {
                _clientConnection.DataEncryptionEnabled = false;
                _serverConnection.DataEncryptionEnabled = false;
                if ( serverResponce.CommandType == ServerCommandType.Success )
                {
                    return new Command( "200 защита канала данных не активна", _serverConnection.Encoding );
                }
            }
            return new Command( "501 команда не распознана", _serverConnection.Encoding );
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

        #region DstsConnectionPreparing

        private Command PassiveDataConnection( Command clientCommand )
        {
            _dataConnectionType = DataConnectionType.Passive;

            List<Command> commandQueue = new List<Command>();

            if ( clientCommand.CommandName == ProcessingClientCommand.Pasv )
            {
                commandQueue.Add( new Command( ProcessingClientCommand.Pasv, _serverConnection.Encoding ) );
                commandQueue.Add( new Command( ProcessingClientCommand.Epsv, _serverConnection.Encoding ) );
            }
            else
            {
                commandQueue.Add( new Command( ProcessingClientCommand.Epsv, _serverConnection.Encoding ) );
                commandQueue.Add( new Command( ProcessingClientCommand.Pasv, _serverConnection.Encoding ) );
            }

            string dataConnectionVersion = String.Empty;

            Command serverResponce = null;
            foreach ( Command command in commandQueue )
            {
                _serverConnection.SendCommand( command );
                serverResponce = _serverConnection.GetCommand();
                if ( serverResponce.CommandType == ServerCommandType.Success )
                {
                    dataConnectionVersion = command.CommandName;
                    break;
                }
            }

            // Если не удалось установить соединение с сервером ни по одному из возможных типов соединения
            // обычному или расширенному - говорим клиенту, что соединение открыть с этим режимом нельзя
            if ( String.IsNullOrEmpty( dataConnectionVersion ) )
            {
                return new Command( "451 can't open data connection", _clientConnection.Encoding );
            }

            _serverDataConnection = dataConnectionVersion == ProcessingClientCommand.Pasv
                ? _commandExecutorHelper.GetPasvDataConnection( serverResponce )
                : _commandExecutorHelper.GetEpsvDataConnection( serverResponce, _serverConnection.IpAddress );

            if ( _serverDataConnection == null )
            {
                Logger.Log.Error( String.Format( "Malformed PASV response: {0}:{1}", serverResponce,
                    _clientConnection.IpAddress ) );

                // Останавливаем на сервере ожидание подключения, т.к. не удалось получить адрес
                _serverConnection.SendCommand( "ABOR can't parse pasv address" );
                _serverConnection.GetCommand();

                return new Command( "451 can't open passive mode", _clientConnection.Encoding );
            }

            _dataConnectionListener = new TcpListener( _clientConnection.IpAddress, 0 );
            _dataConnectionListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint) _dataConnectionListener.LocalEndpoint;

            // Для расширенного пассивного режима соедиенние и данные подготовлены, можно вернуть команду
            // Клиенту отдаем команду по тому типу режима, который он запросил, независимо
            // от того, какой установлен с сервером, иначе соединение будет оборвано клиентом
            if ( clientCommand.CommandName == ProcessingClientCommand.Epsv )
            {
                return new Command(
                    String.Format( "229 Entering Extended Passive Mode (|||{0}|)", passiveListenerEndpoint.Port ),
                    _clientConnection.Encoding );
            }

            // далее подготавливаются данные для ответа по обычному пассивному режиму.
            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short clientPort = (short) passiveListenerEndpoint.Port;

            byte[] clientPortArray = BitConverter.GetBytes( clientPort );
            if ( BitConverter.IsLittleEndian )
                Array.Reverse( clientPortArray );

            return new Command(
                String.Format( "227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})", address[ 0 ], address[ 1 ],
                    address[ 2 ], address[ 3 ], clientPortArray[ 0 ], clientPortArray[ 1 ] ),
                _clientConnection.Encoding );
        }

        private Command ActiveDataConnection( Command clientCommand )
        {
            _dataConnectionType = DataConnectionType.Active;

            _clientDataConnection = _commandExecutorHelper.GetActiveDataConnection( clientCommand );

            _dataConnectionListener = new TcpListener( _serverConnection.LocalEndPoint.Address, 0 );
            _dataConnectionListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint) _dataConnectionListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short clientPort = (short) passiveListenerEndpoint.Port;

            byte[] clientPortArray = BitConverter.GetBytes( clientPort );
            if ( BitConverter.IsLittleEndian )
                Array.Reverse( clientPortArray );

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

            // созданы команды для двух типов подключения чтобы в случае когда сервер не поддерживает одну из них - использовать вторую
            Command portCommand = new Command(
                String.Format( "PORT {0},{1},{2},{3},{4},{5}", address[ 0 ], address[ 1 ],
                    address[ 2 ], address[ 3 ], clientPortArray[ 0 ], clientPortArray[ 1 ] ),
                _serverConnection.Encoding );

            Command eprtCommand = new Command(
                String.Format( "EPRT |{0}|{1}|{2}|", ipver, passiveListenerEndpoint.Address,
                    passiveListenerEndpoint.Port ), _serverConnection.Encoding );

            Command successfulResponce = new Command(
                String.Format( "200 {0} command successful", clientCommand.CommandName ), _clientConnection.Encoding );

            List<Command> commandsQueue = new List<Command>();

            if ( clientCommand.CommandName == ProcessingClientCommand.Port )
            {
                commandsQueue.Add( portCommand );
                commandsQueue.Add( eprtCommand );
            }
            else
            {
                commandsQueue.Add( eprtCommand );
                commandsQueue.Add( portCommand );
            }

            foreach ( Command command in commandsQueue )
            {
                _serverConnection.SendCommand( command );
                Command serverResponse = _serverConnection.GetCommand();

                if ( serverResponse.CommandType != ServerCommandType.Error )
                {
                    return successfulResponce;
                }
            }

            return new Command( "451 can't open data connection", _clientConnection.Encoding );
        } 

        #endregion

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