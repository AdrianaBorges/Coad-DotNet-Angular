import { Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class LoaderService {

  _showLoader: Subject<boolean> = new Subject<boolean>();

  constructor() { }

  getShowLoader(): Observable<boolean> {

    return this._showLoader.asObservable();
  }

  showLoader(): void {

    this._showLoader.next(true);
  }

  hideLoader(): void {
    this._showLoader.next(false);
  }

}
