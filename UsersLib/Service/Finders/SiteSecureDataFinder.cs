using System;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Service.Resolvers;

namespace UsersLib.Service.Finders
{
    internal class SiteSecureDataFinder : ISiteSecureDataFinder
    {
        private readonly IDbSiteController _dbSiteController;
        private readonly ISecureSiteDataResolver _secureSiteDataResolver;

        public SiteSecureDataFinder(IDbSiteController dbSiteController, ISecureSiteDataResolver secureSiteDataResolver)
        {
            _dbSiteController = dbSiteController;
            _secureSiteDataResolver = secureSiteDataResolver;
        }

        public SecureSiteData FindeSiteSecureData(string storageId)
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
            return _secureSiteDataResolver.ResolveSiteData(foundSite.SiteId);
        }
    }
}