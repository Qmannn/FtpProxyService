module Models {
    export interface ISiteGroup {
        id: number;
        name: string;
    }

    export interface ISite {
        name: string;
        description: string;
        id: number;
        groups: ISiteGroup[];
    }
}