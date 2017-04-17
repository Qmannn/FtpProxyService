define("Controllers/UserController", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var App;
    (function (App) {
        var Controllers;
        (function (Controllers) {
            var UserController = (function () {
                function UserController() {
                }
                UserController.prototype.getUsers = function (count) {
                    return new Array(count);
                };
                return UserController;
            }());
            Controllers.UserController = UserController;
        })(Controllers = App.Controllers || (App.Controllers = {}));
    })(App = exports.App || (exports.App = {}));
    var UserController = (function () {
        function UserController() {
        }
        UserController.prototype.getUsers = function (count) {
            return new Array(count);
        };
        return UserController;
    }());
    exports.UserController = UserController;
});
define("main", ["require", "exports", "Controllers/UserController"], function (require, exports, UserController_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    /// <reference path="Controllers/UserController.ts" />
    var MainNs;
    (function (MainNs) {
        //import UserController = App.Controllers.UserController;
        var Main = (function () {
            function Main() {
            }
            Main.main = function () {
                return 0;
            };
            return Main;
        }());
        window.onload = function () {
            var ctrl = new UserController_1.UserController();
            alert(Main.main());
        };
    })(MainNs || (MainNs = {}));
});
//# sourceMappingURL=typescript.js.map