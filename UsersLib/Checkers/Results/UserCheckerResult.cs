using System.Net;

namespace UsersLib.Checkers.Results
{
    internal class UserCheckerResult : IUserCheckerResult
    {
        public string UrlAddress { get; private set; }
        
        public string Login { get; private set; }

        public string Pass { get; private set; }

        public int Port { get; set; }

        internal UserCheckerResult( string urlAddress, int port, string login, string pass )
        {
            Login = login;
            Pass = pass;
            UrlAddress = urlAddress;
            Port = port;
        }

    }
}