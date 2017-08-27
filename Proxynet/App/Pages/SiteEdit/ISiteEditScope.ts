import { ISite } from './../../Dto/ISite';
import { IGroup } from './../../Dto/GroupDto';
export interface ISiteEditScope extends ng.IScope {
    allGroups: IGroup[];
    newGroupName: string;
    site: ISite;
    isNewSite: boolean;
    siteForm: ng.IFormController;

    saveSite(): void;
    getAllowedToSiteGroups(): void;
    addGroup(group: IGroup): void;
    removeGroup(group: IGroup): void;
    deleteGroup(group: IGroup): void;
    saveSite(): void;
    addNewGroup(name: string): void;
}