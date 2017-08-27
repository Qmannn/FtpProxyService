import { ISite } from './ISite';
import { IGroup } from './GroupDto';

export class Site implements ISite {
    public name: string;
    public address: string;
    public port: number;
    public login: string;
    public password: string;
    public description: string;
    public id: number;
    public groups: IGroup[];
    public editLink: string;

    constructor() {
        this.groups = [];
    }
}