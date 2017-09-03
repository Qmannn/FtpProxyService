namespace UsersLib.Service.Log
{
    public interface IAccessLogger
    {
        void LogAssess(string login, bool isAutenticated, string accessTarget = null, string role = null);
    }
}