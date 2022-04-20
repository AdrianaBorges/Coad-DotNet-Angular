import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { map, catchError } from 'rxjs/operators';
import 'rxjs/add/observable/throw';
import { HttpErrorResponse } from '@angular/common/http';
import { ActionResult } from '../models/action-result';
import { TypedResult } from '../models/typed-result';
import { MessageService } from './message.service';

export abstract class BaseService {

  constructor(public message: MessageService) { }

  getResult<T>(httpObservable: Observable<ActionResult>, entityName: string): Observable<T>{

    return httpObservable.pipe(map(result => {
      if (result) {
        if (result.success)
          return result.result[entityName] as T;
      }
    }),
     catchError(error => {
      return this.handlerError(error);
      })
    );
  }

  getTypedResult<T>(httpObservable: Observable<ActionResult>, entityName: string): Observable<TypedResult<T>> {

    return httpObservable.pipe(map(result => {
      var paginatedResult = new TypedResult<T>();
      if (result) {

        paginatedResult.result = result.result[entityName] as T;
        paginatedResult.pagina = result.page;
        paginatedResult.message = result.message;
        paginatedResult.success = result.success;

      }
      return paginatedResult;
    }),
      catchError(error => {
        return this.handlerError(error);
      })
    );
  }

  defaultPipe(httpObservable: Observable<ActionResult>): Observable<ActionResult> {

    return httpObservable.pipe(map(result => {

      return result;
    }),
      catchError(error => {
        return this.handlerError(error);
      })
    );
  }

  private handlerError(error: HttpErrorResponse) {

    if (error.error instanceof ErrorEvent) {

      var errorEvent = error.error as ErrorEvent;
      this.message.fail(errorEvent.message);
    }
    else
      if (error.status == 400) { // Na web api 400 é erro de validação

        this.message.fail("Ocorreram erros de validação.");
        this.message.addValidationMessages(error.error);
        return Observable.throw("Erro de Validação");
      }
      else if(error.status == 404){

        this.message.fail("Não foi possível se conectar ao servidor. Tente novamente mais tarde.");
        return Observable.throw("Não foi possível encontrar a url");
      } if (error.status == 500) {

        this.message.fail("Ocorreu um erro interno no servidor.");
        return Observable.throw("Não foi possível encontrar a url");
      }
      else if (error.status == 401) {

        this.message.fail("Você não tem permissão para acessar esses dados.");
        return Observable.throw("Não foi possível encontrar a url");
      }
      else {
        this.message.fail("Ocorreu um erro desconhecido.");
        return Observable.throw(error);
      }
  }

}
