using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public interface IDbLogController
    {
        void Log(AccessLog logEntity);
    }
}