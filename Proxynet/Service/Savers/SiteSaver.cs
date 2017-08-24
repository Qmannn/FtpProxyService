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

        public SiteSaver( ISiteDtoConverter siteDtoConverter, IDbSiteController dbSiteController)
        {
            _siteDtoConverter = siteDtoConverter;
            _dbSiteController = dbSiteController;
        }

        public void SaveSite(SiteToSaveDto siteData)
        {
            Site site = _siteDtoConverter.ConvertFromCreateData(siteData);
            _dbSiteController.SaveSite(site);
        }
    }
}