using System.Data.Entity;
using System.ServiceProcess;

namespace FtpProxyService
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new FtpProxyService() 
            };
            ServiceBase.Run( servicesToRun );
        }
    }
}
