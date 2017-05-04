var app;
(function (app) {
    var PageControllers;
    (function (PageControllers) {
        'use strict';
        var SitesController = (function () {
            function SitesController(resourceService) {
                this.resourceService = resourceService;
                this.getSites();
            }
            SitesController.prototype.updateSites = function () {
                var self = this;
                this.resourceService.updateSites(function () {
                    self.getSites();
                }, function (data) {
                    console.log(data);
                });
            };
            SitesController.prototype.getSites = function () {
                var self = this;
                self.resourceService.getSites(function (sites) {
                    self.sites = sites;
                }, function (data) { console.log(data); });
            };
            return SitesController;
        }());
        SitesController.$inject = ['app.services.resource'];
        angular.module('app.pageControllers').controller('SitesController', SitesController);
    })(PageControllers = app.PageControllers || (app.PageControllers = {}));
})(app || (app = {}));
//# sourceMappingURL=sites.controller.js.map