using System.Collections.Generic;
using System.Linq;
using UsersLib.Checkers.Results;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Factories;
using UsersLib.Secure.Finders.Results;

namespace UsersLib.Checkers
{
    internal class DataBaseUserChecker : IUserChecker
    {
        private readonly IDbUserController _dbUserController;
        private readonly IDbSiteController _dbSiteController;
        private readonly ISecureFindersFactory _secureFindersFactory;

        public DataBaseUserChecker( IDbUserController dbUserController,
            IDbSiteController dbSiteController,
            ISecureFindersFactory secureFindersFactory )
        {
            _dbUserController = dbUserController;
            _dbSiteController = dbSiteController;
            _secureFindersFactory = secureFindersFactory;
        }

        public IUserCheckerResult Check( string userLogin, string userPass, string serverIdentify )
        {
            User user = _dbUserController.GetUser( userLogin );
            Site site = _dbSiteController.GetSite( serverIdentify );

            if ( user == null || site == null )
            {
                return null;
            }

            List<Group> userGroups = _dbUserController.GetUserGroups( user.Id );
            List<Group> siteUserGroups = _dbSiteController.GetUserGroupsBySite( site.Id );

            if ( userGroups == null || userGroups.Count == 0 )
            {
                return null;
            }

            if ( siteUserGroups == null || siteUserGroups.Count == 0 )
            {
                return null;
            }

            if ( !userGroups.Any( item => siteUserGroups.Any( siteUserGroup => siteUserGroup.Id == item.Id ) ) )
            {
                return null;
            }

            SiteSecureDataFinderResult finderResult =
                _secureFindersFactory.CreateSiteSecureDataFinder()
                    .FindeSiteSecureData( serverIdentify );

            return new UserCheckerResult(
                finderResult.UrlAddress,
                finderResult.Port,
                finderResult.Login,
                finderResult.Password );
        }
    }
}