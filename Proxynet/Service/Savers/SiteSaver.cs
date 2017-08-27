using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Service.Savers
{
    public class SiteSaver : ISiteSaver
    {
        private readonly ISiteDtoConverter _siteDtoConverter;
        private readonly IDbSiteController _dbSiteController;
        private readonly UsersLib.Service.Savers.ISiteSaver _siteSaver;

        public SiteSaver(ISiteDtoConverter siteDtoConverter,
            IDbSiteController dbSiteController,
            UsersLib.Service.Savers.ISiteSaver siteSaver)
        {
            _siteDtoConverter = siteDtoConverter;
            _dbSiteController = dbSiteController;
            _siteSaver = siteSaver;
        }

        public int SaveSite(SiteDto siteData)
        {
            SecureSiteData originalSecureSiteData = _dbSiteController.GetSecureSiteData(siteData.Id);
            Site site = _siteDtoConverter.Convert(siteData, originalSecureSiteData);
            _siteSaver.SaveSite(site);
            return site.SiteId;
        }
    }
}