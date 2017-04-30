using System.Collections.Generic;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public interface IDbSiteController
    {
        Site GetSite( int siteId );
        Site GetSite( string siteKey );
        List<Site> GetSites();
        List<Group> GetSiteGroups();
        List<Group> GetSiteGroups( string siteKey );
        List<Group> GetSiteGroups(int siteId);
        List<Group> GetUserGroupsBySite( int siteId );
        Dictionary<Site, List<Group>> GetSitesByGroups();
        void SaveSite( Site site );
        void SaveSiteGroups( int siteId, List<int> groupIds );
    }
}