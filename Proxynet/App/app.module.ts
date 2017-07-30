import { ServiceRegistrator } from './Registration/ServiceRegistration';
import { RoutesRegistrator } from './Registration/RoutesRegistration';
import { IRegistrator } from './Registration/IRegistrator';
import { ControllersRegistrator } from './Registration/ControllersRegistration';

'use strict';

// Modules creating
angular.module('app.routes', []);
angular.module('app.services', []);
angular.module('app.pageControllers', []);
angular.module('app.navbar', []);
angular.module('app.core', ['ngResource', 'ngRoute']);

angular.module('app', ['app.core',
    'app.routes',
    'app.services',
    'app.navbar',
    'app.pageControllers'
]);

angular.module('app').config(config);

config.$inject = ['$locationProvider', '$httpProvider', '$resourceProvider'];

function config($locationProvider: ng.ILocationProvider,
    $httpProvider: ng.IHttpProvider,
    $resourceProvider: ng.resource.IResourceServiceProvider): void {
    $resourceProvider.defaults.stripTrailingSlashes = false;
    $locationProvider.html5Mode(true);
    $httpProvider.defaults.withCredentials = true;
}

// Registration
const registrators: IRegistrator[] = [];
registrators.push(new ControllersRegistrator(angular));
registrators.push(new RoutesRegistrator(angular));
registrators.push(new ServiceRegistrator(angular));

_.forEach( registrators, ( registrator: IRegistrator ) => registrator.register() );
