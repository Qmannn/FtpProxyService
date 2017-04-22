module Models {
    export interface IUserGroup {
        id: number;
        name: string;
    }

    export interface IUser {
        name: string;
        login: string;
        id: number;
        groups: IUserGroup[];
    }
}