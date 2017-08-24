export class PageControllerBase {
    protected onError(error: Object): void {
        throw new ControllerException( error );
    }
}

class ControllerException {
    public data: Object;
    constructor(data: Object) {
        this.data = data;
    }
}