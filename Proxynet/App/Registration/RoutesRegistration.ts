import { RegistratorBase } from './RegistratorBase';
import { AppConfig } from '../AppConfig';

export class RoutesRegistrator extends RegistratorBase {
    public register(): void {
        this._angular.module('app.routes').config(['$routeProvider', this.setRoutes]);
    }

    private setRoutes($routeProvider: ng.route.IRouteProvider): void {
        const baseUrl: string = AppConfig.RouteBaseUrl();
        const templateBaseUrl: string = AppConfig.AppBaseUrl();
        $routeProvider
            .when(baseUrl,
            {
                templateUrl: templateBaseUrl + 'Pages/Users'
            })
            .when(baseUrl + 'users',
            {
                templateUrl: templateBaseUrl + 'Pages/Users'
            })
            .when(baseUrl + 'user/:userid',
            {
                templateUrl: templateBaseUrl + 'Pages/UserEdit'
            })
            .when(baseUrl + 'sites',
            {
                templateUrl: templateBaseUrl + 'Pages/Sites'
            })
            .when(baseUrl + 'site/:siteid',
            {
                templateUrl: templateBaseUrl + 'Pages/SiteEdit'
            })
            .otherwise({ templateUrl: baseUrl });
    }
}