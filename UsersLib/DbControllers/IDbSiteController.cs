using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public interface IDbSiteController
    {
        Site GetSite(int siteId);
        Site GetSite(string siteKey);
        List<Site> GetSites();
        List<Group> GetSiteGroups(int siteId);
        Dictionary<Site, List<Group>> GetSitesByGroups();
        void SaveSite(Site site);
        void SaveSecureSiteData(SecureSiteData secureSiteData);
        void SaveSiteGroups(int siteId, List<int> groupIds);
        SecureSiteData GetSecureSiteData(int siteId);
        void DeleteSite(int siteId);
    }
}