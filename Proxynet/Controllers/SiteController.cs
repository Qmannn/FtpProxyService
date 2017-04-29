﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Controllers
{
    public class SiteController : BaseController
    {
        private readonly IDbSiteController _dbSiteController;
        private readonly ISiteDtoConverter _siteDtoConverter;
        private readonly ISitesGroupDtoConverter _sitesGroupDtoConverter;

        public SiteController( 
            IDbSiteController dbSiteController, 
            ISiteDtoConverter siteDtoConverter, 
            ISitesGroupDtoConverter sitesGroupDtoConverter )
        {
            _dbSiteController = dbSiteController;
            _siteDtoConverter = siteDtoConverter;
            _sitesGroupDtoConverter = sitesGroupDtoConverter;
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

            List<SiteGroup> siteGroups = _dbSiteController.GetSiteGroups( siteId );

            SiteDto siteDto = _siteDtoConverter.Convert( site );
            siteDto.Groups = _sitesGroupDtoConverter.Convert( siteGroups );

            return Json( siteDto );
        }

        [HttpPost]
        public ContentResult GetGroups()
        {
            List<SiteGroup> siteGroups = _dbSiteController.GetSiteGroups();
            List<SitesGroupDto> siteGroupsDto = _sitesGroupDtoConverter.Convert( siteGroups );

            return Json( siteGroupsDto );
        }

        [HttpPost]
        public ActionResult SaveSite( string site )
        {
            var siteDto = JsonConvert.DeserializeObject<SiteDto>( site );

            if ( siteDto == null || siteDto.Id == 0 )
            {
                return HttpNotFound( "User not found" );
            }

            Site siteItem = _siteDtoConverter.Convert( siteDto );
            List<SiteGroup> siteGroups = _sitesGroupDtoConverter.Convert( siteDto.Groups );

            _dbSiteController.SaveSite( siteItem );
            _dbSiteController.SaveSiteGroups( siteItem.Id, siteGroups.Select( item => item.Id ).ToList() );

            return Json( siteDto );
        }
    }
}