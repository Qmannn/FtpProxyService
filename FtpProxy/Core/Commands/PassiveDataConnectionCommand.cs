using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using FtpProxy.Connections;
using FtpProxy.Core.Factory;
using FtpProxy.Core.Helpers;
using FtpProxy.Entity;
using FtpProxy.Log;

namespace FtpProxy.Core.Commands
{
    public class PassiveDataConnectionCommand : Command
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ICommandExecutorHelper _commandExecutorHelper;
        private TcpListener _dataConnectionListener;

        private IDataConnection _dataConnection;

        public PassiveDataConnectionCommand(IExecutorState executorState, IFtpMessage ftpMessage, IConnectionFactory connectionFactory, ICommandExecutorHelper commandExecutorHelper) : base(executorState, ftpMessage)
        {
            _connectionFactory = connectionFactory;
            _commandExecutorHelper = commandExecutorHelper;
        }

        public override void Execute()
        {
            _dataConnection = _connectionFactory.CreateDataConnection(DataConnectionType.Passive);

            List<FtpMessage> commandQueue = new List<FtpMessage>();

            if (FtpMessage.CommandName == ProcessingClientCommand.Pasv)
            {
                commandQueue.Add(new FtpMessage(ProcessingClientCommand.Pasv, ServerConnection.Encoding));
                commandQueue.Add(new FtpMessage(ProcessingClientCommand.Epsv, ServerConnection.Encoding));
            }
            else
            {
                commandQueue.Add(new FtpMessage(ProcessingClientCommand.Epsv, ServerConnection.Encoding));
                commandQueue.Add(new FtpMessage(ProcessingClientCommand.Pasv, ServerConnection.Encoding));
            }

            string dataConnectionVersion = String.Empty;

            IFtpMessage serverResponce = null;
            foreach (FtpMessage message in commandQueue)
            {
                serverResponce = SendWithResponceServer(message);
                if (serverResponce.CommandType == ServerCommandType.Success)
                {
                    dataConnectionVersion = message.CommandName;
                    break;
                }
            }

            // Если не удалось установить соединение с сервером ни по одному из возможных типов соединения
            // обычному или расширенному - говорим клиенту, что соединение открыть с этим режимом нельзя
            if (String.IsNullOrEmpty(dataConnectionVersion))
            {
                SendToClient(new FtpMessage("451 can't open data connection", ClientConnection.Encoding));
                return;
            }

            _dataConnection.Connection = dataConnectionVersion == ProcessingClientCommand.Pasv
                ? _commandExecutorHelper.GetPasvDataConnection(serverResponce)
                : _commandExecutorHelper.GetEpsvDataConnection(serverResponce, ServerConnection.RemoteIpAddress);

            if (_dataConnection.Connection == null)
            {
                Logger.Log.Error(String.Format("Malformed PASV response: {0}:{1}", serverResponce,
                    ClientConnection.IpAddress));

                // Останавливаем на сервере ожидание подключения, т.к. не удалось получить адрес
                ServerConnection.SendCommand("ABOR can't parse pasv address");
                ServerConnection.GetMessage();

                SendToClient(new FtpMessage("451 can't open passive mode", ClientConnection.Encoding));
                return;
            }

            //// Открываем соединение данных с сервером
            //_dataConnection.Connection.Connect();
            //if (ServerConnection.DataEncryptionEnabled)
            //{
            //    _dataConnection.Connection.SetUpSecureConnectionAsClient();
            //}

            _dataConnectionListener = new TcpListener(ClientConnection.IpAddress, 0);
            _dataConnectionListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)_dataConnectionListener.LocalEndpoint;

            // Для расширенного пассивного режима соедиенние и данные подготовлены, можно вернуть команду
            // Клиенту отдаем команду по тому типу режима, который он запросил, независимо
            // от того, какой установлен с сервером, иначе соединение будет оборвано клиентом
            if (FtpMessage.CommandName == ProcessingClientCommand.Epsv)
            {
                SendToClient(new FtpMessage(
                    String.Format("229 Entering Extended Passive Mode (|||{0}|)", passiveListenerEndpoint.Port),
                    ClientConnection.Encoding));
                return;
            }

            // далее подготавливаются данные для ответа по обычному пассивному режиму.
            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short clientPort = (short)passiveListenerEndpoint.Port;

            byte[] clientPortArray = BitConverter.GetBytes(clientPort);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(clientPortArray);

            SendToClient(new FtpMessage(
                String.Format("227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})", address[0], address[1],
                    address[2], address[3], clientPortArray[0], clientPortArray[1]),
                ClientConnection.Encoding));
        }

        public override IExecutorState GetExecutorState()
        {
            ExecutorState.DataConnectionListener = _dataConnectionListener;
            ExecutorState.ClientDataConnection = _connectionFactory.CreateDataConnection(DataConnectionType.Passive);
            ExecutorState.ServerDataConnection = _dataConnection;
            return base.GetExecutorState();
        }
    }
}