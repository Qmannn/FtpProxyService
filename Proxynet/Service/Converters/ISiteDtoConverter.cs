using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public interface ISiteDtoConverter
    {
        SiteDto Convert(Site site);
        SiteDto Convert(Site site, SecureSiteData secureSiteData);
        Site Convert(SiteDto site, SecureSiteData originalSecureSiteData);
        List<SiteDto> ConvertFromSitesWithGroups(Dictionary<Site, List<Group>> sites);
        SiteDto Convert(Site site, List<Group> groups, SecureSiteData secureSiteData);
    }
}