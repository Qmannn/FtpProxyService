using System.ServiceProcess;

namespace FtpProxyService
{
    public partial class FtpProxyService : ServiceBase
    {
        public FtpProxyService()
        {
            InitializeComponent();
        }

        protected override void OnStart( string[] args )
        {
            FtpProxy.Program.Main( new string[] { } );
        }

        protected override void OnStop()
        {
            FtpProxy.Program.Stop();
        }
    }
}
