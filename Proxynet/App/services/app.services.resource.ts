module app.Services {
    'use strict';
    import IUser = Models.IUser;

    export interface IResourceService {
        getUsers( success: ( users: IUser[] ) => any, error: ( data: any ) => any ): void;
        getUser( id: number, success: ( users: IUser ) => any, error: ( data: any ) => any ): void;
        getGroups(success: (groups: Models.IGroup[]) => any, error: (data: any) => any): void;
        
        getSites(success: (users: Models.ISite[]) => any, error: (data: any) => any): void;
        getSite(id: number, success: (users: Models.ISite) => any, error: (data: any) => any): void;
        getSiteGroups(success: (groups: Models.IGroup[]) => any, error: (data: any) => any): void;

        saveUser(user: IUser, success: (user: IUser) => any, error: (data: any) => any): void;
        saveSite(site: Models.ISite, success: (site: Models.ISite) => any, error: (data: any) => any): void;
        saveGroup(name: string, success: (group: Models.IGroup) => any, error: (data: any) => any): void;

        updateUsers(success: () => any, error: (data:any) => any):void;
        updateSites(success: () => any, error: (data:any) => any):void;
    }

    class ResourceService implements IResourceService {
        private actionHash: ng.resource.IActionHash;
        private actionHashArray: ng.resource.IActionHash;
        private resourceBaseUrl: string;

        static $inject = ['$resource'];

        constructor( private $resource: ng.resource.IResourceService ) {
            let actionDescAtrray = <ng.resource.IActionDescriptor>{};
            actionDescAtrray.isArray = true;
            actionDescAtrray.method = 'POST';
            let actionDesc = <ng.resource.IActionDescriptor>{};
            actionDesc.isArray = false;
            actionDesc.method = 'POST';

            this.resourceBaseUrl = 'secure/';

            this.actionHash = <ng.resource.IActionHash>{
                ['save']: actionDesc,
                ['query']: actionDesc
            }

            this.actionHashArray = <ng.resource.IActionHash>{
                ['save']: actionDescAtrray,
                ['query']: actionDescAtrray
            }
        }

        // GET

        getUsers( success: ( users: IUser[] ) => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'user/getusers', null, this.actionHashArray )
                .query( success, error );
        }

        getUser( id: number, success: ( user: IUser ) => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'user/getuser', { 'userId': id }, this.actionHash )
                .query( success, error );
        }

        getGroups( success: ( groups: Models.IGroup[] ) => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'user/getgroups', null, this.actionHashArray )
                .query( success, error );
        }

        getSites( success: ( users: Models.ISite[] ) => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'site/getsites', null, this.actionHashArray )
                .query( success, error );
        }

        getSite( id: number, success: ( site: Models.ISite ) => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'site/getsite', { 'siteId': id }, this.actionHash )
                .query( success, error );
        }

        getSiteGroups( success: ( groups: Models.IGroup[] ) => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'site/getgroups', null, this.actionHashArray )
                .query( success, error );
        }

        updateUsers( success: () => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'user/updateusers', null, this.actionHash )
                .query( success, error );
        }

        updateSites( success: () => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'site/updatesites', null, this.actionHash )
                .query( success, error );
        }

        // SAVE

        saveUser( user: IUser, success: ( user: IUser ) => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'user/saveuser', { 'users': user }, this.actionHash )
                .query( success, error );
        }

        saveSite( site: Models.ISite, success: ( site: Models.ISite ) => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'site/savesite', { 'site': site }, this.actionHash )
                .query( success, error );
        }

        saveGroup( name: string, success: ( group: Models.IGroup ) => any, error: ( data: any ) => any ): void {
            this.$resource( this.resourceBaseUrl + 'user/savegroup', { 'name': name }, this.actionHash )
                .query( success, error );
        }
    }

    angular.module('app.services').service('app.services.resource', ResourceService);
}