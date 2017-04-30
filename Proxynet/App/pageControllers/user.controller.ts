module app.PageControllers {
    'use strict';

    import IUser = Models.IUser;
    import IGroup = Models.IGroup;
    
    class UserController {
        public user: IUser;
        public allowedToUserGroups: IGroup[];
        public newGroupName: string;

        private allGroups: IGroup[];
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

            this.resourceService.getGroups( ( groups: IGroup[] ): void => {
                    self.allGroups = groups;
                },
                self.onError );
        }

        public getAllowedToUserGroups(): IGroup[] {
            if ( _.isNull( this.user ) || !_.isArray( this.allGroups ) ) {
                return <IGroup[]>{};
            }

            return _.filter( this.allGroups,
                ( group: IGroup ): boolean => {
                    return _.isUndefined( _.find( this.user.groups,
                        ( gr: IGroup ): boolean => {
                            return gr.id === group.id;
                        } ) );
                } );
        }
        
        public addGroup(group: IGroup) {
            this.user.groups.push( group );
        }

        public removeGroup( group: IGroup ) {
            _.remove( this.user.groups,
                ( gr: IGroup ): boolean => {
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

        public addNewGroup(name: string) {
            var self = this;
            this.resourceService.saveGroup( name,
                ( group: Models.IGroup ): void => {
                    self.allGroups.push( group );
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

    angular.module( 'app.pageControllers' ).controller( 'UserController', UserController );
}   