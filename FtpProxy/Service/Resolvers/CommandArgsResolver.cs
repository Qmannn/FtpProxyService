using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FtpProxy.Configuration;

namespace FtpProxy.Service.Resolvers
{
    /// <summary>
    /// Выделяет необходиме компоненты из аргументов FTP команды
    /// </summary>
    public class CommandArgsResolver
    {
        public string ResolveUserLogin( string args )
        {
            if ( String.IsNullOrEmpty( args ) )
            {
                throw new ArgumentException("UserName is null or empty");
            }

            List<string> argsParts = args.Split( Config.LoginSeparator.First() ).ToList();
            if ( argsParts.Count < 2 )
            {
                throw new ArgumentException("UserName not contains remote site key");
            }
            
            StringBuilder loginBuilder = new StringBuilder();
            for ( int i = 0; i < argsParts.Count - 1; i++ )
            {
                loginBuilder.Append( argsParts[ i ] );
            }

            return loginBuilder.ToString();
        }

        public string ResolveServerIdentifier( string args )
        {
            if( String.IsNullOrEmpty( args ) )
            {
                throw new ArgumentException("Site identifier is null");
            }

            List<string> argsParts = args.Split( Config.LoginSeparator.First() ).ToList();
            if( argsParts.Count < 2 )
            {
                throw new ArgumentException("Login not contains site key");
            }

            return argsParts.Last();
        }

        public string ResolvePassword( string args )
        {
            if ( String.IsNullOrEmpty( args ) )
            {
                throw new ArgumentException("User password is null or empty");
            }

            return args;
        }
    }
}