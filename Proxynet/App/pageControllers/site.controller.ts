module app.PageControllers {
    'use strict';

    class SiteController {
        public site: Models.ISite;

        private allGroups: Models.ISiteGroup[];
        private resourceService: Services.IResourceService;

        static $inject = ['$routeParams', 'app.services.resource'];

        constructor( routeParams: ng.route.IRouteParamsService,
            resourceService: Services.IResourceService ) {
            this.resourceService = resourceService;
            this.allGroups = null;
            this.site = null;

            var self = this;
            var siteId = routeParams[ 'siteid' ];
            this.resourceService.getSite( siteId,
                ( site: Models.ISite ): void => {
                    self.site = site;
                },
                self.onError );

            this.resourceService.getSiteGroups( ( groups: Models.ISiteGroup[] ): void => {
                    self.allGroups = groups;
                },
                self.onError );
        }

        public getAllowedToSiteGroups(): Models.ISiteGroup[] {
            if ( _.isNull( this.site ) || !_.isArray( this.allGroups ) ) {
                return <Models.ISiteGroup[]>{};
            }

            return _.filter( this.allGroups,
                ( group: Models.ISiteGroup ): boolean => {
                    return _.isUndefined( _.find( this.site.groups,
                        ( gr: Models.ISiteGroup ): boolean => {
                            return gr.id === group.id;
                        } ) );
                } );
        }

        public addGroup( group: Models.ISiteGroup ) {
            this.site.groups.push( group );
        }

        public removeGroup( group: Models.ISiteGroup ) {
            _.remove( this.site.groups,
                ( gr: Models.ISiteGroup ): boolean => {
                    return gr.id === group.id;
                } );
        }

        public saveSite() {
            var self = this;
            this.resourceService.saveSite( this.site,
                ( site: Models.ISite ): void => {
                    self.site = site;
                    self.onSaveSucefull();
                },
                self.onError );
        }

        /*--PRIVATE---*/

        private onError( data: any ): void {
            console.error( data );
        }

        private onSaveSucefull(): void {

        }
    }

    angular.module( 'app.pageControllers' ).controller( 'SiteController', SiteController );
}