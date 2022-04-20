"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Observable_1 = require("rxjs/Observable");
var operators_1 = require("rxjs/operators");
var LoaderHttpInterceptor = /** @class */ (function () {
    function LoaderHttpInterceptor(loader) {
        this.loader = loader;
    }
    LoaderHttpInterceptor.prototype.intercept = function (req, next) {
        var _this = this;
        return next.handle(req).pipe(operators_1.map(function (result) {
            _this.loader.showLoader();
            return result;
        }), operators_1.catchError(function (error) {
            return Observable_1.Observable.throw(error);
        }), operators_1.finalize(function () {
            _this.loader.hideLoader();
        }));
    };
    return LoaderHttpInterceptor;
}());
exports.LoaderHttpInterceptor = LoaderHttpInterceptor;
//# sourceMappingURL=LoaderHttpInterceptor.js.map