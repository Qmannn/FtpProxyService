import { NotificationService } from './../Core/Services/Notifications/NotificationsService';
import { HttpResourceService } from './../Core/Services/Resource/HttpResourceService';
import { PreloaderService } from './../Core/Services/Preloader/PreloaderService';
import { ResourceService } from './../services/app.services.resource';
import { RegistratorBase } from './RegistratorBase';
import { NavigationService } from '../services/app.services.navigation';

export class ServiceRegistrator extends RegistratorBase {

    public register(): void {
        this._angular.module('app.services').service('app.services.navigation', NavigationService);
        this._angular.module('app.services').service('HttpResourceService', HttpResourceService);
        this._angular.module('app.services').service('app.services.resource', ResourceService);
        this._angular.module('app.services').service('PreloaderService', PreloaderService);
        this._angular.module('app.services').service('NotificationService', NotificationService);
    }
}