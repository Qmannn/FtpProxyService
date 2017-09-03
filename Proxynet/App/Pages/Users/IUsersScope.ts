import { IUser } from './../../Dto/UserDto';
import { Filter } from './Filter';

export interface IUsersScope {
    filter: Filter;
    users: IUser[];
    createUserLink: string;

    getFilteredUsers(): IUser[];
    deleteUser(user: IUser): void;
}