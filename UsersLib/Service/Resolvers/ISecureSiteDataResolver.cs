using UsersLib.Entity;

namespace UsersLib.Service.Resolvers
{
    public interface ISecureSiteDataResolver
    {
        SecureSiteData ResolveSiteData(int siteId);
    }
}