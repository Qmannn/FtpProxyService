using Proxynet.Models;

namespace Proxynet.Service.Savers
{
    public interface ISiteSaver
    {
        int SaveSite(SiteDto siteData);
    }
}