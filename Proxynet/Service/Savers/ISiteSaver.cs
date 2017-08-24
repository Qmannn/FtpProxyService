using Proxynet.Models;

namespace Proxynet.Service.Savers
{
    public interface ISiteSaver
    {
        void SaveSite(SiteToSaveDto siteData);
    }
}