module app.Services {
    'use strict';
    export interface INavigationService {
        setPage(page: Page): void;
        getCurrentPage(): Page;
        getAllPages(): Array<IPageInfo>;
    }

    export enum Page {
        UsersList,
        UserEdit,
        TestPage,
        NotFound
    }

    export interface IPageInfo {
        name: string;
        url: string;
        page: Page;
        showInBar: boolean;
    }

    class PageInfo implements IPageInfo {
        name: string;
        url: string;
        page: Page;
        showInBar: boolean;

        constructor( name: string, url: string, page: Page, showInBar: boolean = false) {
            this.name = name;
            this.url = url;
            this.page = page;
            this.showInBar = showInBar;
        }
    }

    class Pages {
        pages: Array<IPageInfo>;

        constructor() {
            this.pages = new Array<IPageInfo>();

            this.pages.push(new PageInfo('Пользователи', '/FtpProxy/users', Page.UsersList, true));
            this.pages.push(new PageInfo('Тестовая страница', '/FtpProxy/testpage', Page.TestPage, true));
        }

        public getPageInfo(page: Page) : IPageInfo {

            var foundedPage = _.find<IPageInfo>(this.pages,
                (pg): boolean => {
                    return pg.page === page;
                });

            if (!_.isUndefined(foundedPage)) {
                return foundedPage;
            } else {
                return _.find<IPageInfo>(this.pages,
                    (pg): boolean => {
                        return pg.page === Page.NotFound;
                    });
            }
        }

        public getPages(): Array<IPageInfo> {
            return _.clone(this.pages);
        }
    }

    class NavigationService implements INavigationService {
        currentPage: Page;
        pages: Pages;
        location    : ng.ILocationService;

        static $inject = ['$location'];
        constructor($location: ng.ILocationService) {
            this.location = $location;
            this.pages = new Pages();
        }

        setPage(page: Page): void {
            var pageInfo = this.pages.getPageInfo(page);
            this.currentPage = pageInfo.page;
            this.location.path(pageInfo.url);
        }

        getCurrentPage(): Page {
            return this.currentPage;
        }

        getAllPages(): IPageInfo[] {
            return this.pages.getPages();
        }
    }

    angular.module('app.services').service('app.services.navigation', NavigationService);
} 