module app.page0 {
    'use strict';
    class Page0Controller {
        static $inject = ['app.services.navigation'];
        constructor(nav: app.Services.INavigationService) {
            nav.setCurrentStep(0);
            var vm = this;
        }
    }

    angular.module('app.page0').controller('app.page0.Controller', Page0Controller);
}   