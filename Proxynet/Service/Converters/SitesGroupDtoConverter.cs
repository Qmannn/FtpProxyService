using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public class SitesGroupDtoConverter : ISitesGroupDtoConverter
    {
        public SitesGroupDto Convert( SiteGroup group )
        {
            return new SitesGroupDto
            {
                Id = group.Id,
                Name = group.Name
            };
        }

        public List<SitesGroupDto> Convert( List<SiteGroup> groups )
        {
            return groups.ConvertAll( Convert );
        }

        public List<SiteGroup> Convert( List<SitesGroupDto> groups )
        {
            return groups.ConvertAll( item => new SiteGroup
            {
                Id = item.Id,
                Name = item.Name
            } );
        }
    }
}