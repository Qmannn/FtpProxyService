using System;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Secure.Finders.Results;

namespace UsersLib.Secure.Finders
{
    public class SiteSecureDataFinder : ISiteSecureDataFinder
    {
        private readonly IDbSiteController _dbSiteController;

        public SiteSecureDataFinder(IDbSiteController dbSiteController)
        {
            _dbSiteController = dbSiteController;
        }

        public SiteSecureDataFinderResult FindeSiteSecureData(string storageId)
        {
            if (String.IsNullOrEmpty(storageId))
            {
                return null;
            }

            Site foundSite = _dbSiteController.GetSite(storageId);
            if (foundSite == null)
            {
                return null;
            }

            return new SiteSecureDataFinderResult
            {
                Login = foundSite.Login,
                Password = foundSite.Password,
                Port = foundSite.Port,
                UrlAddress = foundSite.Address
            };
        }
    }
}