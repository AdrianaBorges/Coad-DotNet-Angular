"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Subject_1 = require("rxjs/Subject");
var LoaderService = /** @class */ (function () {
    function LoaderService() {
        this._showLoader = new Subject_1.Subject();
    }
    LoaderService.prototype.getShowLoader = function () {
        return this._showLoader.asObservable();
    };
    LoaderService.prototype.showLoader = function () {
        this._showLoader.next(true);
    };
    LoaderService.prototype.hideLoader = function () {
        this._showLoader.next(false);
    };
    return LoaderService;
}());
exports.LoaderService = LoaderService;
//# sourceMappingURL=loader.service.js.map