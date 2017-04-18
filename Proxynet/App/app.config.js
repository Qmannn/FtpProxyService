(function () {
    'use strict';
    angular.module('app').config(config);
    config.$inject = ['$locationProvider', '$httpProvider', '$resourceProvider'];
    function config($locationProvider, $httpProvider, $resourceProvider) {
        $resourceProvider.defaults.stripTrailingSlashes = false;
        $locationProvider.html5Mode(true);
        $httpProvider.defaults.withCredentials = true;
    }
})();
//# sourceMappingURL=app.config.js.map