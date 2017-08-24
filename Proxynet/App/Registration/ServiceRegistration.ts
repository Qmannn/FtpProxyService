import { SiteSaver } from './../Core/Services/Savers/SiteSaver';
import { ResourceService } from './../services/app.services.resource';
import { RegistratorBase } from './RegistratorBase';
import { NavigationService } from '../services/app.services.navigation';

export class ServiceRegistrator extends RegistratorBase {

    public register(): void {
        this._angular.module('app.services').service('app.services.navigation', NavigationService);
        this._angular.module('app.services').service('app.services.resource', ResourceService);
        this._angular.module('app.services').service('sitesaver', SiteSaver);
    }
}