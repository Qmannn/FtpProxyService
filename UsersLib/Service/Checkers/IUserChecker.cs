using UsersLib.Service.Checkers.Results;

namespace UsersLib.Service.Checkers
{
    public interface IUserChecker
    {
        IUserCheckerResult Check( string userLogin, string userPass, string serverIdentify ); 
    }
}