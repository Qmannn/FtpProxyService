import { UserAccount } from './UserAccount';
import { IGroup } from './GroupDto';
export interface IUser {
    name: string;
    login: string;
    id: number;
    groups: IGroup[];
    editLink: string;
    account: UserAccount;
}