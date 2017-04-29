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
        List<SiteGroup> GetSiteGroups(int siteId);
        List<UserGroup> GetUserGroupsBySite( int siteId );
        Dictionary<Site, List<SiteGroup>> GetSitesByGroups();
        void SaveSite( Site site );
        void SaveSiteGroups( int siteId, List<int> siteGroupIds );
    }
}