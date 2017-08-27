using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using UsersLib.DbContextSettings;
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
                return dbContext.Sites.FirstOrDefault(item => item.Name == siteKey);
            }
        }

        public List<Site> GetSites()
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Sites.ToList();
            }
        }

        public Dictionary<Site, List<Group>> GetSitesByGroups()
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Sites.Include(site => site.Groups)
                    .ToDictionary(item => item, item => item.Groups.ToList());
            }
        }
        
        public List<Group> GetSiteGroups(int siteId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Sites
                    .Where(item => item.SiteId == siteId)
                    .SelectMany(item => item.Groups)
                    .ToList();
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

        public void SaveSecureSiteData(SecureSiteData secureSiteData)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                dbContext.SecureSiteData.AddOrUpdate(secureSiteData);
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
                Site site = dbContext.Sites
                    .Where(item => item.SiteId == siteId)
                    .Include(item => item.Groups).FirstOrDefault();
                List<Group> groups = dbContext.Groups
                    .Where(item => siteGroupIds.Contains(item.Id))
                    .ToList();
                if (site != null)
                {
                    site.Groups.Clear();
                    foreach (Group group in groups)
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

        public void DeleteSite(int siteId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                IEnumerable<Site> sites = dbContext.Sites.Where(item => item.SiteId == siteId);
                dbContext.Sites.RemoveRange(sites);
                dbContext.SaveChanges();
            }
        }
    }
}