using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using UsersLib.DbContextSettings;
using UsersLib.DbEntity;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public class DbSiteController : IDbSiteController
    {
        public Site GetSite(int siteId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Sites.FirstOrDefault(item => item.SiteId == siteId);
            }
        }

        public Site GetSite(string siteKey)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Sites.FirstOrDefault(item => item.SiteKey == siteKey);
            }
        }

        public List<Site> GetSites()
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Sites.ToList();
            }
        }

        public List<Group> GetSiteGroups()
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                List<DbGroup> siteGroups = dbContext.Groups
                    .ToList();
                return siteGroups.ConvertAll(item => new Group(item));
            }
        }

        public List<Group> GetSiteGroups(string siteKey)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                Site dbSite = dbContext.Sites
                    .FirstOrDefault(item => item.SiteKey == siteKey);

                return dbSite?.Groups.Select(item => new Group(item)).ToList() ?? new List<Group>();
            }
        }

        public List<Group> GetSiteGroups(int siteId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                Site dbSite = dbContext.Sites
                    .FirstOrDefault(item => item.SiteId == siteId);

                return dbSite?.Groups.Select(item => new Group(item)).ToList() ?? new List<Group>();
            }
        }

        public List<Group> GetUserGroupsBySite(int siteId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                Site dbSite = dbContext.Sites.FirstOrDefault(item => item.SiteId == siteId);

                if (dbSite == null)
                {
                    return new List<Group>();
                }

                return new List<Group>();
            }
        }

        public Dictionary<Site, List<Group>> GetSitesByGroups()
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Sites.Include(site => site.Groups)
                    .ToDictionary(item => item, item => item.Groups.ToList()
                        .ConvertAll(group => new Group(group)));
            }
        }

        public void SaveSite(Site site)
        {
            if (site == null)
            {
                return;
            }
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                dbContext.Sites.AddOrUpdate(site);
                dbContext.SaveChanges();
            }
        }

        public void SaveSiteGroups(int siteId, List<int> siteGroupIds)
        {
            if (siteId == 0)
            {
                return;
            }

            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                Site site = dbContext.Sites.Find(siteId);
                List<DbGroup> groups = dbContext.Groups
                    .Where(item => siteGroupIds.Contains(item.Id))
                    .ToList();
                if (site != null)
                {
                    site.Groups.Clear();
                    foreach (DbGroup group in groups)
                    {
                        site.Groups.Add(group);
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        public SecureSiteData GetSecureSiteData(int siteId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.SecureSiteData.FirstOrDefault(item => item.SiteId == siteId);
            }
        }

        public void SaveSecureSiteData(SecureSiteData siteData)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                dbContext.SecureSiteData.AddOrUpdate(siteData);
                dbContext.SaveChanges();
            }
        }
    }
}