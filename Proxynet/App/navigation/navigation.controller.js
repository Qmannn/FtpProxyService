var app;
(function (app) {
    var Navigation;
    (function (Navigation) {
        'use strict';
        var NavigationController = (function () {
            function NavigationController(nav) {
                this.nav = nav;
            }
            NavigationController.prototype.next = function () {
                this.nav.next();
            };
            NavigationController.prototype.previous = function () {
                this.nav.previous();
            };
            NavigationController.prototype.current = function () {
                return this.nav.current();
            };
            NavigationController.prototype.jump = function (step) {
                this.nav.jump(step);
            };
            NavigationController.prototype.setCurrentStep = function (step) {
                this.nav.setCurrentStep(step);
            };
            NavigationController.prototype.sayHello = function (phrase) {
            };
            return NavigationController;
        }());
        NavigationController.$inject = ['app.services.navigation'];
        angular.module('app.navigation').controller('app.navigation.Controller', NavigationController);
    })(Navigation = app.Navigation || (app.Navigation = {}));
})(app || (app = {}));
//# sourceMappingURL=navigation.controller.js.map