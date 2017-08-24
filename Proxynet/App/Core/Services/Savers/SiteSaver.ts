import { IResourceService } from './../../../services/app.services.resource';
import { Site } from './../../../Dto/SiteToSaveDto';

export interface ISiteSaver {
    validateSiteData(site: Site): boolean;
    saveSite(site: Site): void;
}

export class SiteSaver implements ISiteSaver {
    private _resourceService: IResourceService;
    public static $inject: string[] = ['app.services.resource'];
    constructor(resourceService: IResourceService) {
        this._resourceService = resourceService;
    }

    public validateSiteData(site: Site): boolean {
        return !_.isUndefined(site)
            && !_.isUndefined(site.name);
    }

    public saveSite(site: Site): void {
        this._resourceService.createSite(site).then( (item: Site): void => {
            alert(item.name);
        } );
    }
}