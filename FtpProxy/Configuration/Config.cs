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
    }
}