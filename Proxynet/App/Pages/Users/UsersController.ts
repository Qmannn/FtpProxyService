import { INavigationService } from './../../services/app.services.navigation';
import { IResourceService } from './../../services/app.services.resource';
import { IUser } from './../../Dto/UserDto';

'use strict';
class Filter {
    public login: string;
    public name: string;

    constructor() {
        this.login = '';
        this.name = '';
    }
}

export class UsersController {
    public users: IUser[];

    public filter: Filter;

    private resourceService: IResourceService;

    public static $inject: string[] = ['app.services.navigation', 'app.services.resource'];

    constructor(nav: INavigationService, resourceService: IResourceService) {
        this.resourceService = resourceService;
        this.filter = new Filter();
        this.getUsers();
    }

    public updateUsers(): void {
        this.resourceService.updateUsers((): void => {
            this.getUsers();
        }, () => undefined);
    }

    private getUsers(): void {
        this.resourceService.getUsers((users: IUser[]): void => {
            this.users = users;
        }, () => undefined);
    }

    public getFilteredUsers(): IUser[] {
        return _.filter(this.users,
            (user: IUser): boolean => {
                return user.login.toUpperCase().indexOf(this.filter.login.toUpperCase()) >= 0 ||
                    this.filter.login === '';
            });
    }
}
