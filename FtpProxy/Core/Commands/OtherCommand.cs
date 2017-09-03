using System;
using System.Net.Sockets;
using FtpProxy.Core.DataConnection;
using FtpProxy.Entity;
using FtpProxy.Log;

namespace FtpProxy.Core.Commands
{
    public class OtherCommand : Command
    {
        private readonly IDataOperationExecutor _dataOperationExecutor;

        public OtherCommand(IExecutorState executorState, IFtpMessage clientFtpMessage, IDataOperationExecutor dataOperationExecutor)
            : base(executorState, clientFtpMessage)
        {
            _dataOperationExecutor = dataOperationExecutor;
        }

        public override void Execute()
        {
            IFtpMessage serverResponce = SendWithResponceServer(FtpMessage);
            if (serverResponce == null)
            {
                return;
            }
            switch (serverResponce.CommandType)
            {
                case ServerCommandType.Waiting:
                    if (ExecutorState.ClientDataConnection != null)
                    {
                        StartDataConnectionOperation();
                    }
                    else
                    {
                        SendToClient(serverResponce);
                        serverResponce = GetServerResponce();
                    }
                    break;
            }
            SendToClient(serverResponce);
        }

        private void StartDataConnectionOperation()
        {
            TcpListener dataConnectionListener = ExecutorState.DataConnectionListener;
            if (dataConnectionListener == null)
            {
                Logger.Log.Error("_dataConnectionListener not initializing");
                throw new MemberAccessException("_dataConnectionListener is null");
            }

            DataConnectionExecutorState dataConnectionExecutorState = new DataConnectionExecutorState
            {
                ClientConnection = ExecutorState.ClientConnection,
                ServerConnection = ExecutorState.ServerConnection,
                ClientDataConnection = ExecutorState.ClientDataConnection,
                ServerDataConnection = ExecutorState.ServerDataConnection,
                DataConnetionListener = dataConnectionListener
            };

            ExecutorState.ClientDataConnection = null;
            ExecutorState.ServerDataConnection = null;
            ExecutorState.DataConnectionListener = null;
            
            dataConnectionListener.BeginAcceptTcpClient(_dataOperationExecutor.DoDataConnectionOperation, dataConnectionExecutorState);
        }
    }
}