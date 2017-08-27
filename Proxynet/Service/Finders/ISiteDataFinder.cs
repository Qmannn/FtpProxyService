using System.Collections.Generic;
using Proxynet.Models;

namespace Proxynet.Service.Finders
{
    public interface ISiteDataFinder
    {
        SiteDto GetSiteDto(int siteId);
        List<SiteDto> GetSitesWithoutSecureData();
    }
}