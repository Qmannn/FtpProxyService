using System;
using FtpProxy.Connections;
using FtpProxy.Entity;
using FtpProxy.Log;

namespace FtpProxy.Service.Handlers
{
    public class ClientHandler
    {
        private readonly Connection _clientConnection;
        private readonly CommandExecutor _commandExecutor;

        public bool IsWorking { get; private set; }

        public ClientHandler( object obj )
        {
            IsWorking = true;
            _clientConnection = obj as Connection;
            _commandExecutor = new CommandExecutor( _clientConnection );
        }

        public void HandleClient( object obj )
        {
            // Preparing client
            if ( _clientConnection == null )
            {
                throw new ArgumentException( "Клиент имеет неверный тип", "obj" );
            }
            _clientConnection.SendCommand( "220 Helo my dear client!" );
            _clientConnection.ConnectionClosed += _commandExecutor.Close;

            try
            {
                Command clientCommand;
                while ( ( clientCommand = _clientConnection.GetCommand() ) != null )
                {
                    Command serverCommand = _commandExecutor.Execute( clientCommand );

                    if ( serverCommand == null )
                    {
                        continue;
                    }

                    _clientConnection.SendCommand( serverCommand );
                    if ( clientCommand.CommandName == ProcessingClientCommand.Quit )
                    {
                        Logger.Log.Info( "Client disconnected" );
                        break;
                    }
                }
                _commandExecutor.Close();
            }
            catch ( Exception ex )
            {
                Logger.Log.Error( ex.Message, ex );
            }
            IsWorking = false;
        }

        public void CloseHandler()
        {
            if ( _commandExecutor != null )
            {
                _commandExecutor.Close();
            }
        }
    }
}