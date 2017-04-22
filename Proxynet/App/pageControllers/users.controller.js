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
        var UsersController = (function () {
            function UsersController(nav, resourceService) {
                this.resourceService = resourceService;
                var self = this;
                resourceService.getUsers(function (users) {
                    self.users = _.map(users, function (user) {
                        var userDto = {};
                        userDto.id = user.Id;
                        userDto.login = user.Login;
                        userDto.name = user.Name;
                        return userDto;
                    });
                }, function (data) { console.log(data); });
            }
            UsersController.prototype.editUser = function (user) {
                console.log(user);
            };
            return UsersController;
        }());
        UsersController.$inject = ['app.services.navigation', 'app.services.resource'];
        angular.module('app.pageControllers').controller('UsersController', UsersController);
    })(PageControllers = app.PageControllers || (app.PageControllers = {}));
})(app || (app = {}));
//# sourceMappingURL=users.controller.js.map