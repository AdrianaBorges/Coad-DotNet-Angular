import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Usuario } from '../models/usuario';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { ActionResult } from '../../base/models/action-result';
import { BaseService } from '../../base/services/base.service';
import { MessageService } from '../../base/services/message.service';
import { TypedResult } from '../../base/models/typed-result';
import { Router } from '@angular/router';

@Injectable()
export class AuthService extends BaseService {

  private loginSubject = new Subject<Usuario>();

  constructor(
    private http: HttpClient,
    @Inject('ENDPOINT_URL') private url: string,
    message: MessageService,
    private router: Router) {
      super(message);
  }

  realizarLogin(usuario: Usuario): Observable<TypedResult<Usuario>> {

    if (usuario) {
      this.message.addValidationMessages(null);
      const httpRequest = this.http.post<ActionResult>(this.url + 'api/login/realizar-login', usuario);

      return this.getTypedResult<Usuario>(httpRequest, 'autenticado')
        .pipe(map(result => {
          if (result && result.success) {

            const user = result.result;
            user.authData = this._retornarToken(user);
            sessionStorage.setItem('autenticado', JSON.stringify(user));
            result.result = user;
            this.loginSubject.next(user);
          }
          return result;
        })
        );
    }
    return null;
  }

  logout() {

    sessionStorage.removeItem('autenticado');
    this.updateUser(null);
    this.router.navigate(['']);
  }

  private _retornarToken(usuario: Usuario): string {

    if (usuario) {

      const token = window.btoa(usuario.login + ':' + usuario.senha);
      return token;
    }

    return null;
  }

  isLogado(): boolean {

    const userStr = sessionStorage.getItem('autenticado');

    if (userStr) {

      const usuario = JSON.parse(userStr) as Usuario;
      const token = this._retornarToken(usuario);

      return (usuario && usuario.authData && usuario.authData === token);
    }

    return false;

  }

  getUser(): Usuario {

    if (this.isLogado()) {
      const userStr = sessionStorage.getItem('autenticado');

      if (userStr) {

        const usuario = JSON.parse(userStr) as Usuario;
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
