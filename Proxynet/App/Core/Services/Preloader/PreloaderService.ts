export class PreloaderService {
    private onChangePreloaderState: (show: boolean) => void;
    private activationCount: number;
    constructor() {
        this.activationCount = 0;
    }

    public showPreloader(show: boolean): void {
        if (!_.isUndefined(this.onChangePreloaderState)) {
            this.onChangePreloaderState(show);
        }
    }

    public preloaderStateCanBeModified(show: boolean): boolean {
        this.activationCount += show ? 1 : -1;
        if (_.isUndefined(this.onChangePreloaderState) || (this.activationCount > 0 && !show)) {
            return false;
        }
        return true;
    }

    public setPreloaderChanger(onChangePreloaderState: (show: boolean) => void): void {
        this.onChangePreloaderState = onChangePreloaderState;
    }
}