using System.Configuration;
using System.Net;
using FtpProxy.Configuration;
using FtpProxy.Log;

namespace FtpProxy
{
    public class Program
    {
        private static FtpProxyWorker _worker;

        public static void Main(string[] args)
        {
            Logger.InitLogger();

            _worker = new FtpProxyWorker( Config.ListeningPort );
            _worker.Start();
        }

        public static void Stop()
        {
            _worker.Stop();
        }
    }
}
