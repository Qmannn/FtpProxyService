var app;
(function (app) {
    var page0;
    (function (page0) {
        'use strict';
        var Page0Controller = (function () {
            function Page0Controller(nav) {
                nav.setCurrentStep(0);
                var vm = this;
            }
            return Page0Controller;
        }());
        Page0Controller.$inject = ['app.services.navigation'];
        angular.module('app.page0').controller('app.page0.Controller', Page0Controller);
    })(page0 = app.page0 || (app.page0 = {}));
})(app || (app = {}));
//# sourceMappingURL=page0.controller.js.map