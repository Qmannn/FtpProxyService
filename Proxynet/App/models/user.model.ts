module Models {
    export interface IGroup {
        id: number;
        name: string;
    }

    export interface IUser {
        name: string;
        login: string;
        id: number;
        groups: IGroup[];
    }
}