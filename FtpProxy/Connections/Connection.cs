﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Timers;
using FixedSslLib;
using FtpProxy.Configuration;
using FtpProxy.Entity;
using FtpProxy.Log;

namespace FtpProxy.Connections
{
    public class Connection : IConnection
    {
        /// <summary>
        /// Размер буфера для чтения и записи команд
        /// </summary>
        private const int CommandBufferSize = 1024;

        /// <summary>
        /// Флаг, указывающий на то, поддерживает ли соединение шифрование канала данных
        /// </summary>
        public bool DataEncryptionEnabled { get; set; }

        private TcpClient _controlClient;
        private readonly object _controlClientLocker = new object();

        private FixedSslStream _sslStream;

        private NetworkStream _controlStream;

        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly string _urlAddress;
        
        // TODO придумать что-то получше
        private readonly Queue<FtpMessage> _messageQueue = new Queue<FtpMessage>(); 

        private Dictionary<ConnectionDataType, string> _connectionData;
        /// <summary>
        /// Данные соединения типа логина, пароля и т.п.
        /// </summary>
        public Dictionary<ConnectionDataType, string> ConnectionData
        {
            get { return _connectionData ?? (_connectionData = new Dictionary<ConnectionDataType, string>()); }
        }

        public string UserLogin { get; set; }

        public object ConnectionOperationLocker
        {
            get { return _connectionOperationLocker; }
        }

        public string RemoteServerIdentifier { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Текущий активный стрим (SSl или Network)
        /// </summary>
        public Stream AciveStream
        {
            get { return (Stream)_sslStream ?? _controlStream; }
        }

        private readonly object _activeStreamLocker = new object();

        private readonly object _connectionOperationLocker = new object();

        public DataConnectionType DataConnectionType { get; set; }

        public ConnectionType ConnectionType { get; private set; }

        public Encoding Encoding { get; set; }

        public IPEndPoint LocalEndPoint
        {
            get
            {
                lock (_controlClientLocker)
                {
                    if (_controlClient == null)
                    {
                        return null;
                    }
                    return (IPEndPoint)_controlClient.Client.LocalEndPoint;
                }
            }
        }

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                lock (_controlClientLocker)
                {
                    if (_controlClient == null)
                    {
                        return null;
                    }
                    return (IPEndPoint)_controlClient.Client.RemoteEndPoint;
                }
            }
        }

        public IPAddress IpAddress
        {
            get { return LocalEndPoint.Address; }
        }

        public IPAddress RemoteIpAddress
        {
            get { return RemoteEndPoint.Address; }
        }

        public bool DataAvailable
        {
            get
            {
                lock (_controlClientLocker)
                {
                    return _controlClient != null && _controlClient.Client.Poll( 1000, SelectMode.SelectRead );
                }
            }
        }

        public bool IsConnected
        {
            get
            {
                lock (_controlClientLocker)
                {
                    if (_controlClient == null)
                    {
                        return false;
                    }
                    if (!_controlClient.Connected)
                    {
                        return false;
                    }
                    try
                    {

                        bool part1 = _controlClient.Client.Poll(1000, SelectMode.SelectRead);
                        bool part2 = _controlClient.Client.Available == 0;
                        if (part1 & part2)
                        {
                            return false;
                        }
                        return true;
                    }
                    catch (SocketException e)
                    {
                        Logger.Log.Error("Control client not connected", e);
                        return false;
                    }
                }
            }
        }

        #region Events

        private readonly Timer _timer;
        private const int Interval = 1000;

        private readonly List<ConectionEventHandler> _onCloseHandlers = new List<ConectionEventHandler>();
        public event ConectionEventHandler ConnectionClosed
        {
            add { _onCloseHandlers.Add(value); }
            remove { _onCloseHandlers.Remove(value); }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (!IsConnected)
            {
                foreach (var socketEventHandler in _onCloseHandlers)
                {
                    socketEventHandler.Invoke();
                }
                EventsEnabled = false;
            }
        }

        public bool EventsEnabled
        {
            set
            {
                if (value)
                    _timer.Start();
                else
                    _timer.Stop();
            }
        }

        public delegate void ConectionEventHandler();

        #endregion

        /// <summary>
        /// Конструктор соединения на основе TcpClient.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="connectionType"></param>
        public Connection(TcpClient client, ConnectionType connectionType = ConnectionType.Client) : this()
        {
            _controlClient = client;
            _controlStream = _controlClient.GetStream();
            Encoding = Encoding.Default;
            ConnectionType = connectionType;
        }

        public Connection(IPAddress ipAddress, int port, ConnectionType connectionType = ConnectionType.Server) : this()
        {
            _ipAddress = ipAddress;
            _port = port;
            Encoding = Encoding.Default;
            ConnectionType = connectionType;
        }

        public Connection(string urlAddress, int port, ConnectionType connectionType = ConnectionType.Server) : this()
        {
            _urlAddress = urlAddress;
            _port = port;
            Encoding = Encoding.Default;
            ConnectionType = connectionType;
        }

        public Connection()
        {
            _timer = new Timer { Interval = Interval };
            _timer.Elapsed += TimerTick;
            _timer.Start();
        }

        public void Connect()
        {
            lock (_controlClientLocker)
            {
                if (_ipAddress != null)
                {
                    _controlClient = new TcpClient(_ipAddress.AddressFamily);
                    _controlClient.Connect(_ipAddress, _port);
                }
                else if (!String.IsNullOrEmpty(_urlAddress))
                {
                    foreach (IPAddress ipAddress in Dns.GetHostAddresses(_urlAddress))
                    {
                        _controlClient = new TcpClient(ipAddress.AddressFamily);
                        try
                        {
                            _controlClient.Connect(ipAddress, _port);
                        }
                        catch (SocketException ex)
                        {
                            Logger.Log.Error(
                                String.Format("Не удалось разрешить имя удаленного сервера (DNS) {0}", _urlAddress), ex);
                        }
                        if (IsConnected)
                        {
                            break;
                        }
                    }
                }

                if (!IsConnected)
                {
                    throw new Exception("Не удалось подключиться к удаленному серверу");
                }
                _controlStream = _controlClient.GetStream();
                UserChanged = true;
            }
        }

        public IAsyncResult BeginConnect(AsyncCallback callback, object state)
        {
            _controlClient = new TcpClient();
            return _controlClient.BeginConnect(_ipAddress, _port, callback, state);
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            _controlClient.EndConnect(asyncResult);
        }

        public void SendMessage(IFtpMessage command)
        {
            try
            {
                int position = 0;
                byte[] bytes = command.Bytes;
                while (position < bytes.Length)
                {
                    int length = Math.Min(bytes.Length - position, CommandBufferSize);
                    AciveStream.Write(bytes, position, length);
                    position += length;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw;
            }

        }

        public IFtpMessage SendWithResponce(IFtpMessage ftpMessage)
        {
            SendMessage(ftpMessage);
            return GetMessage();
        }

        public void SendCommand(string stringCommand)
        {
            FtpMessage command = new FtpMessage(stringCommand, Encoding);
            SendMessage(command);
        }

        public IFtpMessage GetMessage()
        {
            if (_messageQueue.Count > 0)
            {
                return _messageQueue.Dequeue();
            }

            if (!IsConnected)
            {
                return null;
            }

            byte[] buffer = new byte[CommandBufferSize];
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    lock (_activeStreamLocker)
                    {
                        int count;
                        while ((count = AciveStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, count);
                            if ((char)buffer[count - 1] == '\n'
                                 && (char)buffer[count - 2] == '\r')
                            {
                                if (ConnectionType == ConnectionType.Client || FtpMessage.IsFullServerCommand(ms.ToArray(), Encoding))
                                {
                                    foreach (FtpMessage ftpMessage in FtpMessage.GetMessages(ms.ToArray(), Encoding, ConnectionType))
                                    {
                                        _messageQueue.Enqueue(ftpMessage);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if (_messageQueue.Count > 0)
                    {
                        return _messageQueue.Dequeue();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
            return null;
        }

        public void SetUpSecureConnectionAsServer()
        {
            X509Certificate certificate = String.IsNullOrEmpty(Config.CertificatePassword)
                ? new X509Certificate(Config.CertificatePath)
                : new X509Certificate2(Config.CertificatePath, Config.CertificatePassword);
            SetUpSecureConnectionAsServer(certificate);
        }

        public void SetUpSecureConnectionAsServer(X509Certificate certificate)
        {
            lock (_controlClientLocker)
            {
                lock (_activeStreamLocker)
                {
                    _sslStream = new FixedSslStream(_controlStream, true);
                    _sslStream.AuthenticateAsServer(certificate);
                }
            }
        }

        public void CopyDataTo(IConnection targetConnection)
        {
            lock (_activeStreamLocker)
            {
                AciveStream.CopyTo(targetConnection.AciveStream);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (_sslStream != null)
                {
                    _sslStream.Close();
                }
                if (_controlStream != null)
                {
                    _controlStream.Close();
                }
            }
            catch (Exception)
            {
                // ignored
            }
            _sslStream = null;
            _controlStream = null;

            lock (_controlClientLocker)
            {
                if (_controlClient != null)
                {
                    _controlClient.Close();
                }
                _controlClient = null;
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            lock (_activeStreamLocker)
            {
                if (AciveStream != null)
                {
                    AciveStream.Write(buffer, offset, count);
                    AciveStream.Flush();
                }
            }
        }

        public void SetDataEncryptionStatus(bool dataEncriptionStatus)
        {
            DataEncryptionEnabled = dataEncriptionStatus;
        }

        public void SetUpSecureConnectionAsClient()
        {
            _sslStream = new FixedSslStream(_controlStream, false, (sender, certificate, chain, errors) => true, null);
            _sslStream.AuthenticateAsClient(IpAddress.ToString());
        }

        public bool UserChanged { get; set; }
    }
}