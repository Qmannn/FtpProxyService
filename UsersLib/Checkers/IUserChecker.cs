using UsersLib.Checkers.Results;

namespace UsersLib.Checkers
{
    public interface IUserChecker
    {
        IUserCheckerResult Check( string userLogin, string userPass, string serverIdentify ); 
    }
}