using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Service.Validators
{
    public class SiteValidator : ISiteValidator
    {
        private readonly IDbSiteController _dbSiteController;

        public SiteValidator(IDbSiteController dbSiteController)
        {
            _dbSiteController = dbSiteController;
        }

        public bool ValidateSiteName(string siteName, int siteId)
        {
            Site existingSite = _dbSiteController.GetSite(siteName);
            return existingSite == null || existingSite.SiteId == siteId;
        }
    }
}