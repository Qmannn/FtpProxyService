using System.Net;

namespace UsersLib.Checkers.Results
{
    internal class UserCheckerResult : IUserCheckerResult
    {
        public IPEndPoint ServerEndPoint { get; private set; }
        
        public string Login { get; private set; }

        public string Pass { get; private set; }

        internal UserCheckerResult( IPEndPoint serverEndPoint, string login, string pass )
        {
            Login = login;
            Pass = pass;
            ServerEndPoint = serverEndPoint;
        }
    }
}