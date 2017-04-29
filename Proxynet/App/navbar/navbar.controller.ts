module app.Navbar {
    import IPageInfo = app.Services.IPageInfo;
    "use strict";

    interface INavbar {
        navigation: Services.INavigationService;

        getPages(): Services.IPageInfo[];
        setPage(page: Services.Page): void;
        isCurrentPage(page: Services.Page): boolean;
    }

    class Navbar implements INavbar {
        currentPage: Services.Page;

        getPages(): Services.IPageInfo[] {
            var allPages = this.navigation.getAllPages();
            var currentPage = this.navigation.getCurrentPage();
            _.forEach( allPages,
                ( pageInfo: IPageInfo ): void => {
                    if ( pageInfo.page === currentPage ) {
                        pageInfo.isActive = true;
                    } else {
                        pageInfo.isActive = false;
                    }
                } );

            return _.filter<Services.IPageInfo>(allPages,
                (pg): boolean => {
                    return pg.showInBar;
                });
        }

        setPage(page: Services.Page) {
            this.navigation.setPage(page);
            this.currentPage = page;
        }

        isCurrentPage( page: Services.Page ): boolean {
            return this.currentPage == page;
        }

        navigation: Services.INavigationService;

        static $inject = ['app.services.navigation'];

        constructor(navigation: Services.INavigationService) {
            this.navigation = navigation;
            this.currentPage = this.navigation.getCurrentPage();
        }


    }

    angular.module('app.navbar').controller('app.navbar.Controller', Navbar);
}