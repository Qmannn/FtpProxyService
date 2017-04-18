((): void => {
    'use strict';
    angular.module('app').config(config);

    config.$inject = ['$locationProvider', '$httpProvider', '$resourceProvider'];

    function config($locationProvider: ng.ILocationProvider,
        $httpProvider: ng.IHttpProvider,
        $resourceProvider: ng.resource.IResourceServiceProvider): void {
        $resourceProvider.defaults.stripTrailingSlashes = false;
        $locationProvider.html5Mode(true);
        $httpProvider.defaults.withCredentials = true;
    }
})();