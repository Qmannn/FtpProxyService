module app.Services {
    'use strict';
    export interface IResourceService {
        getStrings(callBack: Function );
    }

    class ResourceService implements IResourceService {

        static $inject = ['$resource'];
        constructor(private $resource: ng.resource.IResourceService) {
            alert('app.services.resource');
        }

        getStrings(callBack: Function) {
            let actionDesc = <ng.resource.IActionDescriptor>{};
            actionDesc.isArray = true;
            actionDesc.method = 'POST';

            let actionHash  = <ng.resource.IActionHash>{
                ['save']: actionDesc
            }

            var jsonGetter = this.$resource('user/GetUsers', null, actionHash);
            jsonGetter.save(callBack);
        }
    }

    angular.module('app.services').service('app.services.resource', ResourceService);
}