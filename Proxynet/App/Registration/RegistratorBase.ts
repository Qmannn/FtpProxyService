import { IRegistrator } from './IRegistrator';

export class RegistratorBase implements IRegistrator {
    protected _angular: angular.IAngularStatic;

    constructor(angular: angular.IAngularStatic) {
        this._angular = angular;
    }

    public register(): void {
        throw new Error('Method not implemented.');
    }
}