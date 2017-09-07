using System;
using System.Configuration;

namespace FtpProxy.Configuration
{
    public static class Config
    {
        public static string CertificatePath
        {
            get { return ConfigurationManager.AppSettings[ "certificatePath" ]; }
        }

        public static string CertificatePassword
        {
            get { return ConfigurationManager.AppSettings[ "certificatePssword" ]; }
        }

        /// <summary>
        /// Представляет символ-разделитель логина и кода сайта, к которому происходит подключение
        /// </summary>
        public static string LoginSeparator
        {
            get { return ConfigurationManager.AppSettings[ "userLoginSiteSeparator" ]; }
        }

        public static int ListeningPort
        {
            get { return Convert.ToInt32( ConfigurationManager.AppSettings[ "listeningPort" ] ); }
        }
    }
}