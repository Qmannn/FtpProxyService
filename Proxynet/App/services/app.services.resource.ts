import { AppConfig } from './../AppConfig';
import { HttpResourceService } from './../Core/Services/Resource/HttpResourceService';
import { ISite } from './../Dto/ISite';
import { IGroup } from './../Dto/GroupDto';
import { IUser } from './../Dto/UserDto';

export interface IResourceService {
    getUsers(success: (users: IUser[]) => any, error: (data: any) => any): void;
    getUser(id: number, success: (users: IUser) => any, error: (data: any) => any): void;
    getGroups(success: (groups: IGroup[]) => any, error: (data: any) => any): void;
    checkUserLogin(login: string, userId: number): ng.IPromise<boolean>;
    checkSiteName(siteName: string, siteId: number): ng.IPromise<boolean>;

    getSites(success: (users: ISite[]) => any, error: (data: any) => any): void;
    getSite(id: number, success: (users: ISite) => any, error: (data: any) => any): void;

    saveUser(user: IUser, success: (user: IUser) => any, error: (data: any) => any): void;
    saveSite(site: ISite, success: (site: ISite) => any, error: (data: any) => any): void;
    saveGroup(name: string, success: (group: IGroup) => any, error: (data: any) => any): void;

    deleteSite(siteId: number): ng.IPromise<void>;
    deleteUser(userId: number): ng.IPromise<void>;
    deleteGroup(groupId: number): ng.IPromise<void>;
}

export class ResourceService implements IResourceService {
    private resourceBaseUrl: string;
    private _resourceService: HttpResourceService;

    public static $inject: string[] = ['HttpResourceService'];

    constructor(resourceService: HttpResourceService) {
        this._resourceService = resourceService;
        this.resourceBaseUrl = AppConfig.resourceBaseUrl;
    }

    // GET

    public getUsers(success: (users: IUser[]) => any, error: (data: any) => any): void {
        this._resourceService.get(this.resourceBaseUrl + 'users')
            .then(success, error);
    }

    public getUser(id: number, success: (user: IUser) => any, error: (data: any) => any): void {
        this._resourceService.get(this.resourceBaseUrl + 'users/get-user', { params: { 'userId': id } })
            .then(success, error);
    }

    public getGroups(success: (groups: IGroup[]) => any, error: (data: any) => any): void {
        this._resourceService.get(this.resourceBaseUrl + 'groups/get-groups')
            .then(success, error);
    }

    public getSites(success: (users: ISite[]) => any, error: (data: any) => any): void {
        this._resourceService.get(this.resourceBaseUrl + 'sites')
            .then(success, error);
    }

    public getSite(id: number, success: (site: ISite) => any, error: (data: any) => any): void {
        this._resourceService.get(this.resourceBaseUrl + 'sites/get-site', { params: { 'siteId': id } })
            .then(success, error);
    }

    public checkUserLogin(login: string, userId: number): ng.IPromise<boolean> {
        return this._resourceService
            .get<boolean>(this.resourceBaseUrl + 'users/check-user-name', { params: { 'login': login, 'userId': userId } });
    }

    public checkSiteName(siteName: string, siteId: number): ng.IPromise<boolean> {
        return this._resourceService
            .get<boolean>(this.resourceBaseUrl + 'sites/check-site-name', { params: { 'siteName': siteName, 'siteId': siteId } });
    }

    // PUT

    public saveUser(user: IUser, success: (user: IUser) => any, error: (data: any) => any): void {
        this._resourceService.put(this.resourceBaseUrl + 'users/save-user', user)
            .then(success, error);
    }

    public saveSite(site: ISite, success: (site: ISite) => any, error: (data: any) => any): void {
        this._resourceService.put(this.resourceBaseUrl + 'sites/save-site', site)
            .then(success, error);
    }

    public saveGroup(name: string, success: (group: IGroup) => any, error: (data: any) => any): void {
        this._resourceService.put(this.resourceBaseUrl + 'groups/save-group', { name: name })
            .then(success, error);
    }

    // DELETE

    public deleteSite(siteId: number): ng.IPromise<void> {
        return this._resourceService
            .delete(this.resourceBaseUrl + 'sites/delete-site', { params: { siteId: siteId } });
    }

    public deleteUser(userId: number): ng.IPromise<void> {
        return this._resourceService
            .delete(this.resourceBaseUrl + 'users/delete-user', { params: { userId: userId } });
    }

    public deleteGroup(groupId: number): ng.IPromise<void> {
        return this._resourceService
            .delete(this.resourceBaseUrl + 'groups/delete-group', { params: { groupId: groupId } });
    }
}