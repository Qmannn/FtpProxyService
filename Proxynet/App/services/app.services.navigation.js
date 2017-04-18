var app;
(function (app) {
    var Services;
    (function (Services) {
        'use strict';
        var Steps = (function () {
            function Steps() {
                this.url = [
                    'User'
                ];
            }
            Steps.prototype.getRoute = function (step) {
                return this.url[step];
            };
            return Steps;
        }());
        var NavigationService = (function () {
            function NavigationService($location) {
                this.location = $location;
                this.currentStep = 0;
                this.maxSteps = 1;
                this.steps = new Steps();
            }
            NavigationService.prototype.next = function () {
                if (this.currentStep === this.maxSteps) {
                    return;
                }
                if (this.currentStep + 1 <= this.maxSteps) {
                    this.currentStep++;
                    this.location.path(this.steps.getRoute(this.currentStep));
                }
            };
            NavigationService.prototype.previous = function () {
                if (this.currentStep === 0) {
                    return;
                }
                if (this.currentStep - 1 > -1) {
                    this.currentStep--;
                    this.location.path(this.steps.getRoute(this.currentStep));
                }
            };
            NavigationService.prototype.current = function () {
                return this.currentStep;
            };
            NavigationService.prototype.jump = function (step) {
                this.currentStep = step;
                this.location.path(this.steps.getRoute(step));
            };
            NavigationService.prototype.setCurrentStep = function (step) {
                this.currentStep = step;
            };
            return NavigationService;
        }());
        NavigationService.$inject = ['$location'];
        angular.module('app.services').service('app.services.navigation', NavigationService);
    })(Services = app.Services || (app.Services = {}));
})(app || (app = {}));
//# sourceMappingURL=app.services.navigation.js.map