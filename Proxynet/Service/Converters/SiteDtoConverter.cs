﻿using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public class SiteDtoConverter : ISiteDtoConverter
    {
        private readonly IGroupDtoConverter _groupDtoConverter;

        public SiteDtoConverter( IGroupDtoConverter sitesGroupDtoConverter )
        {
            _groupDtoConverter = sitesGroupDtoConverter;
        }

        public SiteDto Convert( Site site )
        {
            return new SiteDto
            {
                Id = site.Id,
                Name = site.SiteKey,
                Description = site.Description
            };
        }

        public Site Convert( SiteDto site )
        {
            return new Site
            {
                Description = site.Description,
                Id = site.Id,
                SiteKey = site.Name
            };
        }

        public List<SiteDto> Convert( List<Site> sites )
        {
            return sites.ConvertAll( Convert );
        }

        public List<SiteDto> ConvertFromSitesWithGroups( Dictionary<Site, List<Group>> groupsBySites )
        {
            List<SiteDto> result = new List<SiteDto>();
            foreach ( var pair in groupsBySites )
            {
                SiteDto site = Convert( pair.Key );
                site.Groups = _groupDtoConverter.Convert( pair.Value );
                result.Add( site );
            }
            return result;
        }
    }
}