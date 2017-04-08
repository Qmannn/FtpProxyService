using UsersLib.Secure.Finders.Results;

namespace UsersLib.Secure.Finders
{
    public interface ISiteSecureDataFinder
    {
        SiteSecureDataFinderResult FindeSiteSecureData( string siteIdentifier );
    }
}