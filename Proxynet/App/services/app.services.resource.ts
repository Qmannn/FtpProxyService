import { Site } from './../Dto/SiteToSaveDto';
import { ISite } from './../Dto/SiteDto';
import { IGroup } from './../Dto/GroupDto';
import { IUser } from './../Dto/UserDto';

export interface IResourceService {
    getUsers(success: (users: IUser[]) => any, error: (data: any) => any): void;
    getUser(id: number, success: (users: IUser) => any, error: (data: any) => any): void;
    getGroups(success: (groups: IGroup[]) => any, error: (data: any) => any): void;

    getSites(success: (users: ISite[]) => any, error: (data: any) => any): void;
    getSite(id: number, success: (users: ISite) => any, error: (data: any) => any): void;
    getSiteGroups(success: (groups: IGroup[]) => any, error: (data: any) => any): void;

    saveUser(user: IUser, success: (user: IUser) => any, error: (data: any) => any): void;
    saveSite(site: ISite, success: (site: ISite) => any, error: (data: any) => any): void;
    saveGroup(name: string, success: (group: IGroup) => any, error: (data: any) => any): void;
    createSite(site: Site): ng.IPromise<Site>;

    updateUsers(success: () => any, error: (data: any) => any): void;
    updateSites(success: () => any, error: (data: any) => any): void;
}

export class ResourceService implements IResourceService {
    private actionHash: ng.resource.IActionHash;
    private actionHashArray: ng.resource.IActionHash;
    private resourceBaseUrl: string;
    private $resource: ng.resource.IResourceService;

    public static $inject: string[] = ['$resource'];

    constructor($resource: ng.resource.IResourceService) {
        this.$resource = $resource;

        let actionDescAtrray: ng.resource.IActionDescriptor = <ng.resource.IActionDescriptor>{};
        actionDescAtrray.isArray = true;
        actionDescAtrray.method = 'POST';
        let actionDesc: ng.resource.IActionDescriptor = <ng.resource.IActionDescriptor>{};
        actionDesc.isArray = false;
        actionDesc.method = 'POST';

        this.resourceBaseUrl = '/Proxynet/service/';

        this.actionHash = <ng.resource.IActionHash>{
            ['save']: actionDesc,
            ['query']: actionDesc
        };

        this.actionHashArray = <ng.resource.IActionHash>{
            ['save']: actionDescAtrray,
            ['query']: actionDescAtrray
        };
    }

    // GET

    public getUsers(success: (users: IUser[]) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'users')
            .query(success, error);
    }

    public getUser(id: number, success: (user: IUser) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'users/get-user').get({ 'userId': id })
            .$promise.then(success, error);
    }

    public getGroups(success: (groups: IGroup[]) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'users/get-groups', { method: 'GET' })
            .query(success, error);
    }

    public getSites(success: (users: ISite[]) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'sites', { method: 'GET' })
            .query(success, error);
    }

    public getSite(id: number, success: (site: ISite) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'sites/get-site', { 'siteId': id }, this.actionHash)
            .query(success, error);
    }

    public getSiteGroups(success: (groups: IGroup[]) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'sites/get-groups', null, this.actionHashArray)
            .query(success, error);
    }

    public updateUsers(success: () => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'users/update-users', null, this.actionHash)
            .query(success, error);
    }

    public updateSites(success: () => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'sites/updatesites', null, this.actionHash)
            .query(success, error);
    }

    // SAVE

    public saveUser(user: IUser, success: (user: IUser) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'users/save-user').save(user)
            .$promise.then(success, error);
    }

    public saveSite(site: ISite, success: (site: ISite) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'sites/save-site').save(site)
            .$promise.then(success, error);
    }

    public saveGroup(name: string, success: (group: IGroup) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'user/savegroup', { 'name': name }, this.actionHash)
            .query(success, error);
    }

    public createSite(site: Site): ng.IPromise<Site> {
        return this.$resource(this.resourceBaseUrl + 'sites/create-site').save(site).$promise;
    }
}