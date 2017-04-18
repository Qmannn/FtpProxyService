module app.Services {
    'use strict';
    export interface INavigationService {
        currentStep : number;
        maxSteps    : number;
        next()      : void;
        previous()  : void;
        current()   : number;
        jump(step: number): void;
        setCurrentStep(step: number): void;
    }

    export interface ISteps {
        url: string[];
        getRoute(step: number): string;
    }

    class Steps implements ISteps {
        url: string[];

        constructor() {
            this.url = [
                'User'
            ];
        }

        getRoute(step: number): string {
            return this.url[step];
        }
    }

    class NavigationService implements INavigationService {
        currentStep : number;
        maxSteps    : number;
        steps       : ISteps;
        location    : ng.ILocationService;

        static $inject = ['$location'];
        constructor($location: ng.ILocationService) {
            this.location    = $location;
            this.currentStep = 0;
            this.maxSteps    = 1;
            this.steps       = new Steps();
        }

        next(): void {
            if (this.currentStep === this.maxSteps) {
                return;
            }

            if (this.currentStep + 1 <= this.maxSteps) {
                this.currentStep++;
                this.location.path(this.steps.getRoute(this.currentStep));
            }
        }

        previous(): void {
            if (this.currentStep === 0) {
                return;
            }

            if (this.currentStep - 1 > -1) {
                this.currentStep--;
                this.location.path(this.steps.getRoute(this.currentStep));
            }
        }

        current(): number {
            return this.currentStep;
        }

        jump(step: number): void {
            this.currentStep = step;
            this.location.path(this.steps.getRoute(step));
        }

        setCurrentStep(step: number): void {
            this.currentStep = step;
        }
    }

    angular.module('app.services').service('app.services.navigation', NavigationService);
} 