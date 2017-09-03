using System.Collections.Generic;
using System.Linq;
using UsersLib.Checkers.Results;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Service.Auth;
using UsersLib.Service.Checkers.Results;
using UsersLib.Service.Resolvers;

namespace UsersLib.Service.Checkers
{
    internal class UserChecker : IUserChecker
    {
        private readonly IDbUserController _dbUserController;
        private readonly IDbSiteController _dbSiteController;
        private readonly IDbAuthController _dbAuthController;
        private readonly IAuthorizer _authorizer;
        private readonly ISecureSiteDataResolver _secureSiteDataResolver;

        public UserChecker(IDbUserController dbUserController, 
            IDbSiteController dbSiteController, 
            IAuthorizer ldapAuthorizer, 
            ISecureSiteDataResolver secureSiteDataResolver, 
            IDbAuthController dbAuthController)
        {
            _dbUserController = dbUserController;
            _dbSiteController = dbSiteController;
            _authorizer = ldapAuthorizer;
            _secureSiteDataResolver = secureSiteDataResolver;
            _dbAuthController = dbAuthController;
        }

        public IUserCheckerResult Check(string userLogin, string userPass, string siteKey)
        {
            if (!_authorizer.ValidateCredentials(userLogin, userPass, UserRoleKind.Unknown))
            {
                return null;
            }

            int userId = _dbAuthController.GetUserId(userLogin);

            Site site = _dbSiteController.GetSite(siteKey);
            if (site == null)
            {
                return null;
            }

            List<Group> userGroups = _dbUserController.GetUserGroups(userId);
            List<Group> siteGroups = _dbSiteController.GetSiteGroups(site.SiteId);

            if (userGroups == null || userGroups.Count == 0)
            {
                return null;
            }

            if (siteGroups == null || siteGroups.Count == 0)
            {
                return null;
            }

            if (!userGroups.Any(item => siteGroups.Any(siteGroup => siteGroup.Id == item.Id)))
            {
                return null;
            }

            SecureSiteData secureSiteData = _secureSiteDataResolver.ResolveSiteData(site.SiteId);
            if (secureSiteData == null)
            {
                return null;
            }

            return new UserCheckerResult(secureSiteData);
        }
    }
}