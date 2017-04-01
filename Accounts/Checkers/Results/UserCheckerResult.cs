using System.Net;

namespace Accounts.Checkers.Results
{
    internal class UserCheckerResult : IUserCheckerResult
    {
        public EndPoint ServerEndPoint { get; private set; }

        public bool IsExist { get; private set; }

        public string Login { get; private set; }

        internal UserCheckerResult( bool isExist, EndPoint serverEndPoint, string login )
        {
            IsExist = isExist;
            Login = login;
            ServerEndPoint = isExist
                ? serverEndPoint
                : null;
        }
    }
}