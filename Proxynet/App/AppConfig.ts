export module AppConfig {
    export function AppBaseUrl(): string {
        return '/';
    }

    export function RouteBaseUrl(): string {
        return AppBaseUrl() + 'FtpProxy/';
    }
}