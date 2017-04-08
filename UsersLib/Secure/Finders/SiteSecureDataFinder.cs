using UsersLib.Secure.Finders.Results;

namespace UsersLib.Secure.Finders
{
    public class SiteSecureDataFinder : ISiteSecureDataFinder
    {
        public SiteSecureDataFinderResult FindeSiteSecureData( string siteIdentifier )
        {
            switch ( siteIdentifier )
            {
                case "ftp":
                    return new SiteSecureDataFinderResult
                    {
                        UrlAddress = "localhost",
                        Login = "anonymous",
                        Password = "blabla",
                        Port = 21
                    };
                case "homeftp":
                    return new SiteSecureDataFinderResult
                    {
                        UrlAddress = "127.0.0.1",
                        Login = "max",
                        Password = "max",
                        Port = 55000
                    };
            }
            return null;
        }
    }
}