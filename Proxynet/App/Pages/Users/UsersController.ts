import { Filter } from './Filter';
import { IUsersScope } from './IUsersScope';
import { PageControllerBase } from './../PageControllerBase';
import { IResourceService } from './../../services/app.services.resource';
import { IUser } from './../../Dto/UserDto';

export class UsersController extends PageControllerBase {
    private _resourceService: IResourceService;
    private _scope: IUsersScope;

    public static $inject: string[] = ['app.services.resource', '$scope'];

    constructor(resourceService: IResourceService, $scope: IUsersScope) {
        super();

        this._resourceService = resourceService;
        this._scope = $scope;

        this.initScope();
    }

    private initScope(): void {
        this._scope.filter = new Filter();
        this.getUsers();

        this._scope.getFilteredUsers = () => this.getFilteredUsers();
    }

    private getUsers(): void {
        this._resourceService.getUsers((users: IUser[]): void => {
            this._scope.users = users;
        }, this.onError);
    }

    private getFilteredUsers(): IUser[] {
        return _.filter(this._scope.users,
            (user: IUser): boolean => {
                return user.login.toUpperCase().indexOf(this._scope.filter.login.toUpperCase()) >= 0 ||
                    this._scope.filter.login === '';
            });
    }
}
