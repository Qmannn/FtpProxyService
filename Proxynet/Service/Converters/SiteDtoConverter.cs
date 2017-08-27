using System;
using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public class SiteDtoConverter : ISiteDtoConverter
    {
        private readonly IGroupDtoConverter _groupDtoConverter;

        public SiteDtoConverter(IGroupDtoConverter sitesGroupDtoConverter)
        {
            _groupDtoConverter = sitesGroupDtoConverter;
        }

        public SiteDto Convert(Site site)
        {
            return new SiteDto
            {
                Id = site.SiteId,
                Name = site.Name,
                Description = site.Description
            };
        }

        public SiteDto Convert(Site site, SecureSiteData secureSiteData)
        {
            SiteDto siteData = Convert(site);
            return ExtendFromSecureData(siteData, secureSiteData);
        }

        public Site Convert(SiteDto site, SecureSiteData originalSecureSiteData)
        {
            return new Site
            {
                Description = site.Description,
                SiteId = site.Id,
                Name = site.Name,
                Groups = _groupDtoConverter.Convert(site.Groups),
                SecureSiteData = ConvertSecureSiteData(site, originalSecureSiteData)
            };
        }

        public List<SiteDto> ConvertFromSitesWithGroups(Dictionary<Site, List<Group>> groupsBySites)
        {
            List<SiteDto> result = new List<SiteDto>();
            foreach (var pair in groupsBySites)
            {
                SiteDto site = Convert(pair.Key);
                site.Groups = _groupDtoConverter.Convert(pair.Value);
                result.Add(site);
            }
            return result;
        }

        public SiteDto Convert(Site site, List<Group> groups, SecureSiteData secureSiteData)
        {
            SiteDto siteData = Convert(site);
            siteData.Groups = _groupDtoConverter.Convert(groups);
            return ExtendFromSecureData(siteData, secureSiteData);
        }

        private SiteDto ExtendFromSecureData(SiteDto site, SecureSiteData secureSiteData)
        {
            site.Address = secureSiteData?.Url;
            site.Port = secureSiteData?.Port ?? 0;
            return site;
        }

        private SecureSiteData ConvertSecureSiteData(SiteDto site, SecureSiteData originalSecureSiteData)
        {
            string login = site.Login;
            string password = site.Password;
            bool secureDataChanged = !String.IsNullOrEmpty(site.Login) 
                || !String.IsNullOrEmpty(site.Password);
            if (originalSecureSiteData != null)
            {
                login = secureDataChanged
                    ? site.Login
                    : originalSecureSiteData.Login;
                password = secureDataChanged
                    ? site.Password
                    : originalSecureSiteData.Password;
            }
            return new SecureSiteData
            {
                Login = login,
                Password = password,
                Port = site.Port,
                Url = site.Address,
                SiteId = site.Id,
                NeedToEncrypt = secureDataChanged
            };
        }
    }
}