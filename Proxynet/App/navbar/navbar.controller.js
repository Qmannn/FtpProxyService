var app;
(function (app) {
    var Navbar;
    (function (Navbar_1) {
        "use strict";
        var Navbar = (function () {
            function Navbar(navigation) {
                this.navigation = navigation;
                this.currentPage = this.navigation.getCurrentPage();
            }
            Navbar.prototype.getPages = function () {
                var allPages = this.navigation.getAllPages();
                var currentPage = this.navigation.getCurrentPage();
                _.forEach(allPages, function (pageInfo) {
                    if (pageInfo.page === currentPage) {
                        pageInfo.isActive = true;
                    }
                    else {
                        pageInfo.isActive = false;
                    }
                });
                return _.filter(allPages, function (pg) {
                    return pg.showInBar;
                });
            };
            Navbar.prototype.setPage = function (page) {
                this.navigation.setPage(page);
                this.currentPage = page;
            };
            Navbar.prototype.isCurrentPage = function (page) {
                return this.currentPage == page;
            };
            return Navbar;
        }());
        Navbar.$inject = ['app.services.navigation'];
        angular.module('app.navbar').controller('app.navbar.Controller', Navbar);
    })(Navbar = app.Navbar || (app.Navbar = {}));
})(app || (app = {}));
//# sourceMappingURL=navbar.controller.js.map