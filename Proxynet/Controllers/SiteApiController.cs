using System;
using System.Collections.Generic;
using System.Web.Http;
using Proxynet.Models;
using Proxynet.Service.Finders;
using Proxynet.Service.Removers;
using Proxynet.Service.Savers;
using Proxynet.Service.Validators;

namespace Proxynet.Controllers
{
    [RoutePrefix("service/sites")]
    public class SiteApiController : BaseApiController
    {
        private readonly ISiteSaver _siteSaver;
        private readonly ISiteDataFinder _siteDataFinder;
        private readonly IGroupDataFinder _groupDataFinder;
        private readonly IDataRemover _dataRemover;
        private readonly ISiteValidator _siteValidator;

        public SiteApiController(
            ISiteSaver siteSaver,
            ISiteDataFinder siteDataFinder,
            IGroupDataFinder groupDataFinder, 
            IDataRemover dataRemover, 
            ISiteValidator siteValidator)
        {
            _siteSaver = siteSaver;
            _siteDataFinder = siteDataFinder;
            _groupDataFinder = groupDataFinder;
            _dataRemover = dataRemover;
            _siteValidator = siteValidator;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetSites()
        {
            return Ok(_siteDataFinder.GetSitesWithoutSecureData());
        }

        [HttpGet]
        [Route("get-site")]
        public IHttpActionResult GetSite(int siteId)
        {
            SiteDto siteData = _siteDataFinder.GetSiteDto(siteId);
            if (siteData == null)
            {
                return BadRequest();
            }
            return Ok(siteData);
        }

        [HttpGet]
        [Route("get-groups")]
        public IHttpActionResult GetGroups()
        {
            List<GroupDto> siteGroupsDto = _groupDataFinder.GetGroupsDto();
            return Ok(siteGroupsDto);
        }

        [HttpPut]
        [Route("save-site")]
        public IHttpActionResult SaveSite(SiteDto site)
        {
            if (site == null)
            {
                return BadRequest();
            }
            int savedSiteId = _siteSaver.SaveSite(site);
            site = _siteDataFinder.GetSiteDto(savedSiteId);
            return Ok(site);
        }

        [HttpDelete]
        [Route("delete-site")]
        public IHttpActionResult DeleteSite(int siteId)
        {
            _dataRemover.RemoveSite(siteId);
            return Ok();
        }

        [HttpGet]
        [Route("check-site-name")]
        public IHttpActionResult CheckSiteName(string siteName, int siteId)
        {
            if (String.IsNullOrEmpty(siteName))
            {
                return Ok(false);
            }

            bool siteNameIsValid = _siteValidator.ValidateSiteName(siteName, siteId);
            return Ok(siteNameIsValid);
        }
    }
}