import { RegistratorBase } from './RegistratorBase';
import { AppConfig } from '../AppConfig';

export class RoutesRegistrator extends RegistratorBase {
    public register(): void {
        this._angular.module('app.routes').config(['$routeProvider', this.setRoutes]);
        this._angular.module('app').config(['$locationProvider', this.setLocationSettings]);
    }

    private setRoutes($routeProvider: ng.route.IRouteProvider): void {
        const baseUrl: string = AppConfig.appBaseUrl;
        const templateBaseUrl: string = AppConfig.routeBaseUrl;
        $routeProvider
            .when(baseUrl,
            {
                redirectTo: baseUrl + 'users'
            })
            .when(baseUrl + 'users',
            {
                templateUrl: templateBaseUrl + '/Users/Users.html'
            })
            .when(baseUrl + 'user/:userid',
            {
                templateUrl: templateBaseUrl + '/UserEdit/UserEdit.html'
            })
            .when(baseUrl + 'user',
            {
                templateUrl: templateBaseUrl + '/UserEdit/UserEdit.html'
            })
            .when(baseUrl + 'sites',
            {
                templateUrl: templateBaseUrl + '/Sites/Sites.html'
            })
            .when(baseUrl + 'site/:siteid',
            {
                templateUrl: templateBaseUrl + '/SiteEdit/SiteEdit.html'
            })
            .when(baseUrl + 'site',
            {
                templateUrl: templateBaseUrl + '/SiteEdit/SiteEdit.html'
            })
            .when(baseUrl + 'Account/Login', {
                template: ''
            })
            .otherwise({ redirectTo: baseUrl + 'users' });
    }

    private setLocationSettings($locationProvider: ng.ILocationProvider): void {
        $locationProvider.html5Mode(false);
        $locationProvider.hashPrefix('');
    }
}