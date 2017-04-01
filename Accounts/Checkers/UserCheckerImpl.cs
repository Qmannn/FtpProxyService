using System.Net;
using Checkers.Results;

namespace Accounts.Checkers
{
    public class UserCheckerImpl : IUserChecker
    {
        public IUserCheckerResult Check( string userLogin )
        {
            return new UserCheckerResult(
                true,
                new IPEndPoint( new IPAddress( new byte[] { 127, 0, 0, 1 } ), 21 ),
                userLogin );
        }
    }
}