import { IGroup } from './../../Dto/GroupDto';
import { IUser } from './../../Dto/UserDto';

export interface IUserEditScope extends ng.IScope {
    user: IUser;
    allowedToUserGroups: IGroup[];
    newGroupName: string;
    allGroups: IGroup[];
    isNewUser: boolean;
    changePassword: boolean;
    userForm: ng.IFormController;

    getAllowedToUserGroups(): IGroup[];
    addGroup(group: IGroup): void;
    removeGroup(group: IGroup): void;
    saveUser(): void;
    addNewGroup(name: string): void;
}