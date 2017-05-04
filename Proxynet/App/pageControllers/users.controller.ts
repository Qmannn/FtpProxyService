module app.PageControllers {
    'use strict';
    
    import IUser = Models.IUser;

    class Filter {
        login: string;
        name: string;

        constructor() {
            this.login = "";
            this.name = "";
        }
    }

    class UsersController {
        public users: IUser[];

        public filter: Filter;

        private resourceService: Services.IResourceService;

        static $inject = ['app.services.navigation', 'app.services.resource'];

        constructor(nav: Services.INavigationService, resourceService: Services.IResourceService) {
            this.resourceService = resourceService;
            this.filter = new Filter();
            this.getUsers();
        }

        public updateUsers() {
            var self = this;
            this.resourceService.updateUsers( (): void => {
                    self.getUsers();
                },
                ( data: any ): void => {
                    console.log( data );
                } );
        }

        private getUsers() {

            var self = this;
            this.resourceService.getUsers( ( users: IUser[] ): void => {
                    self.users = users;
                },
                ( data: any ): void => { console.log( data ); } );
        }

        public getFilteredUsers() : Models.IUser[] {
            return _.filter( this.users,
                ( user: IUser ): boolean => {
                    return user.login.toUpperCase().indexOf( this.filter.login.toUpperCase() ) >= 0 ||
                        this.filter.login === "";
                } );
        }
    }

    angular.module('app.pageControllers').controller('UsersController', UsersController);
}   