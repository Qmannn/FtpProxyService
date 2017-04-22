module app.Services {
    'use strict';
    import IUser = Models.IUser;

    export interface IResourceService {
        getUsers(success: (users: IUser[]) => any, error: (data: any) => any): void;
        getUser(id: number, success: (users: IUser) => any, error: (data: any) => any): void;
    }

    class ResourceService implements IResourceService {
        
        private actionHash: ng.resource.IActionHash;
        private actionHashArray: ng.resource.IActionHash;

        static $inject = ['$resource'];
        constructor(private $resource: ng.resource.IResourceService) {
            let actionDescAtrray = <ng.resource.IActionDescriptor>{};
            actionDescAtrray.isArray = true;
            actionDescAtrray.method = 'POST';

            let actionDesc = <ng.resource.IActionDescriptor>{};
            actionDesc.isArray = false;
            actionDesc.method = 'POST';

            this.actionHash = <ng.resource.IActionHash>{
                ['save']: actionDesc,
                ['query']: actionDesc
            }

            this.actionHashArray = <ng.resource.IActionHash>{
                ['save']: actionDescAtrray,
                ['query']: actionDescAtrray
            }
        }

        getUsers(success: (users: IUser[]) => any, error: (data: any) => any): void {
            this.$resource('user/getusers', null, this.actionHashArray)
                .query(success, error);
        }

        getUser(id: number, success: (users: IUser) => any, error: (data: any) => any): void {
            this.$resource('user/getuser', id, this.actionHashArray)
                .query(success, error);
        }
    }

    angular.module('app.services').service('app.services.resource', ResourceService);
}