using UsersLib.Secure;
using UsersLib.Secure.Finders;

namespace UsersLib.Factories
{
    internal interface ISecureFindersFactory
    {
        ISiteSecureDataFinder CreateSiteSecureDataFinder();
    }
}