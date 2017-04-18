var app;
(function (app) {
    var Services;
    (function (Services) {
        'use strict';
        var ResourceService = (function () {
            function ResourceService($resource) {
                this.$resource = $resource;
                alert('app.services.resource');
            }
            ResourceService.prototype.getStrings = function (callBack) {
                var actionDesc = {};
                actionDesc.isArray = true;
                actionDesc.method = 'POST';
                var actionHash = (_a = {},
                    _a['save'] = actionDesc,
                    _a);
                var jsonGetter = this.$resource('user/GetUsers', null, actionHash);
                jsonGetter.save(callBack);
                var _a;
            };
            return ResourceService;
        }());
        ResourceService.$inject = ['$resource'];
        angular.module('app.services').service('app.services.resource', ResourceService);
    })(Services = app.Services || (app.Services = {}));
})(app || (app = {}));
//# sourceMappingURL=app.services.resource.js.map