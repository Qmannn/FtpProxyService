var app;
(function (app) {
    var page1;
    (function (page1) {
        'use strict';
        var PanelType;
        (function (PanelType) {
            PanelType[PanelType["Impassa"] = 0] = "Impassa";
            PanelType[PanelType["PowerSeries"] = 1] = "PowerSeries";
            PanelType[PanelType["Existing"] = 2] = "Existing";
        })(PanelType || (PanelType = {}));
        var PanelOptions = (function () {
            function PanelOptions() {
                this.isResidential = null;
                this.panelType = null;
                this.impassaOptions = null;
                this.powerSeriesOptions = null;
                this.existingOptions = null;
            }
            return PanelOptions;
        }());
        var ServiceAddress = (function () {
            function ServiceAddress() {
            }
            return ServiceAddress;
        }());
        var BillingAddress = (function () {
            function BillingAddress() {
            }
            return BillingAddress;
        }());
        var Page1Controller = (function () {
            function Page1Controller(nav) {
                nav.setCurrentStep(1);
                this.serviceAddress = new ServiceAddress();
                this.billingAddress = new BillingAddress();
                this.panelOptions = new PanelOptions();
                this.preLoadServiceAddress();
            }
            Page1Controller.prototype.setBillingAddressToServiceAddress = function () {
                this.billingAddress.streetNumber = this.serviceAddress.streetNumber;
                this.billingAddress.address = this.serviceAddress.address;
                this.billingAddress.address2 = this.serviceAddress.address2;
                this.billingAddress.city = this.serviceAddress.city;
                this.billingAddress.state = this.serviceAddress.state;
                this.billingAddress.postalCode = this.serviceAddress.postalCode;
            };
            Page1Controller.prototype.preLoadServiceAddress = function () {
                this.serviceAddress.streetNumber = 123;
                this.serviceAddress.address = 'Main St.';
                this.serviceAddress.address2 = 'Suite 10';
                this.serviceAddress.city = 'Carmel';
                this.serviceAddress.state = 'Indiana';
                this.serviceAddress.postalCode = '46032';
            };
            return Page1Controller;
        }());
        Page1Controller.$inject = ['app.services.navigation'];
        angular.module('app.page1').controller('app.page1.Controller', Page1Controller);
    })(page1 = app.page1 || (app.page1 = {}));
})(app || (app = {}));
//# sourceMappingURL=page1.controller.js.map