var app;
(function (app) {
    var PageControllers;
    (function (PageControllers) {
        'use strict';
        var SiteController = (function () {
            function SiteController(routeParams, resourceService) {
                this.resourceService = resourceService;
                this.allGroups = null;
                this.site = null;
                var self = this;
                var siteId = routeParams['siteid'];
                this.resourceService.getSite(siteId, function (site) {
                    self.site = site;
                }, self.onError);
                this.resourceService.getSiteGroups(function (groups) {
                    self.allGroups = groups;
                }, self.onError);
            }
            SiteController.prototype.getAllowedToSiteGroups = function () {
                var _this = this;
                if (_.isNull(this.site) || !_.isArray(this.allGroups)) {
                    return {};
                }
                return _.filter(this.allGroups, function (group) {
                    return _.isUndefined(_.find(_this.site.groups, function (gr) {
                        return gr.id === group.id;
                    }));
                });
            };
            SiteController.prototype.addGroup = function (group) {
                this.site.groups.push(group);
            };
            SiteController.prototype.removeGroup = function (group) {
                _.remove(this.site.groups, function (gr) {
                    return gr.id === group.id;
                });
            };
            SiteController.prototype.saveSite = function () {
                var self = this;
                this.resourceService.saveSite(this.site, function (site) {
                    self.site = site;
                    self.onSaveSucefull();
                }, self.onError);
            };
            /*--PRIVATE---*/
            SiteController.prototype.onError = function (data) {
                console.error(data);
            };
            SiteController.prototype.onSaveSucefull = function () {
            };
            return SiteController;
        }());
        SiteController.$inject = ['$routeParams', 'app.services.resource'];
        angular.module('app.pageControllers').controller('SiteController', SiteController);
    })(PageControllers = app.PageControllers || (app.PageControllers = {}));
})(app || (app = {}));
//# sourceMappingURL=site.controller.js.map