import { UserAccount } from './UserAccount';
import { IGroup } from './GroupDto';
import { IUser } from './UserDto';

export class User implements IUser {
    public name: string;
    public login: string;
    public id: number;
    public groups: IGroup[];
    public editLink: string;
    public account: UserAccount;

    constructor() {
        this.groups = [];
        this.account = new UserAccount();
        this.id = 0;
    }
}