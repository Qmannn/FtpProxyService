module app.PageControllers {
    'use strict';
    
    class SitesController {
        public sites: Models.ISite[];

        private resourceService: Services.IResourceService;

        static $inject = ['app.services.resource'];

        constructor( resourceService: Services.IResourceService ) {
            this.resourceService = resourceService;
            this.getSites();
        }

        public updateSites() {
            var self = this;
            this.resourceService.updateSites( (): void => {
                    self.getSites();
                },
                ( data: any ): void => {
                    console.log( data );
                } );
        }

        public getSites() {
            var self = this;
            self.resourceService.getSites( ( sites: Models.ISite[] ): void => {
                    self.sites = sites;
                },
                ( data: any ): void => { console.log( data ); } );
        }
    }

    angular.module('app.pageControllers').controller('SitesController', SitesController);
}   