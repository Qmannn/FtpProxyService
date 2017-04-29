using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public interface ISitesGroupDtoConverter
    {
        SitesGroupDto Convert( SiteGroup group );
        List<SitesGroupDto> Convert( List<SiteGroup> groups );
        List<SiteGroup> Convert( List<SitesGroupDto> groups );
    }
}