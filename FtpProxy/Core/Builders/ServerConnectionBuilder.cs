using System;
using System.Security.Authentication;
using FtpProxy.Connections;
using FtpProxy.Core.Factory;
using FtpProxy.Entity;

namespace FtpProxy.Core.Builders
{
    /// <summary>
    /// Строит соединение с удаленным сервером
    /// пошагово выполняя команды. Порядок выполннения команд задается
    /// порядком выполения методов Build*
    /// </summary>
    public class ServerConnectionBuilder : IServerConnectionBuilder
    {
        private readonly IConnectionFactory _connectionFactory;

        private IConnection _connection;

        public ServerConnectionBuilder(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IServerConnectionBuilder BuildRemoteConnection(string url, int port)
        {
            _connection = _connectionFactory.CreateConnection(url, port);
            _connection.Connect();

            IFtpMessage recivedCommand = _connection.GetMessage();

            // Если сервер вернул не код подтверждения подключения
            if (recivedCommand.CommandName != "220")
            {
                throw new AuthenticationException("Remote server is not available");
            }

            return this;
        }

        public IServerConnectionBuilder BuildConnectionSecurity()
        {
            _connection.SendCommand("AUTH TLS");

            IFtpMessage recivedCommand = _connection.GetMessage();

            // Если сервер вернул не код подтверждения установки соединения - соединение не установлено
            if (recivedCommand.CommandName != "234")
            {
                throw new AuthenticationException("Server not support connection secutity");
            }

            _connection.SetUpSecureConnectionAsClient();

            return this;
        }

        public IServerConnectionBuilder BuildUser(string user)
        {
            _connection.SendMessage(new FtpMessage(String.Format("USER {0}", user),
                _connection.Encoding));
            IFtpMessage recivedCommand = _connection.GetMessage();

            // Если сервер вернул не код ожидания ввода пароля - данные неверны
            if (recivedCommand.CommandName != "331")
            {
                throw new AuthenticationException("Incorrect user login for remote server");
            }

            return this;
        }

        public IServerConnectionBuilder BuildPass(string pass)
        {
            _connection.SendMessage(new FtpMessage(String.Format("PASS {0}", pass),
                    _connection.Encoding));
            IFtpMessage recivedCommand = _connection.GetMessage();

            // Если сервер вернул не код подтверждения корректности пароля - данные неверны
            if (recivedCommand.CommandName != "230")
            {
                throw new AuthenticationException("Incorrect password for remote server");
            }

            return this;
        }

        public IConnection GetResult()
        {
            return _connection;
        }
    }
}