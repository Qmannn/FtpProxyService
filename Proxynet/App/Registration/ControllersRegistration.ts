import { UsersController } from './../Pages/Users/UsersController';
import { UserController } from './../Pages/UserEdit/UserEditController';
import { SitesController } from './../Pages/Sites/SitesController';
import { SiteController } from './../Pages/SiteEdit/SiteEditController';
import { Navbar } from './../Pages/NavigationBar/NavigationController';
import { RegistratorBase } from './RegistratorBase';

export class ControllersRegistrator extends RegistratorBase {
    public register(): void {
        this._angular.module('app.navbar').controller('NavbarController', Navbar);
        this._angular.module('app.pageControllers').controller('SiteController', SiteController);
        this._angular.module('app.pageControllers').controller('SitesController', SitesController);
        this._angular.module('app.pageControllers').controller('UserController', UserController);
        this._angular.module('app.pageControllers').controller('UsersController', UsersController);
    }
}