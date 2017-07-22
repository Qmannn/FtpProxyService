using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UsersLib.Checkers.Results;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Factories;
using UsersLib.Secure.Auth;
using UsersLib.Secure.Finders.Results;

namespace UsersLib.Checkers
{
    internal class DataBaseUserChecker : IUserChecker
    {
        private readonly IDbUserController _dbUserController;
        private readonly IDbSiteController _dbSiteController;
        private readonly ISecureFindersFactory _secureFindersFactory;
        private readonly ILdapAuthorizer _ldapAuthorizer;

        public DataBaseUserChecker( IDbUserController dbUserController,
            IDbSiteController dbSiteController,
            ISecureFindersFactory secureFindersFactory, 
            ILdapAuthorizer ldapAuthorizer )
        {
            _dbUserController = dbUserController;
            _dbSiteController = dbSiteController;
            _secureFindersFactory = secureFindersFactory;
            _ldapAuthorizer = ldapAuthorizer;
        }

        public IUserCheckerResult Check( string userLogin, string userPass, string siteKey )
        {
            if ( !_ldapAuthorizer.ValidateCredentials( userLogin, userPass, false ) )
            {
                return null;
            }

            User user = _dbUserController.GetUser( userLogin );
            Site site = _dbSiteController.GetSite( siteKey );

            if ( user == null || site == null )
            {
                return null;
            }

            List<Group> userGroups = _dbUserController.GetUserGroups( user.Id );
            List<Group> siteGroups = _dbSiteController.GetSiteGroups( site.Id );

            if ( userGroups == null || userGroups.Count == 0 )
            {
                return null;
            }

            if ( siteGroups == null || siteGroups.Count == 0 )
            {
                return null;
            }
            
            if ( !userGroups.Any( item => siteGroups.Any( siteGroup => siteGroup.Id == item.Id ) ) )
            {
                return null;
            }

            SiteSecureDataFinderResult finderResult =
                _secureFindersFactory.CreateSiteSecureDataFinder()
                    .FindeSiteSecureData( site.StorageId );

            if( finderResult == null )
            {
                return null;
            }

            return new UserCheckerResult(
                finderResult.UrlAddress,
                finderResult.Port,
                finderResult.Login,
                finderResult.Password );
        }
    }
}