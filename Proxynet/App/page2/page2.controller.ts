module app.page2 {
    'use strict';
    interface IPage2 {
        firstName : string;
        middleName: string;
        lastName  : string;
    }

    class Page2Controller implements IPage2 {
        firstName  : string;
        middleName : string;
        lastName   : string;        

        static $inject = ['app.services.navigation'];
        constructor(nav: app.Services.INavigationService) {
            nav.setCurrentStep(2);            
            this.firstName  = '';
            this.middleName = '';
            this.lastName   = '';
        }
    }

    angular.module('app.page2').controller('app.page2.Controller', Page2Controller);
} 