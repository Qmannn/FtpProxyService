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

        public List<Group> GetSiteGroups()
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                List<DbGroup> siteGroups = dbContext.Groups
                    .ToList();
                return siteGroups.ConvertAll( item => new Group( item ) );
            }
        }

        public List<Group> GetSiteGroups( string siteKey )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbSite dbSite = dbContext.Sites
                    .FirstOrDefault( item => item.SiteKey == siteKey );

                return dbSite != null
                    ? dbSite.Groups.Select( item => new Group( item ) ).ToList()
                    : new List<Group>();
            }
        }

        public List<Group> GetSiteGroups( int siteId )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbSite dbSite = dbContext.Sites
                    .FirstOrDefault( item => item.SiteId == siteId );

                return dbSite != null
                    ? dbSite.Groups.Select( item => new Group( item ) ).ToList()
                    : new List<Group>();
            }
        }

        public List<Group> GetUserGroupsBySite( int siteId )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbSite dbSite = dbContext.Sites.FirstOrDefault( item => item.SiteId == siteId );

                if ( dbSite == null )
                {
                    return new List<Group>();
                }

                return new List<Group>();
            }
        }

        public Dictionary<Site, List<Group>> GetSitesByGroups()
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                return dbContext.Sites.Include( site => site.Groups )
                    .ToDictionary( item => new Site( item ),
                        item => item.Groups.ToList()
                            .ConvertAll( group => new Group( group ) ) );
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

        public void SaveSiteGroups( int siteId, List<int> siteGroupIds )
        {
            if ( siteId == 0 )
            {
                return;
            }

            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbSite site = dbContext.Sites.Find( siteId );
                List<DbGroup> groups = dbContext.Groups
                    .Where( item => siteGroupIds.Contains( item.Id ) )
                    .ToList();
                if ( site != null )
                {
                    site.Groups.Clear();
                    foreach ( DbGroup group in groups )
                    {
                        site.Groups.Add( group );
                    }
                    dbContext.SaveChanges();
                }
            }
        }
    }
}