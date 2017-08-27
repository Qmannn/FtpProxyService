import { PreloaderService } from './../Preloader/PreloaderService';
export class HttpResourceService {
    private _httpService: ng.IHttpService;
    private _preloaderService: PreloaderService;

    public static $inject: string[] = ['$http', 'PreloaderService'];
    constructor(httpService: ng.IHttpService, preloaderService: PreloaderService) {
        this._httpService = httpService;
        this._preloaderService = preloaderService;
    }

    private wrapPromise<TResult>(promise: ng.IHttpPromise<TResult>): ng.IPromise<TResult> {
        return promise
            .then((result: ng.IHttpPromiseCallbackArg<TResult>): TResult => result.data)
            .finally(() => {
                this._preloaderService.showPreloader(false);
            });
    }

    public get<TResult>(url: string, config?: ng.IRequestShortcutConfig): ng.IPromise<TResult> {
        this._preloaderService.showPreloader(true);
        return this.wrapPromise(this._httpService.get(url, config));
    }

    public post<TResult>(url: string, data?: any, config?: ng.IRequestShortcutConfig): ng.IPromise<TResult> {
        this._preloaderService.showPreloader(true);
        return this.wrapPromise(this._httpService.post(url, data, config));
    }

    public put<TResult>(url: string, data?: any, config?: ng.IRequestShortcutConfig): ng.IPromise<TResult> {
        this._preloaderService.showPreloader(true);
        return this.wrapPromise(this._httpService.put(url, data, config));
    }

    public delete<TResult>(url: string, config?: ng.IRequestShortcutConfig): ng.IPromise<TResult> {
        this._preloaderService.showPreloader(true);
        return this.wrapPromise(this._httpService.delete(url, config));
    }
}