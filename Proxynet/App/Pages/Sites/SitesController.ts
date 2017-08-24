import { ISiteSaver } from './../../Core/Services/Savers/SiteSaver';
import { IResourceService } from './../../services/app.services.resource';
import { PageControllerBase } from './../PageControllerBase';
import { IGroup } from './../../Dto/GroupDto';
import { ISite } from './../../Dto/SiteDto';
import { Site } from './../../Dto/SiteToSaveDto';
import { IScope } from 'angular';
'use strict';

interface ISIteScope extends IScope {
    newSiteData: Site;
    sites: ISite[];
    groups: IGroup[];

    saveNewSite(): void;
}

export class SitesController extends PageControllerBase {

    private resourceService: IResourceService;
    private _scope: ISIteScope;
    private _siteSaver: ISiteSaver;

    public static $inject: string[] = [
        '$scope',
        'app.services.resource',
        'sitesaver'];

    constructor($scope: ISIteScope, resourceService: IResourceService, siteSaver: ISiteSaver) {
        super();
        this.resourceService = resourceService;
        this._scope = $scope;
        this._siteSaver = siteSaver;

        this.initScope();
    }

    private initScope(): void {
        this.clearSiteData();
        this.getSites();
        this.getGroups();

        this.bindMethidsToScope();
    }

    private clearSiteData(): void {
        this._scope.newSiteData = new Site();
    }

    private bindMethidsToScope(): void {
        this._scope.saveNewSite = () => this.saveNewSite();
    }

    public updateSites(): void {
        this.resourceService.updateSites((): void => {
            this.getSites();
        }, () => this.onError);
    }

    private getSites(): void {
        this.resourceService.getSites((sites: ISite[]): void => {
            this._scope.sites = sites;
            _.forEach(this._scope.sites, (site: ISite): void => {
                site.editLink = '/site/' + site.id;
            });
        }, () => this.onError);
    }

    private getGroups(): void {
        this.resourceService.getGroups((groups: IGroup[]): void => {
            this._scope.groups = groups;
        }, this.onError);
    }

    private saveNewSite(): void {
        if ( this._siteSaver.validateSiteData( this._scope.newSiteData ) ) {
            this._siteSaver.saveSite( this._scope.newSiteData );
        }
    }
}