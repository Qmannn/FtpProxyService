var app;
(function (app) {
    var PageControllers;
    (function (PageControllers) {
        'use strict';
        var Filter = (function () {
            function Filter() {
                this.login = "";
                this.name = "";
            }
            return Filter;
        }());
        var UsersController = (function () {
            function UsersController(nav, resourceService) {
                this.resourceService = resourceService;
                this.filter = new Filter();
                this.getUsers();
            }
            UsersController.prototype.updateUsers = function () {
                var self = this;
                this.resourceService.updateUsers(function () {
                    self.getUsers();
                }, function (data) {
                    console.log(data);
                });
            };
            UsersController.prototype.getUsers = function () {
                var self = this;
                this.resourceService.getUsers(function (users) {
                    self.users = users;
                }, function (data) { console.log(data); });
            };
            UsersController.prototype.getFilteredUsers = function () {
                var _this = this;
                return _.filter(this.users, function (user) {
                    return user.login.toUpperCase().indexOf(_this.filter.login.toUpperCase()) >= 0 ||
                        _this.filter.login === "";
                });
            };
            return UsersController;
        }());
        UsersController.$inject = ['app.services.navigation', 'app.services.resource'];
        angular.module('app.pageControllers').controller('UsersController', UsersController);
    })(PageControllers = app.PageControllers || (app.PageControllers = {}));
})(app || (app = {}));
//# sourceMappingURL=users.controller.js.map