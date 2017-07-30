import { RegistratorBase } from './RegistratorBase';
import { UsersController } from './../pageControllers/users.controller';
import { UserController } from './../pageControllers/user.controller';
import { SitesController } from './../pageControllers/sites.controller';
import { SiteController } from './../pageControllers/site.controller';
import { Navbar } from './../navbar/navbar.controller';

export class ControllersRegistrator extends RegistratorBase {
    public register(): void {
        this._angular.module('app.navbar').controller('NavbarController', Navbar);
        this._angular.module('app.pageControllers').controller('SiteController', SiteController);
        this._angular.module('app.pageControllers').controller('SitesController', SitesController);
        this._angular.module('app.pageControllers').controller('UserController', UserController);
        this._angular.module('app.pageControllers').controller('UsersController', UsersController);
    }
}