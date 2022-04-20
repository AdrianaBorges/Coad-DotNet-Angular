import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError, finalize } from 'rxjs/operators';
import { LoaderService } from '../services/loader.service';
import { Injectable } from '@angular/core';
import 'rxjs/add/observable/throw';

@Injectable()
export class LoaderHttpInterceptor implements HttpInterceptor  {

  constructor(private loader: LoaderService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {

    return next.handle(req).pipe(
      map(result => {

        setTimeout(() => {
          this.loader.showLoader();
        });
        return result;
      }),
      catchError(error => {
        return Observable.throw(error);
      }),
      finalize(() => {

        setTimeout(() => {
          this.loader.hideLoader();
        });
      })
    );
    }


}
