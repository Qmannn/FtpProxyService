using UsersLib.DbControllers;
using UsersLib.Entity;

namespace UsersLib.Service.Log
{
    public class AccessLogger : IAccessLogger
    {
        private readonly IDbLogController _dbLogController;

        public AccessLogger(IDbLogController dbLogController)
        {
            _dbLogController = dbLogController;
        }

        public void LogAssess(string login, bool isAutenticated, string accessTarget = null, string role = null)
        {
            AccessLog log = new AccessLog
            {
                Login = login,
                AccessTarget = accessTarget,
                Role = role,
                IsAutenticated = isAutenticated
            };
            _dbLogController.Log(log);
        }
    }
}