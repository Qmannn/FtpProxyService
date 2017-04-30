module app.PageControllers {
    'use strict';

    class SiteController {
        public site: Models.ISite;
        public newGroupName: string;

        private allGroups: Models.IGroup[];
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

            this.resourceService.getSiteGroups( ( groups: Models.IGroup[] ): void => {
                    self.allGroups = groups;
                },
                self.onError );
        }

        public getAllowedToSiteGroups(): Models.IGroup[] {
            if ( _.isNull( this.site ) || !_.isArray( this.allGroups ) ) {
                return <Models.IGroup[]>{};
            }

            return _.filter( this.allGroups,
                ( group: Models.IGroup ): boolean => {
                    return _.isUndefined( _.find( this.site.groups,
                        ( gr: Models.IGroup ): boolean => {
                            return gr.id === group.id;
                        } ) );
                } );
        }

        public addGroup( group: Models.IGroup ) {
            this.site.groups.push( group );
        }

        public removeGroup( group: Models.IGroup ) {
            _.remove( this.site.groups,
                ( gr: Models.IGroup ): boolean => {
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

        public addNewGroup(name: string) {
            var self = this;
            this.resourceService.saveGroup(name,
                (group: Models.IGroup): void => {
                    self.allGroups.push(group);
                },
                self.onError);
            this.newGroupName = "";
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