using UsersLib.Entity;

namespace UsersLib.Service.Finders
{
    internal interface ISiteSecureDataFinder
    {
        SecureSiteData FindeSiteSecureData(string storageId);
    }
}