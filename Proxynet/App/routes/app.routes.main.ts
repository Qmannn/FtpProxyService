((): void => {
    'use strict';
    angular.module('app.routes').config(['$locationProvider', '$routeProvider', config]);

    function config($locationProvider: ng.ILocationProvider, $routeProvider: ng.route.IRouteProvider): void {
        $routeProvider
            .when('/User', {
                templateUrl: '/User/TestVIew'
            })
            .when('/User/TestVIew', {
                templateUrl: '/User/TestVIew'
            })
            .when('/User/Test', {
                templateUrl: '/User/TestVIew'
            });
    }
})();