import { IPageInfo, Page, INavigationService } from '../../services/app.services.navigation';

export class Navbar {
    private currentPage: Page;

    public getPages(): IPageInfo[] {
        let allPages: IPageInfo[] = this.navigation.getAllPages();
        let currentPage: Page = this.navigation.getCurrentPage();
        _.forEach(allPages,
            (pageInfo: IPageInfo): void => {
                if (pageInfo.page === currentPage) {
                    pageInfo.isActive = true;
                } else {
                    pageInfo.isActive = false;
                }
            });

        return _.filter<IPageInfo>(allPages,
            (pg): boolean => {
                return pg.showInBar;
            });
    }

    public setPage(page: Page): void {
        this.navigation.setPage(page);
        this.currentPage = page;
    }

    public isCurrentPage(page: Page): boolean {
        return this.currentPage === page;
    }

    private navigation: INavigationService;

    public static $inject: string[] = ['app.services.navigation'];

    constructor(navigation: INavigationService) {
        this.navigation = navigation;
        this.currentPage = this.navigation.getCurrentPage();
    }
}