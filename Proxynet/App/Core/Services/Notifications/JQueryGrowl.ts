export interface IJQueryGrowl extends JQueryStatic {
    bootstrapGrowl(message: string, settings: NotificationSettings): void;
}

export class NotificationSettings {
    public type: string;
}