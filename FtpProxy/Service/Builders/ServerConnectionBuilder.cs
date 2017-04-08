using System;
using System.Security.Authentication;
using FtpProxy.Connections;
using FtpProxy.Entity;

namespace FtpProxy.Service.Builders
{
    /// <summary>
    /// Строит соединение с удаленным сервером
    /// пошагово выполняя команды. Порядок выполннения команд задается
    /// порядком выполения методов Build*
    /// </summary>
    public class ServerConnectionBuilder
    {
        private readonly Connection _connection;

        public ServerConnectionBuilder( string url, int port, string user, string pass )
        {
            _connection = new Connection( url, port );
            _connection.ConnectionData[ ConnectionDataType.User ] = user;
            _connection.ConnectionData[ ConnectionDataType.Pass ] = pass;
        }

        public ServerConnectionBuilder BuildRemoteConnection()
        {
            _connection.Connect();

            Command recivedCommand = _connection.GetCommand();

            // Если сервер вернул не код подтверждения подключения
            if( recivedCommand.CommandName != "220" )
            {
                throw new AuthenticationException( "Remote server is not available" );
            }

            return this;
        }

        public ServerConnectionBuilder BuildConnectionSecurity()
        {
            _connection.SendCommand( "AUTH TLS" );

            Command recivedCommand = _connection.GetCommand();

            // Если сервер вернул не код подтверждения установки соединения - соединение не установлено
            if( recivedCommand.CommandName != "234" )
            {
                throw new AuthenticationException( "Server not support connection secutity" );
            }

            _connection.SetUpSecureConnectionAsClient();

            return this;
        }

        public ServerConnectionBuilder BuildUser()
        {
            _connection.SendCommand( new Command( String.Format( "USER {0}", _connection.ConnectionData[ ConnectionDataType.User ] ),
                    _connection.Encoding ) );
            Command recivedCommand = _connection.GetCommand();

            // Если сервер вернул не код ожидания ввода пароля - данные неверны
            if ( recivedCommand.CommandName != "331" )
            {
                throw new AuthenticationException("Incorrect user login for remote server");
            }

            return this;
        }

        public ServerConnectionBuilder BuildPass()
        {
            _connection.SendCommand( new Command( String.Format( "PASS {0}", _connection.ConnectionData[ ConnectionDataType.User ] ),
                    _connection.Encoding ) );
            Command recivedCommand = _connection.GetCommand();

            // Если сервер вернул не код подтверждения корректности пароля - данные неверны
            if( recivedCommand.CommandName != "230" )
            {
                throw new AuthenticationException( "Incorrect password for remote server" );
            }

            return this;
        }

        public Connection GetResult()
        {
            return _connection;
        }
    }
}