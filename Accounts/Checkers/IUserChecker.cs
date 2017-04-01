using Accounts.Checkers.Results;

namespace Accounts.Checkers
{
    public interface IUserChecker
    {
        IUserCheckerResult Check( string userLogin ); 
    }
}