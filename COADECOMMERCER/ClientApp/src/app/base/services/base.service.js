"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Observable_1 = require("rxjs/Observable");
var operators_1 = require("rxjs/operators");
require("rxjs/add/observable/throw");
var typed_result_1 = require("../models/typed-result");
var BaseService = /** @class */ (function () {
    function BaseService(message) {
        this.message = message;
    }
    BaseService.prototype.getResult = function (httpObservable, entityName) {
        var _this = this;
        return httpObservable.pipe(operators_1.map(function (result) {
            if (result) {
                if (result.success)
                    return result.result[entityName];
            }
        }), operators_1.catchError(function (error) {
            return _this.handlerError(error);
        }));
    };
    BaseService.prototype.getTypedResult = function (httpObservable, entityName) {
        var _this = this;
        return httpObservable.pipe(operators_1.map(function (result) {
            var paginatedResult = new typed_result_1.TypedResult();
            if (result) {
                paginatedResult.result = result.result[entityName];
                paginatedResult.pagina = result.page;
                paginatedResult.message = result.message;
                paginatedResult.success = result.success;
            }
            return paginatedResult;
        }), operators_1.catchError(function (error) {
            return _this.handlerError(error);
        }));
    };
    BaseService.prototype.defaultPipe = function (httpObservable) {
        var _this = this;
        return httpObservable.pipe(operators_1.map(function (result) {
            return result;
        }), operators_1.catchError(function (error) {
            return _this.handlerError(error);
        }));
    };
    BaseService.prototype.handlerError = function (error) {
        if (error.error instanceof ErrorEvent) {
            var errorEvent = error.error;
            this.message.fail(errorEvent.message);
        }
        else if (error.status == 400) { // Na web api 400 é erro de validação
            this.message.fail("Ocorreram erros de validação.");
            this.message.addValidationMessages(error.error);
            return Observable_1.Observable.throw("Erro de Validação");
        }
        else if (error.status == 404) {
            this.message.fail("Não foi possível se conectar ao servidor. Tente novamente mais tarde.");
            return Observable_1.Observable.throw("Não foi possível encontrar a url");
        }
        if (error.status == 500) {
            this.message.fail("Ocorreu um erro interno no servidor.");
            return Observable_1.Observable.throw("Não foi possível encontrar a url");
        }
        else {
            this.message.fail("Ocorreu um erro desconhecido.");
            return Observable_1.Observable.throw(error);
        }
    };
    return BaseService;
}());
exports.BaseService = BaseService;
//# sourceMappingURL=base.service.js.map