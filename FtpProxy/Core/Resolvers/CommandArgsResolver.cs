using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FtpProxy.Configuration;

namespace FtpProxy.Core.Resolvers
{
    /// <summary>
    /// Выделяет необходиме компоненты из аргументов FTP команды
    /// </summary>
    public class CommandArgsResolver : ICommandArgsResolver
    {
        public string ResolveUserLogin(string args)
        {
            if (String.IsNullOrEmpty(args))
            {
                return string.Empty;
            }

            List<string> argsParts = args.Split(Config.LoginSeparator.First()).ToList();
            if (argsParts.Count < 2)
            {
                return string.Empty;
            }

            StringBuilder loginBuilder = new StringBuilder();
            for (int i = 0; i < argsParts.Count - 1; i++)
            {
                loginBuilder.Append(argsParts[i]);
            }

            return loginBuilder.ToString();
        }

        public string ResolveServerIdentifier(string args)
        {
            if (String.IsNullOrEmpty(args))
            {
                return string.Empty;
            }

            List<string> argsParts = args.Split(Config.LoginSeparator.First()).ToList();
            if (argsParts.Count < 2)
            {
                return string.Empty;
            }

            return argsParts.Last();
        }

        public string ResolvePassword(string args)
        {
            if (String.IsNullOrEmpty(args))
            {
                return string.Empty;
            }

            return args;
        }
    }
}