module app.Navigation {
    'use strict';

    interface INavigationScope {
        nav: app.Services.INavigationService;
    }

    class NavigationController implements INavigationScope {
        nav: app.Services.INavigationService;
        static $inject = ['app.services.navigation'];
        constructor(nav: app.Services.INavigationService) {
            this.nav = nav;
        }

        next(): void {
            this.nav.next();
        }

        previous(): void {
            this.nav.previous();
        }

        current(): number {
            return this.nav.current();
        }

        jump(step: number): void {
            this.nav.jump(step);
        }

        setCurrentStep(step: number): void {
            this.nav.setCurrentStep(step);
        }

        sayHello(phrase: string): void {
            
        }
    }

    angular.module('app.navigation').controller('app.navigation.Controller', NavigationController);
}