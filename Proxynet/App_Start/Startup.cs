using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Diagnostics.CodeAnalysis;

namespace Proxynet
{
    [ExcludeFromCodeCoverage]
    public partial class Startup
    {
        public void ConfigureAuth( IAppBuilder app )
        {
            app.UseCookieAuthentication( new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString( "/Account/Login" ),
            } );
        }
    }
}