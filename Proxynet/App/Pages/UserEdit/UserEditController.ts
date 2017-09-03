import { NotificationService } from './../../Core/Services/Notifications/NotificationsService';
import { IUserEditScope } from './IUserEditScope';
import { PageControllerBase } from './../PageControllerBase';
import { IResourceService } from './../../services/app.services.resource';
import { IGroup } from './../../Dto/GroupDto';
import { IUser } from './../../Dto/UserDto';
import { User } from '../../Dto/User';
import { NotificationType } from '../../Core/Services/Notifications/NotificationType';

export class UserController extends PageControllerBase {
    private _resourceService: IResourceService;
    private _scope: IUserEditScope;
    private _routeParams: ng.route.IRouteParamsService;
    private _notificationService: NotificationService;

    public static $inject: string[] = ['$routeParams', 'app.services.resource', '$scope', 'NotificationService'];

    constructor(routeParams: ng.route.IRouteParamsService,
        resourceService: IResourceService, $scope: IUserEditScope, notificationService: NotificationService) {
        super();
        this._resourceService = resourceService;
        this._scope = $scope;
        this._routeParams = routeParams;
        this._notificationService = notificationService;

        this.initScope();
    }

    private initScope(): void {
        this.loadData();

        this._scope.getAllowedToUserGroups = () => this.getAllowedToUserGroups();
        this._scope.saveUser = () => this.saveUser();
        this._scope.addGroup = (group: IGroup) => this.addGroup(group);
        this._scope.removeGroup = (group: IGroup) => this.removeGroup(group);
        this._scope.addNewGroup = (groupName: string) => this.addNewGroup(groupName);
    }

    private loadData(): void {
        var userId: number = this._routeParams['userid'];

        if (!_.isUndefined(userId)) {
            this._resourceService.getUser(userId,
                (user: IUser): void => {
                    this._scope.user = user;
                },
                this.onError);
        } else {
            this._scope.user = new User();
            this._scope.isNewUser = true;
        }

        this._resourceService.getGroups((groups: IGroup[]): void => {
            this._scope.allGroups = groups;
        },
            this.onError);
    }

    private getAllowedToUserGroups(): IGroup[] {
        if (_.isUndefined(this._scope.user) || !_.isArray(this._scope.allGroups)) {
            return <IGroup[]>{};
        }

        return _.filter(this._scope.allGroups,
            (group: IGroup): boolean => {
                return _.isUndefined(_.find(this._scope.user.groups,
                    (gr: IGroup): boolean => {
                        return gr.id === group.id;
                    }));
            });
    }

    private addGroup(group: IGroup): void {
        this._scope.user.groups.push(group);
    }

    private removeGroup(group: IGroup): void {
        _.remove(this._scope.user.groups,
            (gr: IGroup): boolean => {
                return gr.id === group.id;
            });
    }

    private saveUser(): void {
        if (this._scope.user.account.needSaveAccount) {
            this.validateUser(this._scope.user).then((checkSuccess: boolean): void => {
                if (checkSuccess) {
                    this.saveCurrentUser();
                } else {
                    this._notificationService
                        .showNotification('Пользователь с таким логином уже существует', NotificationType.Error);
                }
            });
        } else {
            this.saveCurrentUser();
        }
    }

    private saveCurrentUser(): void {
        this._resourceService.saveUser(this._scope.user,
            (user: IUser): void => {
                this._scope.user = user;
            }, this.onError);
    }

    private validateUser(user: IUser): ng.IPromise<boolean> {
        return this._resourceService.checkUserLogin(user.account.login, user.id);
    }

    private addNewGroup(name: string): void {
        this._resourceService.saveGroup(name,
            (group: IGroup): void => {
                this._scope.allGroups.push(group);
            }, this.onError);
        this._scope.newGroupName = '';
    }
}