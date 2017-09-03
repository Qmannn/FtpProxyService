using System;
using System.Threading;
using FtpProxy.Connections;
using FtpProxy.Entity;
using FtpProxy.Log;

namespace FtpProxy.Core.DataConnection
{
    public class DataOperationExecutor : IDataOperationExecutor
    {
        private IConnection _clientDataConnection;
        private IConnection _serverDataConnection;

        public void DoDataConnectionOperation(IAsyncResult result)
        {
            DataConnectionExecutorState state = result.AsyncState as DataConnectionExecutorState;
            if (state == null)
            {
                return;
            }

            _clientDataConnection = state.ClientDataConnection.Connection;
            _serverDataConnection = state.ServerDataConnection.Connection;

            lock (state.ClientConnection.ConnectionOperationLocker)
            {
                try
                {
                    PrepareDataConnections(result, state);
                }
                catch (Exception ex)
                {
                    Logger.Log.Error("Error starting data connection operation", ex);
                    return;
                }

                try
                {
                    // Бесконечный цикл, работает до появления в одном из каналов данных для считывания
                    // таким образом проверяется направление передачи
                    while (true)
                    {
                        if (_serverDataConnection.DataAvailable)
                        {
                            _serverDataConnection.CopyDataTo(_clientDataConnection);
                            break;
                        }
                        if (_clientDataConnection.DataAvailable)
                        {
                            _clientDataConnection.CopyDataTo(_serverDataConnection);
                            break;
                        }
                        Thread.Sleep(150);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log.Error("Data connection closed when copy operation was running", ex);
                }

                _serverDataConnection.CloseConnection();
                IFtpMessage command = state.ServerConnection.GetMessage();
                if (command != null)
                {
                    state.ClientConnection.SendMessage(command);
                }
                _clientDataConnection.CloseConnection();
            }
        }

        private void PrepareDataConnections(IAsyncResult result, DataConnectionExecutorState state)
        {
            if (state.ClientDataConnection.DataConnectionType == DataConnectionType.Active)
            {
                _serverDataConnection = new Connection(state.DataConnetionListener.EndAcceptTcpClient(result));
                if (state.ServerConnection.DataEncryptionEnabled)
                {
                    _serverDataConnection.SetUpSecureConnectionAsClient();
                }
                _clientDataConnection.Connect();
                if (state.ClientConnection.DataEncryptionEnabled)
                {
                    _clientDataConnection.SetUpSecureConnectionAsServer();
                }

            }
            else if (state.ClientDataConnection.DataConnectionType == DataConnectionType.Passive)
            {
                _clientDataConnection = new Connection(state.DataConnetionListener.EndAcceptTcpClient(result));
                if (state.ClientConnection.DataEncryptionEnabled)
                {
                    _clientDataConnection.SetUpSecureConnectionAsServer();
                }
                _serverDataConnection.Connect();
                if (state.ServerConnection.DataEncryptionEnabled)
                {
                    _serverDataConnection.SetUpSecureConnectionAsClient();
                }
            }
            else
            {
                Logger.Log.Error(String.Format("Not implemented data connection type {0} ",
                    state.ClientDataConnection.DataConnectionType));
            }
        }
    }
}