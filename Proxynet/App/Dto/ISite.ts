﻿import { IGroup } from './GroupDto';
export interface ISite {
        name: string;
        description: string;
        id: number;
        groups: IGroup[];
        editLink: string;
        address: string;
        port: number;
        login: string;
        password: string;
}