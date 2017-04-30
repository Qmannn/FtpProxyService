using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public interface ISiteDtoConverter
    {
        SiteDto Convert( Site site );
        Site Convert( SiteDto site );

        List<SiteDto> Convert( List<Site> sites );

        List<SiteDto> ConvertFromSitesWithGroups( Dictionary<Site, List<Group>> users );
    }
}