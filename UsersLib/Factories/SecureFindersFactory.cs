using UsersLib.Secure;
using UsersLib.Secure.Finders;

namespace UsersLib.Factories
{
    public class SecureFindersFactory : ISecureFindersFactory
    {
        public ISiteSecureDataFinder CreateSiteSecureDataFinder()
        {
            return new SiteSecureDataFinder();
        }
    }
}