using log4net;
using log4net.Config;

namespace FtpProxy.Log
{
    public static class Logger
    {
        private static readonly ILog FtpLogger = LogManager.GetLogger("FtpLogger");

        public static ILog Log
        {
            get { return FtpLogger; }
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}