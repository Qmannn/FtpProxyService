export class UserAccount {
    public login: string;
    public password: string;
    public needSaveAccount: boolean;
    public needChangePassword: boolean;

    constructor() {
        this.needChangePassword = true;
        this.needSaveAccount = true;
    }
}