using System;
using UsersLib.Secure.Finders.Results;
using UsersLib.Secure.Passwork;

namespace UsersLib.Secure.Finders
{
    public class SiteSecureDataFinder : ISiteSecureDataFinder
    {
        public SiteSecureDataFinderResult FindeSiteSecureData( string storageId )
        {
            if ( String.IsNullOrEmpty( storageId ) )
            {
                return null;
            }

            IPassworkController passworkController = new PassworkController();
            return passworkController.GetSiteData( storageId );
        }
    }
}