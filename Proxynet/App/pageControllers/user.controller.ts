module app.PageControllers {
    'use strict';

    interface IUserGroup {
        id: number;
        name: string;
    }

    interface IUser {
        name: string;
        login: string;
        id: number;
        groups: IUserGroup[];
    }

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

    class UserController {
        public user: IUser;

        static $inject = ['app.services.navigation', '$routeParams'];
        constructor(nav: app.Services.INavigationService, routeParams: ng.route.IRouteParamsService) {

            var user = new User();
            user.login = 'Max';
            user.name = 'Максим';
            console.log(routeParams);
            user.id = routeParams['userid'];

            user.groups = new Array<UserGroup>();
            var group = new UserGroup();
            group.name = 'Admin';
            user.groups.push(group);

            group = new UserGroup();
            group.name = 'Loshara';
            user.groups.push(group);

            group = new UserGroup();
            group.name = 'LolGroup';
            user.groups.push(group);

            this.user = user;
        }
    }

    angular.module('app.pageControllers').controller('UserController', UserController);
}   