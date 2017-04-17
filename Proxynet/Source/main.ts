/// <reference path="Controllers/UserController.ts" />

namespace MainNs {
    import UserController = App.Controllers.UserController;

    class Main {
        public static main(): number {
            return 0;
        }
    }

    window.onload = () => {
        var ctrl: UserController = new UserController();
        alert(Main.main());
    }
}