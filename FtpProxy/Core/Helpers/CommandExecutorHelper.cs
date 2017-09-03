using System;
using System.Net;
using System.Text.RegularExpressions;
using FtpProxy.Connections;
using FtpProxy.Entity;
using FtpProxy.Log;

namespace FtpProxy.Core.Helpers
{
    public class CommandExecutorHelper : ICommandExecutorHelper
    {
        #region Public

        /// <summary>
        /// Возвращает готовое к устанволению связи соединение для активного режима передачи.
        /// Тип активного режима PORT или EPRT выбирается по переданной команде клиента
        /// </summary>
        /// <param name="clientCommand"></param>
        /// <returns></returns>
        public Connection GetActiveDataConnection(IFtpMessage clientCommand)
        {
            switch (clientCommand.CommandName)
            {
                case ProcessingClientCommand.Port:
                    return GetPortDataConnetion(clientCommand);
                case ProcessingClientCommand.Eprt:
                    return GetEprtDataConnection(clientCommand);
            }

            throw new ArgumentException("Invalid connection type");
        }

        /// <summary>
        /// Соединение данных для пассивного режима работы (PASV)
        /// </summary>
        /// <param name="serverResponce">Ответ сервера на команда PASV</param>
        /// <returns>Соединение</returns>
        public Connection GetPasvDataConnection(IFtpMessage serverResponce)
        {
            Match pasvInfo = Regex.Match(serverResponce.Args,
                @"(?<quad1>\d+),(?<quad2>\d+),(?<quad3>\d+),(?<quad4>\d+),(?<port1>\d+),(?<port2>\d+)");

            if (!pasvInfo.Success || pasvInfo.Groups.Count != 7)
            {
                Logger.Log.Error(String.Format("Malformed PASV response: {0}", serverResponce));
                return null;
            }
            byte[] host =
            {
                Convert.ToByte( pasvInfo.Groups[ "quad1" ].Value ),
                Convert.ToByte( pasvInfo.Groups[ "quad2" ].Value ),
                Convert.ToByte( pasvInfo.Groups[ "quad3" ].Value ),
                Convert.ToByte( pasvInfo.Groups[ "quad4" ].Value )
            };

            int port = (Convert.ToInt32(pasvInfo.Groups["port1"].Value) << 8) +
                       Convert.ToInt32(pasvInfo.Groups["port2"].Value);

            return new Connection(new IPAddress(host), port);
        }

        /// <summary>
        /// Соединение данных для пассивного режима работы (EPSV)
        /// </summary>
        /// <param name="serverResponce">Ответ сервера на команду EPSV</param>
        /// <param name="serverIpAddress">IpAddress соединения с сервером</param>
        /// <returns></returns>
        public Connection GetEpsvDataConnection(IFtpMessage serverResponce, IPAddress serverIpAddress)
        {
            Match epsvInfo = Regex.Match(serverResponce.Args,
                @"\(\|\|\|(?<port>\d+)\|\)");

            if (!epsvInfo.Success)
            {
                Logger.Log.Error(String.Format("Malformed EPSV response: {0}", serverResponce));
                return null;
            }

            int port = Int32.Parse(epsvInfo.Groups["port"].Value);

            return new Connection(serverIpAddress, port);
        }

        #endregion

        #region Private

        private Connection GetPortDataConnetion(IFtpMessage clientCommand)
        {
            Match portInfo = Regex.Match(clientCommand.Args,
                @"(?<quad1>\d+),(?<quad2>\d+),(?<quad3>\d+),(?<quad4>\d+),(?<port1>\d+),(?<port2>\d+)");
            byte[] host =
            {
                Convert.ToByte( portInfo.Groups[ "quad1" ].Value ),
                Convert.ToByte( portInfo.Groups[ "quad2" ].Value ),
                Convert.ToByte( portInfo.Groups[ "quad3" ].Value ),
                Convert.ToByte( portInfo.Groups[ "quad4" ].Value )
            };

            int port = (Convert.ToInt32(portInfo.Groups["port1"].Value) << 8) +
                       Convert.ToInt32(portInfo.Groups["port2"].Value);

            return new Connection(new IPAddress(host), port);
        }

        private Connection GetEprtDataConnection(IFtpMessage clientCommand)
        {
            string hostPort = clientCommand.Args;

            char delimiter = hostPort[0];

            string[] rawSplit = hostPort.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            string ipAddress = rawSplit[1];
            string port = rawSplit[2];

            return new Connection(IPAddress.Parse(ipAddress), int.Parse(port));
        }

        #endregion
    }
}