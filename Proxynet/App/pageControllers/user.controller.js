var app;
(function (app) {
    var PageControllers;
    (function (PageControllers) {
        'use strict';
        var User = (function () {
            function User() {
            }
            return User;
        }());
        var UserGroup = (function () {
            function UserGroup() {
            }
            return UserGroup;
        }());
        var UserController = (function () {
            function UserController(nav, routeParams) {
                var user = new User();
                user.login = 'Max';
                user.name = 'Максим';
                console.log(routeParams);
                user.id = routeParams['userid'];
                user.groups = new Array();
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
            return UserController;
        }());
        UserController.$inject = ['app.services.navigation', '$routeParams'];
        angular.module('app.pageControllers').controller('UserController', UserController);
    })(PageControllers = app.PageControllers || (app.PageControllers = {}));
})(app || (app = {}));
//# sourceMappingURL=user.controller.js.map