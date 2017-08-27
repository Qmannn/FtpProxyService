using System.Collections.Generic;
using System.Linq;
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

        public void SaveSite(Site site)
        {
            IEnumerable<Group> siteGroups = site.Groups;
            site.Groups = null;
            _dbSiteController.SaveSite(site);
            if (siteGroups != null)
            {
                _dbSiteController.SaveSiteGroups(site.SiteId, siteGroups.Select(item => item.Id).ToList());
            }
            if (site.SecureSiteData != null)
            {
                SaveSecureSiteData(site.SecureSiteData);
            }
        }

        private void SaveSecureSiteData(SecureSiteData secureSiteData)
        {
            if (secureSiteData.NeedToEncrypt)
            {
                secureSiteData.Password = _cryptoService.EncryptString(secureSiteData.Password);
                secureSiteData.Login = _cryptoService.EncryptString(secureSiteData.Login);
            }
            _dbSiteController.SaveSecureSiteData(secureSiteData);
        }
    }
}