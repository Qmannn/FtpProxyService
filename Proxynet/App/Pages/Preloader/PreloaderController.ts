import { IPreloaderScope } from './IPreloaderScope';
import { PreloaderService } from './../../Core/Services/Preloader/PreloaderService';
export class PreloaderController {
    private _preloaderService: PreloaderService;
    private _scope: IPreloaderScope;

    public static $inject: string[] = ['$scope', 'PreloaderService'];
    constructor($scope: IPreloaderScope, preloaderService: PreloaderService) {
        this._preloaderService = preloaderService;
        this._scope = $scope;

        this._preloaderService.setPreloaderChanger((show: boolean) => this.onChangePreloaderState(show));
        this.initScope();
    }

    private initScope(): void {
        this._scope.showPreloader = false;
    }

    private onChangePreloaderState(show: boolean): void {
        this._scope.showPreloader = show;
    }
}