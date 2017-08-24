using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Proxynet.Models;
using Proxynet.Service.Converters;
using Proxynet.Service.Savers;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Controllers
{
    [RoutePrefix("service/sites")]
    public class SiteApiController : BaseApiController
    {
        private readonly IDbSiteController _dbSiteController;
        private readonly ISiteDtoConverter _siteDtoConverter;
        private readonly IGroupDtoConverter _groupDtoConverter;
        private readonly ISiteSaver _siteSaver;

        public SiteApiController(IDbSiteController dbSiteController,
            ISiteDtoConverter siteDtoConverter,
            IGroupDtoConverter groupDtoConverter,
            ISiteSaver siteSaver)
        {
            _dbSiteController = dbSiteController;
            _siteDtoConverter = siteDtoConverter;
            _groupDtoConverter = groupDtoConverter;
            _siteSaver = siteSaver;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetSites()
        {
            return Ok(_siteDtoConverter.ConvertFromSitesWithGroups(_dbSiteController.GetSitesByGroups()));
        }

        [HttpPost]
        [Route("get-site")]
        public IHttpActionResult GetSite(int siteId)
        {
            Site site = _dbSiteController.GetSite(siteId);
            if (site == null)
            {
                return BadRequest();
            }

            List<Group> siteGroups = _dbSiteController.GetSiteGroups(siteId);

            SiteDto siteDto = _siteDtoConverter.Convert(site);
            siteDto.Groups = _groupDtoConverter.Convert(siteGroups);

            return Ok(siteDto);
        }

        [HttpPost]
        [Route("get-groups")]
        public IHttpActionResult GetGroups()
        {
            List<Group> siteGroups = _dbSiteController.GetSiteGroups();
            List<GroupDto> siteGroupsDto = _groupDtoConverter.Convert(siteGroups);
            return Ok(siteGroupsDto);
        }

        [HttpPost]
        [Route("save-site")]
        public IHttpActionResult SaveSite(SiteDto site)
        {
            if (site == null || site.Id == 0)
            {
                return BadRequest();
            }

            Site otiginalSite = _dbSiteController.GetSite(site.Id);
            if (otiginalSite == null)
            {
                return NotFound();
            }

            Site siteItem = _siteDtoConverter.Convert(site);

            // Init internal data
            siteItem.StorageId = otiginalSite.StorageId;

            List<Group> siteGroups = _groupDtoConverter.Convert(site.Groups);

            _dbSiteController.SaveSite(siteItem);
            _dbSiteController.SaveSiteGroups(siteItem.Id, siteGroups.Select(item => item.Id).ToList());

            return Ok(site);
        }

        [HttpPost]
        [Route("create-site")]
        public IHttpActionResult CreateSite(SiteToSaveDto siteData)
        {
            if (siteData == null)
            {
                return BadRequest();
            }
            _siteSaver.SaveSite(siteData);
            return Ok(siteData);
        }
    }
}