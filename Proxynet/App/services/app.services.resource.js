var app;
(function (app) {
    var Services;
    (function (Services) {
        'use strict';
        var ResourceService = (function () {
            function ResourceService($resource) {
                this.$resource = $resource;
                var actionDescAtrray = {};
                actionDescAtrray.isArray = true;
                actionDescAtrray.method = 'POST';
                var actionDesc = {};
                actionDesc.isArray = false;
                actionDesc.method = 'POST';
                this.actionHash = (_a = {},
                    _a['save'] = actionDesc,
                    _a['query'] = actionDesc,
                    _a);
                this.actionHashArray = (_b = {},
                    _b['save'] = actionDescAtrray,
                    _b['query'] = actionDescAtrray,
                    _b);
                var _a, _b;
            }
            ResourceService.prototype.getUsers = function (success, error) {
                this.$resource('user/getusers', null, this.actionHashArray)
                    .query(success, error);
            };
            ResourceService.prototype.getUser = function (id, success, error) {
                this.$resource('user/getuser', id, this.actionHashArray)
                    .query(success, error);
            };
            return ResourceService;
        }());
        ResourceService.$inject = ['$resource'];
        angular.module('app.services').service('app.services.resource', ResourceService);
    })(Services = app.Services || (app.Services = {}));
})(app || (app = {}));
//# sourceMappingURL=app.services.resource.js.map