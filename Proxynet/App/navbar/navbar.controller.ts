module app.Navbar {
    "use strict";
    interface INavbar {
        text: string;
    }

    class Navbar implements INavbar {
        text: string;
        private scope: ng.IScope;

        static $inject = ['$scope', 'app.services.resource'];

        constructor(private $scope: ng.IScope, private resourceService: app.Services.IResourceService) {
            this.text = 'Navbar Controller';
            $scope.text = 'testTextFromScope';
            this.scope = $scope;
            let localThis = this;
            resourceService.getStrings((data) => {
                localThis.onUsersLoaded(data);
            });

        }

        private onUsersLoaded(data: any) {
            console.log(this);
            console.log(this.$scope);
            this.scope.text += '!!!' + data[0].DisplayName;
        }
    }

    angular.module('app.navbar').controller('app.navbar.Controller', Navbar);
}