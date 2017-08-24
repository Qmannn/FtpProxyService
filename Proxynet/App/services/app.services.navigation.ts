import { IPageInfo } from './app.services.navigation';
import { AppConfig } from '../AppConfig';

export interface INavigationService {
    setPage(page: Page): void;
    getCurrentPage(): Page;
    getAllPages(): Array<IPageInfo>;
}

export enum Page {
    UsersList,
    TestPage,
    SiteList,
    NotFound
}

export interface IPageInfo {
    name: string;
    url: string;
    page: Page;
    showInBar: boolean;
    isActive: boolean;
}

class PageInfo implements IPageInfo {
    public name: string;
    public url: string;
    public page: Page;
    public showInBar: boolean;
    public isActive: boolean;

    constructor(name: string, url: string, page: Page, showInBar: boolean = false, isActive: boolean = false) {
        this.name = name;
        this.url = url;
        this.page = page;
        this.showInBar = showInBar;
        this.isActive = false;
    }
}

class Pages {
    public pages: Array<IPageInfo>;

    constructor() {
        this.pages = new Array<IPageInfo>();

        this.pages.push(new PageInfo('Пользователи', AppConfig.AppBaseUrl() + 'users', Page.UsersList, true));
        this.pages.push(new PageInfo('Аккаунты сайтов', AppConfig.AppBaseUrl() + 'sites', Page.SiteList, true));
    }

    public getPageInfo(page: Page): IPageInfo {
        let foundedPage: IPageInfo = _.find<IPageInfo>(this.pages,
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

export class NavigationService implements INavigationService {
    public currentPage: Page;
    public pages: Pages;
    public location: ng.ILocationService;

    public static $inject: string[] = ['$location'];

    constructor($location: ng.ILocationService) {
        this.location = $location;
        this.pages = new Pages();
    }

    public setPage(page: Page): void {
        var pageInfo: IPageInfo = this.pages.getPageInfo(page);
        this.currentPage = pageInfo.page;
        this.location.path(pageInfo.url);
    }

    public getCurrentPage(): Page {
        var currentPuth: string = this.location.path();
        var currentPageInfo: IPageInfo = _.find(this.pages.pages,
            (pageInfo: IPageInfo): boolean => {
                return pageInfo.url === currentPuth;
            });
        if (_.isUndefined(currentPageInfo)) {
            return null;
        }
        return currentPageInfo.page;
    }

    public getAllPages(): IPageInfo[] {
        return this.pages.getPages();
    }
}