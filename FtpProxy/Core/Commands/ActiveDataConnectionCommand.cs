using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using FtpProxy.Connections;
using FtpProxy.Core.Factory;
using FtpProxy.Core.Helpers;
using FtpProxy.Entity;

namespace FtpProxy.Core.Commands
{
    public class ActiveDataConnectionCommand : Command
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ICommandExecutorHelper _commandExecutorHelper;

        private IDataConnection _dataConnection;
        private TcpListener _dataConnectionListener;

        public ActiveDataConnectionCommand(IExecutorState executorState, IFtpMessage ftpMessage, IConnectionFactory connectionFactory, ICommandExecutorHelper commandExecutorHelper) : base(executorState, ftpMessage)
        {
            _connectionFactory = connectionFactory;
            _commandExecutorHelper = commandExecutorHelper;
        }

        public override void Execute()
        {
            _dataConnection = _connectionFactory.CreateDataConnection(DataConnectionType.Active);

            _dataConnection.Connection = _commandExecutorHelper.GetActiveDataConnection(FtpMessage);

            _dataConnectionListener = new TcpListener(ServerConnection.LocalEndPoint.Address, 0);
            _dataConnectionListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)_dataConnectionListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short clientPort = (short)passiveListenerEndpoint.Port;

            byte[] clientPortArray = BitConverter.GetBytes(clientPort);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(clientPortArray);

            int ipver;
            switch (passiveListenerEndpoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    ipver = 1;
                    break;
                case AddressFamily.InterNetworkV6:
                    ipver = 2;
                    break;
                default:
                    throw new InvalidOperationException("The IP protocol being used is not supported.");
            }

            // созданы команды для двух типов подключения чтобы в случае когда сервер не поддерживает одну из них - использовать вторую
            FtpMessage portCommand = new FtpMessage(
                String.Format("PORT {0},{1},{2},{3},{4},{5}", address[0], address[1],
                    address[2], address[3], clientPortArray[0], clientPortArray[1]),
                ServerConnection.Encoding);

            FtpMessage eprtCommand = new FtpMessage(
                String.Format("EPRT |{0}|{1}|{2}|", ipver, passiveListenerEndpoint.Address,
                    passiveListenerEndpoint.Port), ServerConnection.Encoding);

            FtpMessage successfulResponce = new FtpMessage(
                String.Format("200 {0} command successful", FtpMessage.CommandName), ClientConnection.Encoding);

            List<IFtpMessage> messagesQueue = new List<IFtpMessage>();

            if (FtpMessage.CommandName == ProcessingClientCommand.Port)
            {
                messagesQueue.Add(portCommand);
                messagesQueue.Add(eprtCommand);
            }
            else
            {
                messagesQueue.Add(eprtCommand);
                messagesQueue.Add(portCommand);
            }

            foreach (IFtpMessage message in messagesQueue)
            {
                IFtpMessage serverResponse = SendWithResponceServer(message);

                if (serverResponse.CommandType != ServerCommandType.Error)
                {
                    SendToClient(successfulResponce);
                    return;
                }
            }

            _dataConnectionListener.Stop();
            _dataConnectionListener = null;
            SendToClient(new FtpMessage("451 can't open data connection", ClientConnection.Encoding));
        }

        public override IExecutorState GetExecutorState()
        {
            ExecutorState.DataConnectionListener = _dataConnectionListener;
            ExecutorState.ClientDataConnection = _dataConnection;
            ExecutorState.ServerDataConnection = _connectionFactory.CreateDataConnection(DataConnectionType.Active);
            return base.GetExecutorState();
        }
    }
}