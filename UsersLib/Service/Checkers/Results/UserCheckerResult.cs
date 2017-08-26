using System.Net;
using UsersLib.Entity;
using UsersLib.Service.Checkers.Results;

namespace UsersLib.Checkers.Results
{
    internal class UserCheckerResult : IUserCheckerResult
    {
        public string UrlAddress { get; private set; }
        
        public string Login { get; private set; }

        public string Pass { get; private set; }

        public int Port { get; set; }

        internal UserCheckerResult( SecureSiteData secureSiteData )
        {
            Login = secureSiteData.Login;
            Pass = secureSiteData.Password;
            UrlAddress = secureSiteData.Url;
            Port = secureSiteData.Port;
        }

    }
}