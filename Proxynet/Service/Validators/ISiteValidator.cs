namespace Proxynet.Service.Validators
{
    public interface ISiteValidator
    {
        bool ValidateSiteName(string siteName, int siteId);
    }
}