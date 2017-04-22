module app.PageControllers {
    'use strict';

    import IUserGroup = Models.IUserGroup;
    import IUser = Models.IUser;

    class User implements IUser {
        name: string;
        login: string;
        id: number;
        groups: IUserGroup[];
    }

    class UserGroup implements IUserGroup {
        id: number;
        name: string;
    }

    class UsersController {
        public users: IUser[];

        private resourceService: Services.IResourceService;

        static $inject = ['app.services.navigation', 'app.services.resource'];

        constructor(nav: app.Services.INavigationService, resourceService: Services.IResourceService) {
            this.resourceService = resourceService;

            var self = this;

            resourceService.getUsers((users: IUser[]): void => {
                    self.users = _.map(users,
                        (user: any): IUser => {
                            var userDto = <IUser>{};
                            userDto.id = user.Id;
                            userDto.login = user.Login;
                            userDto.name = user.Name;
                            return userDto;
                        });
                },
                (data: any): void => { console.log(data); });
        }

        editUser(user: IUser) {
            console.log(user);
        }
    }

    angular.module('app.pageControllers').controller('UsersController', UsersController);
}   