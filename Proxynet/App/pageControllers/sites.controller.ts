import { IResourceService } from './../services/app.services.resource';
import { ISite } from './../models/site.model';

'use strict';

export class SitesController {
    public sites: ISite[];

    private resourceService: IResourceService;

    public static $inject: string[] = ['app.services.resource'];

    constructor(resourceService: IResourceService) {
        this.resourceService = resourceService;
        this.getSites();
    }

    public updateSites(): void {
        this.resourceService.updateSites((): void => {
            this.getSites();
        }, () => undefined);
    }

    public getSites(): void {
        this.resourceService.getSites((sites: ISite[]): void => {
            this.sites = sites;
        }, () => undefined);
    }
}