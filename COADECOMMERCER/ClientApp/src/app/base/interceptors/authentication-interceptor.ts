import { HttpInterceptor, HttpRequest, HttpHandler } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { AuthService } from "../../auth/services/auth.service";

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor{

  constructor(private auth: AuthService) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {

    var usuario = this.auth.getUser();

    if (usuario && usuario.authData) {

      req = req.clone({
        setHeaders: {
          Authorization: `Basic ${usuario.authData}`
        }
      });
    }

    return next.handle(req);
  }


}
