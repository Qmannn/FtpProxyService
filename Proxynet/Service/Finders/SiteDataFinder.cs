using System.Collections.Generic;
using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Service.Finders
{
    public class SiteDataFinder : ISiteDataFinder
    {
        private readonly ISiteDtoConverter _siteDtoConverter;
        private readonly IDbSiteController _dbSiteController;

        public SiteDataFinder(ISiteDtoConverter siteDtoConverter, IDbSiteController dbSiteController)
        {
            _siteDtoConverter = siteDtoConverter;
            _dbSiteController = dbSiteController;
        }

        public SiteDto GetSiteDto(int siteId)
        {
            Site site = _dbSiteController.GetSite(siteId);
            if (site == null)
            {
                return null;
            }
            List<Group> siteGroups = _dbSiteController.GetSiteGroups(site.SiteId);
            SecureSiteData secureSiteData = _dbSiteController.GetSecureSiteData(site.SiteId);
            return _siteDtoConverter.Convert(site, siteGroups, secureSiteData);
        }

        public List<SiteDto> GetSitesWithoutSecureData()
        {
            return _siteDtoConverter.ConvertFromSitesWithGroups(_dbSiteController.GetSitesByGroups());
        }
    }
}