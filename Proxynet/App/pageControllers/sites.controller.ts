module app.PageControllers {
    'use strict';
    
    class SitesController {
        public sites: Models.ISite[];

        private resourceService: Services.IResourceService;

        static $inject = ['app.services.resource'];

        constructor( resourceService: Services.IResourceService ) {
            this.resourceService = resourceService;

            var self = this;

            resourceService.getSites( ( sites: Models.ISite[] ): void => {
                    self.sites = sites;
                },
                ( data: any ): void => { console.log( data ); } );
        }
    }

    angular.module('app.pageControllers').controller('SitesController', SitesController);
}   