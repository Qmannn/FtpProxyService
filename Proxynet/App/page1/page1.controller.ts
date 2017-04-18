module app.page1 {
    'use strict';
    interface IAddress {
        streetNumber : number;
        address      : string;
        address2     : string;
        city         : string;
        state        : string;
        postalCode   : string;
    }

    interface IImpassaOptions {
        option1: number;
    }

    interface IPowerSeriesOptions{
        option1: number;
        option2: number;
        option3: number;
    }

    interface IExistingOptions {
        model: string;
        serial: string;
    }

    enum PanelType {
        Impassa,
        PowerSeries,
        Existing
    }

    interface IPanelOptions {
        isResidential      : boolean;
        panelType          : PanelType;
        impassaOptions     : IImpassaOptions;
        powerSeriesOptions : IPowerSeriesOptions;
        existingOptions    : IExistingOptions;
    }

    class PanelOptions implements IPanelOptions {
        isResidential      : boolean;
        panelType          : PanelType;
        impassaOptions     : IImpassaOptions;
        powerSeriesOptions : IPowerSeriesOptions;
        existingOptions    : IExistingOptions;

        constructor() {
            this.isResidential      = null;
            this.panelType          = null;
            this.impassaOptions     = null;
            this.powerSeriesOptions = null;
            this.existingOptions    = null;
        }
    }

    class ServiceAddress implements IAddress {
        streetNumber : number;
        address      : string;
        address2     : string;
        city         : string;
        state        : string;
        postalCode   : string;
    }

    class BillingAddress implements IAddress {
        streetNumber : number;
        address      : string;
        address2     : string;
        city         : string;
        state        : string;
        postalCode   : string;
    }

    class Page1Controller {
        serviceAddress : ServiceAddress;
        billingAddress : BillingAddress;
        panelOptions   : IPanelOptions;

        static $inject = ['app.services.navigation'];
        constructor(nav: app.Services.INavigationService) {
            nav.setCurrentStep(1);
            this.serviceAddress = new ServiceAddress();
            this.billingAddress = new BillingAddress();
            this.panelOptions   = new PanelOptions();

            this.preLoadServiceAddress();
        }

        setBillingAddressToServiceAddress(): void {
            this.billingAddress.streetNumber = this.serviceAddress.streetNumber;
            this.billingAddress.address      = this.serviceAddress.address;
            this.billingAddress.address2     = this.serviceAddress.address2;
            this.billingAddress.city         = this.serviceAddress.city;
            this.billingAddress.state        = this.serviceAddress.state;
            this.billingAddress.postalCode   = this.serviceAddress.postalCode; 
        }

        private preLoadServiceAddress(): void {
            this.serviceAddress.streetNumber = 123;
            this.serviceAddress.address      = 'Main St.';     
            this.serviceAddress.address2     = 'Suite 10';    
            this.serviceAddress.city         = 'Carmel';        
            this.serviceAddress.state        = 'Indiana';      
            this.serviceAddress.postalCode   = '46032'; 
        }
    }

    angular.module('app.page1').controller('app.page1.Controller', Page1Controller);
} 