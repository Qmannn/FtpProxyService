using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FtpProxy.Connections;
using FtpProxy.Log;
using FtpProxy.Service.Handlers;

namespace FtpProxy
{
    public class FtpProxyWorker
    {
        private TcpListener _listener;
        private List<ClientHandler> _activeHandlers;

        private readonly IPEndPoint _localEndPoint;

        private bool _listening;

        public FtpProxyWorker( IPAddress ipAddress, int port )
        {
            _localEndPoint = new IPEndPoint( ipAddress, port );
        }

        public FtpProxyWorker()
            : this( IPAddress.Any, 21 )
        {
        }
        
        public void Start()
        {
            _listener = new TcpListener( _localEndPoint );

            Logger.Log.Info( "Started" );

            _listener.Start();
            _listening = true;
            _activeHandlers = new List<ClientHandler>();
            _listener.BeginAcceptTcpClient( HandleAcceptTcpClient, _listener );
        }

        public void Stop()
        {
            Logger.Log.Info( "Service stoped" );

            _listening = false;
            _listener.Stop();

            foreach( ClientHandler activeHandler in _activeHandlers )
            {
                activeHandler.CloseHandler();
            }
        }

        private void HandleAcceptTcpClient( IAsyncResult result )
        {
            if ( _listening )
            {
                _listener.BeginAcceptTcpClient( HandleAcceptTcpClient, _listener );
                TcpClient client = _listener.EndAcceptTcpClient( result );

                Logger.Log.Error( "Client was connected" );

                Connection connection = new Connection( client );
                ClientHandler clientHandler = new ClientHandler( connection );
                _activeHandlers.Add( clientHandler );

                ThreadPool.QueueUserWorkItem( clientHandler.HandleClient );
            }
        }
    }
}