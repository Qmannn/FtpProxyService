using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                throw new ArgumentException();
            }

            List<string> argsParts = args.Split( '.' ).ToList();
            if ( argsParts.Count < 2 )
            {
                throw new ArgumentException();
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
                throw new ArgumentException();
            }

            List<string> argsParts = args.Split( '.' ).ToList();
            if( argsParts.Count < 2 )
            {
                throw new ArgumentException();
            }

            return argsParts.Last();
        }

        public string ResolvePassword( string args )
        {
            if ( String.IsNullOrEmpty( args ) )
            {
                throw new ArgumentException();
            }

            return args;
        }
    }
}