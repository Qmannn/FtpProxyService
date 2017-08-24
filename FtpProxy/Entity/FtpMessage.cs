using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FtpProxy.Entity
{
    public class FtpMessage : IFtpMessage
    {
        private static readonly Regex CommandFinder = new Regex( @"(\r\n|^)(\d\d\d)\s" );

        private readonly byte[] _bytes;

        public byte[] Bytes
        {
            get { return _bytes; }
        }

        public ServerCommandType CommandType
        {
            get
            {
                int commandCode;
                if ( !Int32.TryParse( CommandName, out commandCode ) )
                {
                    return ServerCommandType.Unknown;
                }
                if ( commandCode < 200 )
                {
                    return ServerCommandType.Waiting;
                }
                if ( commandCode < 300 )
                {
                    return ServerCommandType.Success;
                }
                if ( commandCode < 400 )
                {
                    return ServerCommandType.WaitingForClient;
                }
                return ServerCommandType.Error;
            }
        }

        private readonly Encoding _encoding;

        private Encoding Encoding
        {
            get { return _encoding; }
        }

        public FtpMessage( byte[] bytes, Encoding encoding )
        {
            _bytes = bytes;
            _encoding = encoding;
            Init();
        }

        public FtpMessage( string command, Encoding encoding = null )
        {
            _encoding = encoding ?? Encoding.UTF8;
            _bytes = _encoding.GetBytes( String.Format( "{0}\r\n", command ) );
            Init();
        }

        public FtpMessage(ServerMessageCode messageCode, string messageText, Encoding encoding = null)
            : this(String.Format("{0} {1}", (int) messageCode, messageText), encoding)
        {
        }

        private void Init()
        {
            string command = Encoding.GetString( _bytes );
            string[] commandParts = command.Split( ' ' );

            Match commandName = CommandFinder.Match( command );
            if( String.IsNullOrEmpty( commandName.Value ) || commandName.Groups.Count != 3 )
            {
                CommandName = commandParts.First().Trim( '\n', '\r' );
            }
            else
            {
                CommandName = commandName.Groups[ 2 ].Value;
            }
            CommandName = CommandName.ToUpper();
            Args = String.Empty;
            if ( commandParts.Length > 1 )
            {
                Args = String.Join( " ", commandParts.Skip( 1 ) );
            }
            Args = Args.Trim( '\n', '\r' );
        }

        public string CommandName { get; private set; }

        public string Args { get; private set; }

        public override string ToString()
        {
            return String.Format( "{0} {1}", CommandName, Args );
        }

        #region Static members

        /// <summary>
        /// Проверяет, является ли команда полным ответом от СЕРВЕРА (НЕ КЛИЕНТА)
        /// </summary>
        /// <param name="commandBytes"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool IsFullServerCommand( byte[] commandBytes, Encoding encoding )
        {
            if ( commandBytes == null || commandBytes.Length == 0 )
            {
                return false;
            }
            string commandText = encoding.GetString( commandBytes );
            List<string> commandLines = commandText.Split( '\n' ).ToList();
            commandLines.RemoveAll( String.IsNullOrEmpty );
            if ( commandLines.Count == 0 )
            {
                return false;
            }
            string lastCommandLine = commandLines.Last();
            if ( Regex.Match( lastCommandLine, "^(?<code>[0-9]{3}) (?<message>.*)$" ).Success )
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}