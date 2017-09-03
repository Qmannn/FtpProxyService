using UsersLib.DbContextSettings;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public class DbLogController : IDbLogController
    {
        public void Log(AccessLog logEntity)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                dbContext.AccessLog.Add(logEntity);
                dbContext.SaveChanges();
            }
        }
    }
}