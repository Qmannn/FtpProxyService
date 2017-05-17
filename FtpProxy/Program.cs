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
            Logger.InitLogger();

            _worker = new FtpProxyWorker( 26000 );
            _worker.Start();
        }

        public static void Stop()
        {
            _worker.Stop();
        }
    }
}
