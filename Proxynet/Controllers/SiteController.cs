using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Secure.Passwork;

namespace Proxynet.Controllers
{
    public class SiteController : BaseController
    {
        private readonly IDbSiteController _dbSiteController;
        private readonly ISiteDtoConverter _siteDtoConverter;
        private readonly IGroupDtoConverter _groupDtoConverter;

        private readonly IPassworkController _sitesUpdater;

        public SiteController( 
            IDbSiteController dbSiteController, 
            ISiteDtoConverter siteDtoConverter,
            IGroupDtoConverter groupDtoConverter, 
            IPassworkController sitesUpdater )
        {
            _dbSiteController = dbSiteController;
            _siteDtoConverter = siteDtoConverter;
            _groupDtoConverter = groupDtoConverter;
            _sitesUpdater = sitesUpdater;
        }

        [HttpPost]
        public ActionResult GetSites()
        {
            var sitesByGroups = _dbSiteController.GetSitesByGroups();
            return Json( _siteDtoConverter.ConvertFromSitesWithGroups( sitesByGroups ) );
        }

        [HttpPost]
        public ActionResult GetSite( int siteId )
        {
            Site site = _dbSiteController.GetSite( siteId );
            if ( site == null )
            {
                return HttpNotFound( $"Site #{siteId} not found" );
            }

            List<Group> siteGroups = _dbSiteController.GetSiteGroups( siteId );

            SiteDto siteDto = _siteDtoConverter.Convert( site );
            siteDto.Groups = _groupDtoConverter.Convert( siteGroups );

            return Json( siteDto );
        }

        [HttpPost]
        public ContentResult GetGroups()
        {
            List<Group> siteGroups = _dbSiteController.GetSiteGroups();
            List<GroupDto> siteGroupsDto = _groupDtoConverter.Convert( siteGroups );

            return Json( siteGroupsDto );
        }

        [HttpPost]
        public ActionResult SaveSite( string site )
        {
            var siteDto = JsonConvert.DeserializeObject<SiteDto>( site );

            if ( siteDto == null || siteDto.Id == 0 )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }

            Site siteData = _dbSiteController.GetSite( siteDto.Id );
            if ( siteData == null )
            {
                return HttpNotFound();
            }

            Site siteItem = _siteDtoConverter.Convert( siteDto );
            
            // Init internal data
            siteItem.StorageId = siteData.StorageId;

            List<Group> siteGroups = _groupDtoConverter.Convert( siteDto.Groups );

            _dbSiteController.SaveSite( siteItem );
            _dbSiteController.SaveSiteGroups( siteItem.Id, siteGroups.Select( item => item.Id ).ToList() );

            return Json( siteDto );
        }

        [HttpPost]
        public ActionResult UpdateSites()
        {
            _sitesUpdater.Update();
            return new EmptyResult();
        }
    }
}