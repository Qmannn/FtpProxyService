import { ISite } from './../models/site.model';
import { IUser, IGroup } from './../models/user.model';

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

        this.resourceBaseUrl = '/';

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
        this.$resource(this.resourceBaseUrl + 'user/getusers', null, this.actionHashArray)
            .query(success, error);
    }

    public getUser(id: number, success: (user: IUser) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'user/getuser', { 'userId': id }, this.actionHash)
            .query(success, error);
    }

    public getGroups(success: (groups: IGroup[]) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'user/getgroups', null, this.actionHashArray)
            .query(success, error);
    }

    public getSites(success: (users: ISite[]) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'site/getsites', null, this.actionHashArray)
            .query(success, error);
    }

    public getSite(id: number, success: (site: ISite) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'site/getsite', { 'siteId': id }, this.actionHash)
            .query(success, error);
    }

    public getSiteGroups(success: (groups: IGroup[]) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'site/getgroups', null, this.actionHashArray)
            .query(success, error);
    }

    public updateUsers(success: () => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'user/updateusers', null, this.actionHash)
            .query(success, error);
    }

    public updateSites(success: () => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'site/updatesites', null, this.actionHash)
            .query(success, error);
    }

    // SAVE

    public saveUser(user: IUser, success: (user: IUser) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'user/saveuser', { 'users': user }, this.actionHash)
            .query(success, error);
    }

    public saveSite(site: ISite, success: (site: ISite) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'site/savesite', { 'site': site }, this.actionHash)
            .query(success, error);
    }

    public saveGroup(name: string, success: (group: IGroup) => any, error: (data: any) => any): void {
        this.$resource(this.resourceBaseUrl + 'user/savegroup', { 'name': name }, this.actionHash)
            .query(success, error);
    }
}