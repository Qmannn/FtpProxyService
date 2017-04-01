using System;
using System.Net;
using FtpProxy.Connections;
using FtpProxy.Entity;
using FtpProxy.Log;

namespace FtpProxy.Service.Handlers
{
    public class ClientHandler
    {
        // соденинеия для передачи команд
        private Connection _clientConnection;

        public void HandleClient( object obj )
        {
            _clientConnection = obj as Connection;

            if ( _clientConnection == null )
            {
                throw new ArgumentException( "Клиент имеет неверный тип", "obj" );
            }
            
            _clientConnection.SendCommand( "220 Helo my dear client!" );
            
            CommandExecutor commandExecutor = new CommandExecutor( _clientConnection );

            try
            {
                Command clientCommand;
                while ( ( clientCommand = _clientConnection.GetCommand() ) != null )
                {
                    Command serverCommand = commandExecutor.Execute( clientCommand );

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
                commandExecutor.Close();
            }
            catch ( Exception ex )
            {
                Logger.Log.Error( ex.Message, ex );
            }
        }
    }
}