module app.PageControllers {
    'use strict';
    
    import IUser = Models.IUser;

    class UsersController {
        public users: IUser[];

        private resourceService: Services.IResourceService;

        static $inject = ['app.services.navigation', 'app.services.resource'];

        constructor(nav: Services.INavigationService, resourceService: Services.IResourceService) {
            this.resourceService = resourceService;

            var self = this;

            resourceService.getUsers((users: IUser[]): void => {
                    self.users = users;
                },
                (data: any): void => { console.log(data); });
        }

        editUser(user: IUser) {
            console.log(user);
        }
    }

    angular.module('app.pageControllers').controller('UsersController', UsersController);
}   