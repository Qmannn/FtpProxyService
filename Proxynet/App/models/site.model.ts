import { IGroup } from './user.model';
export interface ISite {
        name: string;
        description: string;
        id: number;
        groups: IGroup[];
}