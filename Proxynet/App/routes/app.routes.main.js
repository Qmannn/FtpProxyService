(function () {
    'use strict';
    angular.module('app.routes').config(['$locationProvider', '$routeProvider', config]);
    function config($locationProvider, $routeProvider) {
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
//# sourceMappingURL=app.routes.main.js.map