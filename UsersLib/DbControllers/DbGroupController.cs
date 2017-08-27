using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using UsersLib.DbContextSettings;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public class DbGroupController : IDbGroupController
    {
        public Group SaveGroup(Group group)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                dbContext.Groups.AddOrUpdate(group);
                dbContext.SaveChanges();
                return group;
            }
        }

        public List<Group> GetGroups()
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Groups.ToList();
            }
        }

        public void DeleteGroup(int groupId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                IEnumerable<Group> groups = dbContext.Groups.Where(item => item.Id == groupId);
                dbContext.Groups.RemoveRange(groups);
                dbContext.SaveChanges();
            }
        }
    }
}