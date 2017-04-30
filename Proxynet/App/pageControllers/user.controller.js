var app;
(function (app) {
    var PageControllers;
    (function (PageControllers) {
        'use strict';
        var UserController = (function () {
            function UserController(routeParams, resourceService) {
                this.resourceService = resourceService;
                this.allGroups = null;
                this.user = null;
                var self = this;
                var userId = routeParams['userid'];
                this.resourceService.getUser(userId, function (user) {
                    self.user = user;
                }, self.onError);
                this.resourceService.getGroups(function (groups) {
                    self.allGroups = groups;
                }, self.onError);
            }
            UserController.prototype.getAllowedToUserGroups = function () {
                var _this = this;
                if (_.isNull(this.user) || !_.isArray(this.allGroups)) {
                    return {};
                }
                return _.filter(this.allGroups, function (group) {
                    return _.isUndefined(_.find(_this.user.groups, function (gr) {
                        return gr.id === group.id;
                    }));
                });
            };
            UserController.prototype.addGroup = function (group) {
                this.user.groups.push(group);
            };
            UserController.prototype.removeGroup = function (group) {
                _.remove(this.user.groups, function (gr) {
                    return gr.id === group.id;
                });
            };
            UserController.prototype.saveUser = function () {
                var self = this;
                this.resourceService.saveUser(this.user, function (user) {
                    self.user = user;
                    self.onSaveSucefull();
                }, self.onError);
            };
            UserController.prototype.addNewGroup = function (name) {
                var self = this;
                this.resourceService.saveGroup(name, function (group) {
                    self.allGroups.push(group);
                }, self.onError);
                this.newGroupName = "";
            };
            /*--PRIVATE---*/
            UserController.prototype.onError = function (data) {
                console.error(data);
            };
            UserController.prototype.onSaveSucefull = function () {
            };
            return UserController;
        }());
        UserController.$inject = ['$routeParams', 'app.services.resource'];
        angular.module('app.pageControllers').controller('UserController', UserController);
    })(PageControllers = app.PageControllers || (app.PageControllers = {}));
})(app || (app = {}));
//# sourceMappingURL=user.controller.js.map