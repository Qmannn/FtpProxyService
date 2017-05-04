var app;
(function (app) {
    var Services;
    (function (Services) {
        'use strict';
        var ResourceService = (function () {
            function ResourceService($resource) {
                this.$resource = $resource;
                var actionDescAtrray = {};
                actionDescAtrray.isArray = true;
                actionDescAtrray.method = 'POST';
                var actionDesc = {};
                actionDesc.isArray = false;
                actionDesc.method = 'POST';
                this.actionHash = (_a = {},
                    _a['save'] = actionDesc,
                    _a['query'] = actionDesc,
                    _a);
                this.actionHashArray = (_b = {},
                    _b['save'] = actionDescAtrray,
                    _b['query'] = actionDescAtrray,
                    _b);
                var _a, _b;
            }
            // GET
            ResourceService.prototype.getUsers = function (success, error) {
                this.$resource('user/getusers', null, this.actionHashArray)
                    .query(success, error);
            };
            ResourceService.prototype.getUser = function (id, success, error) {
                this.$resource('user/getuser', { 'userId': id }, this.actionHash)
                    .query(success, error);
            };
            ResourceService.prototype.getGroups = function (success, error) {
                this.$resource('user/getgroups', null, this.actionHashArray)
                    .query(success, error);
            };
            ResourceService.prototype.getSites = function (success, error) {
                this.$resource('site/getsites', null, this.actionHashArray)
                    .query(success, error);
            };
            ResourceService.prototype.getSite = function (id, success, error) {
                this.$resource('site/getsite', { 'siteId': id }, this.actionHash)
                    .query(success, error);
            };
            ResourceService.prototype.getSiteGroups = function (success, error) {
                this.$resource('site/getgroups', null, this.actionHashArray)
                    .query(success, error);
            };
            ResourceService.prototype.updateUsers = function (success, error) {
                this.$resource('user/updateusers', null, this.actionHash)
                    .query(success, error);
            };
            ResourceService.prototype.updateSites = function (success, error) {
                this.$resource('site/updatesites', null, this.actionHash)
                    .query(success, error);
            };
            // SAVE
            ResourceService.prototype.saveUser = function (user, success, error) {
                this.$resource('user/saveuser', { 'users': user }, this.actionHash)
                    .query(success, error);
            };
            ResourceService.prototype.saveSite = function (site, success, error) {
                this.$resource('site/savesite', { 'site': site }, this.actionHash)
                    .query(success, error);
            };
            ResourceService.prototype.saveGroup = function (name, success, error) {
                this.$resource('user/savegroup', { 'name': name }, this.actionHash)
                    .query(success, error);
            };
            return ResourceService;
        }());
        ResourceService.$inject = ['$resource'];
        angular.module('app.services').service('app.services.resource', ResourceService);
    })(Services = app.Services || (app.Services = {}));
})(app || (app = {}));
//# sourceMappingURL=app.services.resource.js.map