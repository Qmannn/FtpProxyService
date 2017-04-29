((): void => {
    'use strict';
    angular.module('app.routes').config(['$locationProvider', '$routeProvider', config]);

    function config($locationProvider: ng.ILocationProvider, $routeProvider: ng.route.IRouteProvider): void {
        $routeProvider
            .when( '/FtpProxy/',
                {
                    templateUrl: 'Pages/Users'
                } )
            .when( '/FtpProxy/users',
                {
                    templateUrl: 'Pages/Users'
                } )
            .when( '/FtpProxy/user/:userid',
                {
                    templateUrl: 'Pages/UserEdit'
                } )
            .when( '/FtpProxy/sites',
                {
                    templateUrl: 'Pages/Sites'
                } )
            .when( '/FtpProxy/site/:siteid',
                {
                    templateUrl: 'Pages/SiteEdit'
                } );
    }
})();