import { AppConfig } from './../../AppConfig';
import { IResourceService } from './../../services/app.services.resource';
import { PageControllerBase } from './../PageControllerBase';
import { IGroup } from './../../Dto/GroupDto';
import { ISite } from './../../Dto/ISite';
import { IScope } from 'angular';
'use strict';

interface ISIteScope extends IScope {
    sites: ISite[];
    createSiteLink: string;

    deleteSite(site: ISite): void;
}

export class SitesController extends PageControllerBase {

    private resourceService: IResourceService;
    private _scope: ISIteScope;

    public static $inject: string[] = [
        '$scope',
        'app.services.resource'];

    constructor($scope: ISIteScope, resourceService: IResourceService) {
        super();
        this.resourceService = resourceService;
        this._scope = $scope;

        this.initScope();
    }

    private initScope(): void {
        this.getSites();
        this.getGroups();
        this._scope.createSiteLink = AppConfig.appName + '#/site';

        this.bindMethidsToScope();
    }

    private bindMethidsToScope(): void {
        this._scope.deleteSite = (site: ISite) => this.deleteSite(site);
    }

    private getSites(): void {
        this.resourceService.getSites((sites: ISite[]): void => {
            this._scope.sites = sites;
            _.forEach(this._scope.sites, (site: ISite): void => {
                site.editLink = AppConfig.appName + '#/site/' + site.id;
            });
        }, () => this.onError);
    }

    private getGroups(): void {
        this.resourceService.getGroups((groups: IGroup[]): void => {
            this._scope.groups = groups;
        }, this.onError);
    }

    private deleteSite(site: ISite): void {
        this.resourceService.deleteSite(site.id).then(() => {
            _.remove(this._scope.sites, (item: ISite) => item.id === site.id);
        });
    }
}