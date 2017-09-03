import { AppConfig } from './../../AppConfig';
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
        this._scope.createUserLink = AppConfig.appName + '#/user';

        this._scope.getFilteredUsers = () => this.getFilteredUsers();
        this._scope.deleteUser = (user: IUser) => this.deleteUser(user);
    }

    private getUsers(): void {
        this._resourceService.getUsers((users: IUser[]): void => {
            this._scope.users = users;
            _.forEach(this._scope.users, (user: IUser): void => {
                user.editLink = AppConfig.appName + '#/user/' + user.id;
            });
        }, this.onError);
    }

    private getFilteredUsers(): IUser[] {
        return _.filter(this._scope.users,
            (user: IUser): boolean => {
                return user.name.toUpperCase().indexOf(this._scope.filter.name.toUpperCase()) >= 0 ||
                    this._scope.filter.name === '';
            });
    }

    private deleteUser(user: IUser): void {
        this._resourceService.deleteUser(user.id).then(() => {
            _.remove(this._scope.users, (item: IUser) => item.id === user.id);
        });
    }
}
