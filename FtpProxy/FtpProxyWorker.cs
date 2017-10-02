using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FtpProxy.Connections;
using FtpProxy.Core;
using FtpProxy.Log;

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
        private readonly List<Executor> _activeIpv4Executors;
        /// <summary>
        /// Активные обработчики клиентов ipV6
        /// </summary>
        private readonly List<Executor> _activeIpv6Executors;

        private readonly IPEndPoint _ipV6LocalEndPoint;
        private readonly IPEndPoint _ipV4LocalEndPoint;

        private bool _listening;

        public FtpProxyWorker(int port)
        {
            _ipV6LocalEndPoint = new IPEndPoint(IPAddress.IPv6Any, port);
            _ipV4LocalEndPoint = new IPEndPoint(IPAddress.Any, port);

            _activeIpv4Executors = new List<Executor>();
            _activeIpv6Executors = new List<Executor>();
        }

        public FtpProxyWorker()
            : this(21)
        {
        }

        public void Start()
        {
            _ipV6Listener = new TcpListener(_ipV6LocalEndPoint);
            _ipV4Listener = new TcpListener(_ipV4LocalEndPoint);

            Logger.Log.Info("Started");

            _ipV6Listener.Start();
            _ipV4Listener.Start();
            _listening = true;
            _ipV6Listener.BeginAcceptTcpClient(HandleAcceptIpv6TcpClient, _ipV6Listener);
            _ipV4Listener.BeginAcceptTcpClient(HandleAcceptIpv4TcpClient, _ipV4Listener);
        }

        public void Stop()
        {
            Logger.Log.Info("Service stoped");

            _listening = false;
            _ipV6Listener.Stop();
            _ipV4Listener.Stop();

            foreach (Executor executor in _activeIpv4Executors)
            {
                executor.StopExecuting();
            }

            foreach (Executor executor in _activeIpv6Executors)
            {
                executor.StopExecuting();
            }
        }

        private void HandleAcceptIpv6TcpClient(IAsyncResult result)
        {
            if (!_listening)
            {
                return;
            }

            _ipV6Listener.BeginAcceptTcpClient(HandleAcceptIpv6TcpClient, _ipV6Listener);
            TcpClient client = _ipV6Listener.EndAcceptTcpClient(result);
            Connection connection = new Connection(client);
            Logger.Log.Info(String.Format("Client connected {0}", connection.IpAddress));
            Executor executor = new Executor(connection);
            _activeIpv6Executors.Add(executor);

            ThreadPool.QueueUserWorkItem(executor.StartExecuting);
        }

        private void HandleAcceptIpv4TcpClient(IAsyncResult result)
        {
            if (!_listening)
            {
                return;
            }

            _ipV4Listener.BeginAcceptTcpClient(HandleAcceptIpv4TcpClient, _ipV4Listener);
            TcpClient client = _ipV4Listener.EndAcceptTcpClient(result);
            Connection connection = new Connection(client);
            Logger.Log.Info(String.Format("Client connected {0}", connection.IpAddress));
            Executor executor = new Executor(connection);
            _activeIpv4Executors.Add(executor);

            ThreadPool.QueueUserWorkItem(executor.StartExecuting);
        }
    }
}