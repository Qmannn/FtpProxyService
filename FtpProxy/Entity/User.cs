using System.Net;

namespace FtpProxy.Entity
{
    public class User
    {
        public string Login { get; private set; }

        public string Password { get; private set; }

        public EndPoint RemoteEndPoint { get; private set; }

        public User( string login, string password, EndPoint remoteEndPoint )
        {
            Login = login;
            Password = password;
            RemoteEndPoint = remoteEndPoint;
        }
    }
}