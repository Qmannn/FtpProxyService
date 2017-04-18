var app;
(function (app) {
    var Navbar;
    (function (Navbar_1) {
        "use strict";
        var Navbar = (function () {
            function Navbar($scope, resourceService) {
                this.$scope = $scope;
                this.resourceService = resourceService;
                this.text = 'Navbar Controller';
                $scope.text = 'testTextFromScope';
                this.scope = $scope;
                var localThis = this;
                resourceService.getStrings(function (data) {
                    localThis.onUsersLoaded(data);
                });
            }
            Navbar.prototype.onUsersLoaded = function (data) {
                console.log(this);
                console.log(this.$scope);
                this.scope.text += '!!!' + data[0].DisplayName;
            };
            return Navbar;
        }());
        Navbar.$inject = ['$scope', 'app.services.resource'];
        angular.module('app.navbar').controller('app.navbar.Controller', Navbar);
    })(Navbar = app.Navbar || (app.Navbar = {}));
})(app || (app = {}));
//# sourceMappingURL=navbar.controller.js.map