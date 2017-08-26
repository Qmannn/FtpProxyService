using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Service.Cryptography;

namespace UsersLib.Service.Savers
{
    internal class SiteSaver : ISiteSaver
    {
        private readonly IDbSiteController _dbSiteController;
        private readonly ICryptoService _cryptoService;

        public SiteSaver(ICryptoService cryptoService, IDbSiteController dbSiteController)
        {
            _cryptoService = cryptoService;
            _dbSiteController = dbSiteController;
        }

        public void SaveSite(Site site, SecureSiteData secureSiteData)
        {
            secureSiteData.Password = _cryptoService.EncryptString(secureSiteData.Password);
            secureSiteData.Login = _cryptoService.EncryptString(secureSiteData.Login);
            site.SecureSiteData = secureSiteData;
            _dbSiteController.SaveSite(site);
        }
    }
}