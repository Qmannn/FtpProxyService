using System.Configuration;
using System.Net;
using FtpProxy.Log;

namespace FtpProxy
{
    public class Program
    {
        private static FtpProxyWorker _worker;

        public static void Main(string[] args)
        {

            var a = ConfigurationManager.AppSettings[ "certificatePath" ];

            Logger.InitLogger();

            _worker = new FtpProxyWorker( IPAddress.Any, 26000 );
            _worker.Start();
        }

        public static void Stop()
        {
            _worker.Stop();
        }
    }
}
