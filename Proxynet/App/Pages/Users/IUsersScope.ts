import { IUser } from './../../Dto/UserDto';
import { Filter } from './Filter';

export interface IUsersScope {
    filter: Filter;
    users: IUser[];

    getFilteredUsers(): IUser[];
}