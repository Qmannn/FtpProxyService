using Microsoft.Owin;
using Owin;


[assembly: OwinStartup( typeof( Proxynet.Startup ) )]
namespace Proxynet
{
    public partial class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            ConfigureAuth( app );
        }
    }
}