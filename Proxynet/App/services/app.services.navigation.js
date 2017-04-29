var app;
(function (app) {
    var Services;
    (function (Services) {
        'use strict';
        var Page;
        (function (Page) {
            Page[Page["UsersList"] = 0] = "UsersList";
            Page[Page["TestPage"] = 1] = "TestPage";
            Page[Page["SiteList"] = 2] = "SiteList";
            Page[Page["NotFound"] = 3] = "NotFound";
        })(Page = Services.Page || (Services.Page = {}));
        var PageInfo = (function () {
            function PageInfo(name, url, page, showInBar, isActive) {
                if (showInBar === void 0) { showInBar = false; }
                if (isActive === void 0) { isActive = false; }
                this.name = name;
                this.url = url;
                this.page = page;
                this.showInBar = showInBar;
                this.isActive = false;
            }
            return PageInfo;
        }());
        var Pages = (function () {
            function Pages() {
                this.pages = new Array();
                this.pages.push(new PageInfo('Пользователи', '/FtpProxy/users', Page.UsersList, true));
                this.pages.push(new PageInfo('Аккаунты сайтов', '/FtpProxy/sites', Page.SiteList, true));
            }
            Pages.prototype.getPageInfo = function (page) {
                var foundedPage = _.find(this.pages, function (pg) {
                    return pg.page === page;
                });
                if (!_.isUndefined(foundedPage)) {
                    return foundedPage;
                }
                else {
                    return _.find(this.pages, function (pg) {
                        return pg.page === Page.NotFound;
                    });
                }
            };
            Pages.prototype.getPages = function () {
                return _.clone(this.pages);
            };
            return Pages;
        }());
        var NavigationService = (function () {
            function NavigationService($location) {
                this.location = $location;
                this.pages = new Pages();
            }
            NavigationService.prototype.setPage = function (page) {
                var pageInfo = this.pages.getPageInfo(page);
                this.currentPage = pageInfo.page;
                this.location.path(pageInfo.url);
            };
            NavigationService.prototype.getCurrentPage = function () {
                var currentPuth = this.location.path();
                var currentPageInfo = _.find(this.pages.pages, function (pageInfo) {
                    return pageInfo.url === currentPuth;
                });
                if (_.isUndefined(currentPageInfo)) {
                    return null;
                }
                return currentPageInfo.page;
            };
            NavigationService.prototype.getAllPages = function () {
                return this.pages.getPages();
                ;
            };
            return NavigationService;
        }());
        NavigationService.$inject = ['$location'];
        angular.module('app.services').service('app.services.navigation', NavigationService);
    })(Services = app.Services || (app.Services = {}));
})(app || (app = {}));
//# sourceMappingURL=app.services.navigation.js.map