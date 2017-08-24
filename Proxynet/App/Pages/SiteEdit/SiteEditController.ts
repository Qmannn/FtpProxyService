import { ISite } from './../../Dto/SiteDto';
import { IGroup } from './../../Dto/GroupDto';
import { IResourceService } from '../../services/app.services.resource';

export class SiteController {
    public site: ISite;
    public newGroupName: string;

    private allGroups: IGroup[];
    private resourceService: IResourceService;

    public push: any;

    public static $inject: string[] = ['$routeParams', 'app.services.resource'];

    constructor(routeParams: ng.route.IRouteParamsService,
        resourceService: IResourceService) {
        this.resourceService = resourceService;
        this.allGroups = null;
        this.site = null;

        var siteId: number = routeParams['siteid'];
        this.resourceService.getSite(siteId,
            (site: ISite): void => {
                this.site = site;
            },
            this.onError);

        this.resourceService.getSiteGroups((groups: IGroup[]): void => {
            this.allGroups = groups;
        },
            this.onError);
    }

    public getAllowedToSiteGroups(): IGroup[] {
        if (_.isNull(this.site) || !_.isArray(this.allGroups)) {
            return <IGroup[]>{};
        }

        return _.filter(this.allGroups,
            (group: IGroup): boolean => {
                return _.isUndefined(_.find(this.site.groups,
                    (gr: IGroup): boolean => {
                        return gr.id === group.id;
                    }));
            });
    }

    public addGroup(group: IGroup): void {
        this.site.groups.push(group);
    }

    public removeGroup(group: IGroup): void {
        _.remove(this.site.groups,
            (gr: IGroup): boolean => {
                return gr.id === group.id;
            });
    }

    public saveSite(): void {
        this.resourceService.saveSite(this.site,
            (site: ISite): void => {
                this.site = site;
                this.onSaveSucefull();
            },
            this.onError);
    }

    public addNewGroup(name: string): void {
        this.resourceService.saveGroup(name,
            (group: IGroup): void => {
                this.allGroups.push(group);
            },
            this.onError);
        this.newGroupName = '';
    }

    /*--PRIVATE---*/

    private onError(data: any): void {
        console.error(data);
    }

    private onSaveSucefull(): void {

    }
}