module Models {

    export interface ISite {
        name: string;
        description: string;
        id: number;
        groups: IGroup[];
    }
}