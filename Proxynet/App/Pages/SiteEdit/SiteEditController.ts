import { NotificationService } from './../../Core/Services/Notifications/NotificationsService';
import { ISiteEditScope } from './ISiteEditScope';
import { Site } from './../../Dto/Site';
import { ISite } from './../../Dto/ISite';
import { IGroup } from './../../Dto/GroupDto';
import { IResourceService } from '../../services/app.services.resource';
import { NotificationType } from '../../Core/Services/Notifications/NotificationType';

export class SiteController {
    private resourceService: IResourceService;
    private _scope: ISiteEditScope;
    private _routeParams: ng.route.IRouteParamsService;
    private _notificationService: NotificationService;

    public static $inject: string[] = ['$routeParams', 'app.services.resource', '$scope', 'NotificationService'];

    constructor(routeParams: ng.route.IRouteParamsService,
        resourceService: IResourceService,
        $scope: ISiteEditScope,
        notificationService: NotificationService) {
        this.resourceService = resourceService;
        this._scope = $scope;
        this._routeParams = routeParams;
        this._notificationService = notificationService;

        this.initScope();
    }

    private initScope(): void {
        this.loadData();

        this._scope.getAllowedToSiteGroups = () => this.getAllowedToSiteGroups();
        this._scope.saveSite = () => this.saveSite();
        this._scope.addGroup = (group: IGroup) => this.addGroup(group);
        this._scope.addNewGroup = (name: string) => this.addNewGroup(name);
        this._scope.removeGroup = (group: IGroup) => this.removeGroup(group);
        this._scope.deleteGroup = (group: IGroup) => this.deleteGroup(group);

    }

    private loadData(): void {
        let siteId: number = this._routeParams['siteid'];
        if (!_.isUndefined(siteId)) {
            this.resourceService.getSite(siteId,
                (site: ISite): void => {
                    this._scope.site = site;
                },
                this.onError);
        } else {
            this._scope.site = new Site();
            this._scope.isNewSite = true;
        }

        this.resourceService.getGroups((groups: IGroup[]): void => {
            this._scope.allGroups = groups;
        },
            this.onError);
    }

    private getAllowedToSiteGroups(): IGroup[] {
        if (_.isUndefined(this._scope.site) || !_.isArray(this._scope.allGroups)) {
            return <IGroup[]>{};
        }

        return _.filter(this._scope.allGroups,
            (group: IGroup): boolean => {
                return _.isUndefined(_.find(this._scope.site.groups,
                    (gr: IGroup): boolean => {
                        return gr.id === group.id;
                    }));
            });
    }

    private addGroup(group: IGroup): void {
        this._scope.site.groups.push(group);
    }

    private removeGroup(group: IGroup): void {
        _.remove(this._scope.site.groups,
            (gr: IGroup): boolean => {
                return gr.id === group.id;
            });
    }

    private saveSite(): void {
        if (!this._scope.siteForm.$valid) {
            return;
        }
        this.validateSite(this._scope.site).then((checkSuccess: boolean): void => {
                if (checkSuccess) {
                    this.saveCurrentSite();
                } else {
                    this._notificationService
                        .showNotification('Сайт с таким названием уже существует', NotificationType.Error);
                }
        });
    }

    private validateSite(site: ISite): ng.IPromise<boolean> {
        return this.resourceService.checkSiteName(site.name, site.id);
    }

    private saveCurrentSite(): void {
        this.resourceService.saveSite(this._scope.site,
            (site: ISite): void => {
                this._scope.site = site;
                this.onSaveSucefull();
                this._scope.isNewSite = false;
            },
            this.onError);
    }

    private addNewGroup(name: string): void {
        this.resourceService.saveGroup(name,
            (group: IGroup): void => {
                this._scope.allGroups.push(group);
            },
            this.onError);
        this._scope.newGroupName = '';
    }

    private deleteGroup(group: IGroup): void {
        this.resourceService.deleteGroup(group.id)
            .then(() => {
                _.remove(this._scope.allGroups, (item: IGroup) => item.id === group.id );
            });
    }

    private onError(data: any): void {
        console.error(data);
    }

    private onSaveSucefull(): void {

    }
}