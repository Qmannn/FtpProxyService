module app.Navbar {
    "use strict";

    interface INavbar {
        navigation: Services.INavigationService;

        getPages(): Services.IPageInfo[];
        setPage(page: Services.Page): void;
        isCurrentPage(page: Services.Page): boolean;
    }

    class Navbar implements INavbar {
        getPages(): Services.IPageInfo[] {
            var allPages = this.navigation.getAllPages();
            return _.filter<Services.IPageInfo>(allPages,
                (pg): boolean => {
                    return pg.showInBar;
                });
        }

        setPage(page: Services.Page) {
            this.navigation.setPage(page);
        }

        isCurrentPage(page: Services.Page): boolean {
            var currentPage = this.navigation.getCurrentPage();
            return currentPage === page;
        }

        navigation: Services.INavigationService;

        static $inject = ['app.services.navigation'];

        constructor(navigation: Services.INavigationService) {
            this.navigation = navigation;
        }


    }

    angular.module('app.navbar').controller('app.navbar.Controller', Navbar);
}