import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Usuario } from '../models/usuario';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operators';
import { errorHandler } from '@angular/platform-browser/src/browser';
import { ActionResult } from '../../base/models/action-result';
import { BaseService } from '../../base/services/base.service';
import { MessageService } from '../../base/services/message.service';
import { TypedResult } from '../../base/models/typed-result';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class AuthService extends BaseService {

  private loginSubject = new Subject<Usuario>();

  constructor(
    private http: HttpClient,
    @Inject("ENDPOINT_URL") private url: string,
    message: MessageService) {
      super(message);
  }

  realizarLogin(usuario: Usuario): Observable<TypedResult<Usuario>> {

    if (usuario) {
      this.message.addValidationMessages(null);
      var httpRequest = this.http.post<ActionResult>(this.url + "api/login/realizar-login", usuario);

      return this.getTypedResult<Usuario>(httpRequest, "autenticado")
        .pipe(map(result => {
          if (result && result.success) {

            let usuario = result.result;
            usuario.authData = this._retornarToken(usuario);
            sessionStorage.setItem("autenticado", JSON.stringify(usuario));
            result.result = usuario;
            this.loginSubject.next(usuario);
          }
          return result;
        })
        );
    }
    return null;
  }

  logout() {

    sessionStorage.removeItem("autenticado");
    this.updateUser(null);
  }

  private _retornarToken(usuario: Usuario): string {

    if (usuario) {

      let token = window.btoa(usuario.login + ":" + usuario.senha);
      return token;
    }

    return null;
  }

  isLogado(): boolean {

    let userStr = sessionStorage.getItem("autenticado");

    if (userStr) {

      let usuario = JSON.parse(userStr) as Usuario;
      let token = this._retornarToken(usuario);

      return (usuario && usuario.authData && usuario.authData == token)
    }

    return false;

  }

  getUser(): Usuario {

    if (this.isLogado()) {
      let userStr = sessionStorage.getItem("autenticado");

      if (userStr) {

        let usuario = JSON.parse(userStr) as Usuario;
        return usuario;
      }
    }

    return null;
  }

  subscribeUser(): Observable<Usuario> {

    return this.loginSubject.asObservable();
  }

  updateUser(usuario: Usuario) {

    this.loginSubject.next(usuario);
  }
}
