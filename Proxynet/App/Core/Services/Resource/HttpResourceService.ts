import { NotificationService } from './../Notifications/NotificationsService';
import { PreloaderService } from './../Preloader/PreloaderService';
import { NotificationType } from '../Notifications/NotificationType';
export class HttpResourceService {
    private _httpService: ng.IHttpService;
    private _preloaderService: PreloaderService;
    private _notificationService: NotificationService;

    public static $inject: string[] = ['$http', 'PreloaderService', 'NotificationService'];
    constructor(httpService: ng.IHttpService, preloaderService: PreloaderService, notificationService: NotificationService) {
        this._httpService = httpService;
        this._preloaderService = preloaderService;
        this._notificationService = notificationService;
    }

    private wrapPromise<TResult>(promise: ng.IHttpPromise<TResult>): ng.IPromise<TResult> {
        return promise
            .catch((result: ng.IHttpPromiseCallbackArg<TResult>): TResult => {
                this._notificationService.showNotification('Ошибка', NotificationType.Error);
                return result.data;
            })
            .then((result: ng.IHttpPromiseCallbackArg<TResult>): TResult => {
                this._notificationService.showNotification('Успех', NotificationType.Success);
                return result.data;
            })
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