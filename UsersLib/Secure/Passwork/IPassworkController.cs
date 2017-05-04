using UsersLib.Secure.Finders.Results;

namespace UsersLib.Secure.Passwork
{
    public interface IPassworkController
    {
        int Update();

        SiteSecureDataFinderResult GetSiteData( string storageId );
    }
}