using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using UsersLib.DbContextSettings;
using UsersLib.DbEntity;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public class DbSiteController : IDbSiteController
    {
        public Site GetSite( int siteId )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbSite dbSite = dbContext.Sites.FirstOrDefault( item => item.SiteId == siteId );

                return dbSite != null
                    ? new Site( dbSite )
                    : null;
            }
        }

        public Site GetSite( string siteKey )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbSite dbSite = dbContext.Sites.FirstOrDefault( item => item.SiteKey == siteKey );

                return dbSite != null
                    ? new Site( dbSite )
                    : null;
            }
        }

        public List<Site> GetSites()
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                return dbContext.Sites
                    .Select( item => new Site( item ) )
                    .ToList();
            }
        }

        public List<SiteGroup> GetSiteGroups()
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                return dbContext.SiteGroups
                    .Select( item => new SiteGroup( item ) )
                    .ToList();
            }
        }

        public List<SiteGroup> GetSiteGroups( string siteKey )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbSite dbSite = dbContext.Sites
                    .FirstOrDefault( item => item.SiteKey == siteKey );

                return dbSite != null
                    ? dbSite.SiteGroups.ConvertAll( item => new SiteGroup( item ) ).ToList()
                    : new List<SiteGroup>();
            }
        }

        public List<UserGroup> GetUserGroupsBySite( int siteId )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbSite dbSite = dbContext.Sites.FirstOrDefault( item => item.SiteId == siteId );

                if ( dbSite == null )
                {
                    return new List<UserGroup>();
                }

                return dbSite.SiteGroups
                        .SelectMany( item => item.UserGroups )
                        .Select( item => new UserGroup( item ) )
                        .ToList();
            }
        }

        public void SaveSite( Site site )
        {
            if ( site == null )
            {
                return;
            }
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                dbContext.Sites.AddOrUpdate( site.ConvertToDbSite() );
                dbContext.SaveChanges();
            }
        }
    }
}