import { IResourceService } from './../services/app.services.resource';
import { IUser, IGroup } from './../models/user.model';

export class UserController {
    public user: IUser;
    public allowedToUserGroups: IGroup[];
    public newGroupName: string;

    private allGroups: IGroup[];
    private resourceService: IResourceService;

    public static $inject: string[] = ['$routeParams', 'app.services.resource'];

    constructor(routeParams: ng.route.IRouteParamsService,
        resourceService: IResourceService) {
        this.resourceService = resourceService;
        this.allGroups = null;
        this.user = null;

        var userId: number = routeParams['userid'];
        this.resourceService.getUser(userId,
            (user: IUser): void => {
                this.user = user;
            },
            this.onError);

        this.resourceService.getGroups((groups: IGroup[]): void => {
            this.allGroups = groups;
        },
            this.onError);
    }

    public getAllowedToUserGroups(): IGroup[] {
        if (_.isNull(this.user) || !_.isArray(this.allGroups)) {
            return <IGroup[]>{};
        }

        return _.filter(this.allGroups,
            (group: IGroup): boolean => {
                return _.isUndefined(_.find(this.user.groups,
                    (gr: IGroup): boolean => {
                        return gr.id === group.id;
                    }));
            });
    }

    public addGroup(group: IGroup): void {
        this.user.groups.push(group);
    }

    public removeGroup(group: IGroup): void {
        _.remove(this.user.groups,
            (gr: IGroup): boolean => {
                return gr.id === group.id;
            });
    }

    public saveUser(): void {
        this.resourceService.saveUser(this.user,
            (user: IUser): void => {
                this.user = user;
                this.onSaveSucefull();
            }, this.onError);
    }

    public addNewGroup(name: string): void {
        this.resourceService.saveGroup(name,
            (group: IGroup): void => {
                this.allGroups.push(group);
            }, this.onError);
        this.newGroupName = '';
    }

    /*--PRIVATE---*/

    private onError(data: any): void {
        console.error(data);
    }

    private onSaveSucefull(): void {

    }
}