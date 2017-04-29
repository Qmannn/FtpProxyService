module app.PageControllers {
    'use strict';

    import IUser = Models.IUser;
    import IUserGroup = Models.IUserGroup;
    
    class UserController {
        public user: IUser;
        public allowedToUserGroups: IUserGroup[];

        private allGroups: IUserGroup[];
        private resourceService: Services.IResourceService;

        static $inject = ['$routeParams', 'app.services.resource' ];

        constructor( routeParams: ng.route.IRouteParamsService,
            resourceService: Services.IResourceService ) {
            this.resourceService = resourceService;
            this.allGroups = null;
            this.user = null;

            var self = this;
            var userId = routeParams[ 'userid' ];
            this.resourceService.getUser( userId,
                ( user: IUser ): void => {
                    self.user = user;
                },
                self.onError );

            this.resourceService.getGroups( ( groups: IUserGroup[] ): void => {
                    self.allGroups = groups;
                },
                self.onError );
        }

        public getAllowedToUserGroups(): IUserGroup[] {
            if ( _.isNull( this.user ) || !_.isArray( this.allGroups ) ) {
                return <IUserGroup[]>{};
            }

            return _.filter( this.allGroups,
                ( group: IUserGroup ): boolean => {
                    return _.isUndefined( _.find( this.user.groups,
                        ( gr: IUserGroup ): boolean => {
                            return gr.id === group.id;
                        } ) );
                } );
        }
        
        public addGroup(group: IUserGroup) {
            this.user.groups.push( group );
        }

        public removeGroup( group: IUserGroup ) {
            _.remove( this.user.groups,
                ( gr: IUserGroup ): boolean => {
                    return gr.id === group.id;
                } );
        }

        public saveUser() {
            var self = this;
            this.resourceService.saveUser( this.user,
                ( user: IUser ): void => {
                    self.user = user;
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

    angular.module( 'app.pageControllers' ).controller( 'UserController', UserController );
}   