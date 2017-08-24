using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FtpProxy.Connections;
using FtpProxy.Core;
using FtpProxy.Log;
using FtpProxy.Service.Handlers;

namespace FtpProxy
{
    public class FtpProxyWorker
    {
        /// <summary>
        /// Слушатель адресов ipV6
        /// </summary>
        private TcpListener _ipV6Listener;
        /// <summary>
        /// Слушатель адресов ipV4
        /// </summary>
        private TcpListener _ipV4Listener;

        /// <summary>
        /// Активные обработчики клиентов ipV4
        /// </summary>
        private readonly List<ClientHandler> _activeIpv4Handlers;
        /// <summary>
        /// Активные обработчики клиентов ipV6
        /// </summary>
        private readonly List<ClientHandler> _activeIpv6Handlers;
        
        private readonly IPEndPoint _ipV6LocalEndPoint;
        private readonly IPEndPoint _ipV4LocalEndPoint;

        private bool _listening;

        public FtpProxyWorker( int port )
        {
            _ipV6LocalEndPoint = new IPEndPoint( IPAddress.IPv6Any, port );
            _ipV4LocalEndPoint = new IPEndPoint( IPAddress.Any, port );

            _activeIpv4Handlers = new List<ClientHandler>();
            _activeIpv6Handlers = new List<ClientHandler>();
        }

        public FtpProxyWorker()
            : this( 21 )
        {
        }
        
        public void Start()
        {
            _ipV6Listener = new TcpListener( _ipV6LocalEndPoint );
            _ipV4Listener = new TcpListener( _ipV4LocalEndPoint );

            Logger.Log.Info( "Started" );

            _ipV6Listener.Start();
            _ipV4Listener.Start();
            _listening = true;
            _ipV6Listener.BeginAcceptTcpClient( HandleAcceptIpv6TcpClient, _ipV6Listener );
            _ipV4Listener.BeginAcceptTcpClient( HandleAcceptIpv4TcpClient, _ipV4Listener );
        }

        public void Stop()
        {
            Logger.Log.Info( "Service stoped" );

            _listening = false;
            _ipV6Listener.Stop();
            _ipV4Listener.Stop();

            foreach ( ClientHandler activeHandler in _activeIpv4Handlers )
            {
                activeHandler.CloseHandler();
            }

            foreach ( ClientHandler activeHandler in _activeIpv6Handlers )
            {
                activeHandler.CloseHandler();
            }
        }

        private void HandleAcceptIpv6TcpClient( IAsyncResult result )
        {
            if ( _listening )
            {
                _ipV6Listener.BeginAcceptTcpClient( HandleAcceptIpv6TcpClient, _ipV6Listener );
                TcpClient client = _ipV6Listener.EndAcceptTcpClient( result );

                Logger.Log.Info( "Client was connected" );

                Connection connection = new Connection( client );
                Executor executor = new Executor(connection);

                ClientHandler clientHandler = new ClientHandler( connection );
                _activeIpv6Handlers.Add( clientHandler );

                ThreadPool.QueueUserWorkItem(executor.StartExecuting);
            }
        }

        private void HandleAcceptIpv4TcpClient( IAsyncResult result )
        {
            if ( _listening )
            {
                _ipV4Listener.BeginAcceptTcpClient( HandleAcceptIpv4TcpClient, _ipV4Listener );
                TcpClient client = _ipV4Listener.EndAcceptTcpClient( result );

                Logger.Log.Info( "Client was connected" );

                Connection connection = new Connection( client );
                Executor executor = new Executor(connection);
                ClientHandler clientHandler = new ClientHandler( connection );
                _activeIpv4Handlers.Add( clientHandler );

                ThreadPool.QueueUserWorkItem(executor.StartExecuting);
            }
        }
    }
}