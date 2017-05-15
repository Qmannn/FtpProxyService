((): void => {
    'use strict';
    angular.module('app.routes').config(['$locationProvider', '$routeProvider', config]);

    var baseUrl = '/secure/FtpProxy/';
    var templateBaseUrl = 'secure/';

    function config($locationProvider: ng.ILocationProvider, $routeProvider: ng.route.IRouteProvider): void {
        $routeProvider
            .when( baseUrl,
                {
                    templateUrl: templateBaseUrl + 'Pages/Users'
                } )
            .when( baseUrl + 'users',
                {
                    templateUrl: templateBaseUrl + 'Pages/Users'
                } )
            .when( baseUrl + 'user/:userid',
                {
                    templateUrl: templateBaseUrl + 'Pages/UserEdit'
                } )
            .when( baseUrl + 'sites',
                {
                    templateUrl: templateBaseUrl + 'Pages/Sites'
                } )
            .when( baseUrl + 'site/:siteid',
                {
                    templateUrl: templateBaseUrl + 'Pages/SiteEdit'
                } );
    }
})();