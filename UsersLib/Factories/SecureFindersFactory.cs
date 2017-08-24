using UsersLib.DbControllers;
using UsersLib.Secure;
using UsersLib.Secure.Finders;

namespace UsersLib.Factories
{
    public class SecureFindersFactory : ISecureFindersFactory
    {
        private IDbSiteController _dbSiteController;

        private IDbSiteController DbSiteController => _dbSiteController ?? (_dbSiteController = new DbSiteController());

        public ISiteSecureDataFinder CreateSiteSecureDataFinder()
        {
            return new SiteSecureDataFinder(DbSiteController);
        }
    }
}