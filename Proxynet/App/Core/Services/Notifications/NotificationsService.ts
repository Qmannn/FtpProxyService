import { IJQueryGrowl, NotificationSettings } from './JQueryGrowl';
import { NotificationType } from './NotificationType';
export class NotificationService {
    private _jqueryGrowl: IJQueryGrowl = ($ as IJQueryGrowl);

    public showNotification(message: string, type: NotificationType): void {
        let messageType: string;
        switch (type) {
            case NotificationType.Success:
                messageType = 'success';
                break;
            case NotificationType.Error:
                messageType = 'danger';
                break;
            default:
                break;
        }
        let settings: NotificationSettings = undefined;
        if (messageType !== '') {
            settings = new NotificationSettings();
            settings.type = messageType;
        }
        this._jqueryGrowl.bootstrapGrowl(message, settings);
    }
}