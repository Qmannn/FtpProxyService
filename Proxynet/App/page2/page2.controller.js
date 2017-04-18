var app;
(function (app) {
    var page2;
    (function (page2) {
        'use strict';
        var Page2Controller = (function () {
            function Page2Controller(nav) {
                nav.setCurrentStep(2);
                this.firstName = '';
                this.middleName = '';
                this.lastName = '';
            }
            return Page2Controller;
        }());
        Page2Controller.$inject = ['app.services.navigation'];
        angular.module('app.page2').controller('app.page2.Controller', Page2Controller);
    })(page2 = app.page2 || (app.page2 = {}));
})(app || (app = {}));
//# sourceMappingURL=page2.controller.js.map