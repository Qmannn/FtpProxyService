using System.Collections.Generic;
using UsersLib.DbEntity;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public interface IDbSiteController
    {
        Site GetSite( int siteId );
        Site GetSite( string siteKey );
        List<Site> GetSites();
        List<SiteGroup> GetSiteGroups();
        List<SiteGroup> GetSiteGroups( string siteKey );
        List<UserGroup> GetUserGroupsBySite( int siteId );
        void SaveSite( Site site );
    }
}