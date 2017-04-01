using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FtpProxy.Connections;
using FtpProxy.Log;
using FtpProxy.Service.Handlers;
using log4net;

namespace FtpProxy
{
    public class FtpProxyWorker
    {
        private TcpListener _listener;
        private List<Connection> _activeConnections;

        private readonly IPEndPoint _localEndPoint;

        private bool _listening = false;

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
            _activeConnections = new List<Connection>();
            _listener.BeginAcceptTcpClient( HandleAcceptTcpClient, _listener );
        }

        public void Stop()
        {
            Logger.Log.Info( "Service stoped" );

            _listening = false;
            _listener.Stop();

            foreach ( Connection activeConnection in _activeConnections )
            {
                activeConnection.CloseConnection();
            }
        }

        private void HandleAcceptTcpClient( IAsyncResult result )
        {
            if ( _listening )
            {
                _listener.BeginAcceptTcpClient( HandleAcceptTcpClient, _listener );
                TcpClient client = _listener.EndAcceptTcpClient( result );

                Logger.Log.Info( "Client was connected" );

                Connection connection = new Connection( client );
                _activeConnections.Add( connection );

                ClientHandler clientHandler = new ClientHandler();

                ThreadPool.QueueUserWorkItem( clientHandler.HandleClient, connection );
            }
        }
    }
}