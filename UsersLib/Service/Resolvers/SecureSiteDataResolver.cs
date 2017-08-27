using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Service.Cryptography;

namespace UsersLib.Service.Resolvers
{
    internal class SecureSiteDataResolver : ISecureSiteDataResolver
    {
        private readonly IDbSiteController _dbSiteController;
        private readonly ICryptoService _cryptoService;

        public SecureSiteDataResolver(IDbSiteController dbSiteController, ICryptoService cryptoService)
        {
            _dbSiteController = dbSiteController;
            _cryptoService = cryptoService;
        }

        public SecureSiteData ResolveSiteData(int siteId)
        {
            SecureSiteData secureSiteData = _dbSiteController.GetSecureSiteData(siteId);
            if (secureSiteData == null)
            {
                return null;
            }
            secureSiteData.Password = _cryptoService.DecryptData(secureSiteData.Password);
            secureSiteData.Login = _cryptoService.DecryptData(secureSiteData.Login);
            return secureSiteData;
        }
    }
}