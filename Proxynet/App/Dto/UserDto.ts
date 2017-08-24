import { IGroup } from './GroupDto';
export interface IUser {
    name: string;
    login: string;
    id: number;
    groups: IGroup[];
}