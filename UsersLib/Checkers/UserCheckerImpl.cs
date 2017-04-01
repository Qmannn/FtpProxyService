using System.Net;
using UsersLib.Checkers.Results;

namespace UsersLib.Checkers
{
    public class UserCheckerImpl : IUserChecker
    {
        public IUserCheckerResult Check( string userLogin, string userPass, string serverIdentify )
        {
            // Типа проверка в AD
            //if( userLogin != "anonymous" || userPass != "max" )
            //{
            //    return null;
            //}

            // Типа проверка прав доступа к удаленному серверу
            if ( serverIdentify == "local" )
            {
                // Если все хорошо - возврааем
                return new UserCheckerResult(
                    new IPEndPoint( new IPAddress( new byte[] { 127, 0, 0, 1 } ), 21 ),
                    userLogin,
                    userPass );
            }

            // Типа проверка прав доступа к удаленному серверу
            if( serverIdentify == "local1" )
            {
                // Если все хорошо - возврааем
                return new UserCheckerResult(
                    new IPEndPoint( new IPAddress( new byte[] { 192, 168, 1, 3 } ), 21 ),
                    userLogin,
                    userPass );
            }

            return null;
        }
    }
}