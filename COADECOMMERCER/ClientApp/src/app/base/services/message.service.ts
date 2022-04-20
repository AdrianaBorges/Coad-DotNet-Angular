import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Message } from '../models/message';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class MessageService {

  private messageSubject = new Subject<Message>();
  private validationSubject = new Subject<object>();

  constructor() { }

  adicionarMensagem(message: Message) {

    this.messageSubject.next(message);
  }

  apagarMensagem() {

    this.messageSubject.next();
  }

  fail(message: string) {

    let msg = new Message();
    msg.type = 'fail';
    msg.message = message;

    this.adicionarMensagem(msg);
  }

  warning(message: string) {

    let msg = new Message();
    msg.type = 'warning';
    msg.message = message;

    this.adicionarMensagem(msg);
  }

  success(message: string) {

    let msg = new Message();
    msg.type = 'success';
    msg.message = message;

    this.adicionarMensagem(msg);
  }

  info(message: string) {

    let msg = new Message();
    msg.type = 'info';
    msg.message = message;

    this.adicionarMensagem(msg);
  }

  getObservable(): Observable<Message>{

    return this.messageSubject.asObservable();
  }

  getValidation(): Observable<object> {

    return this.validationSubject.asObservable();
  }

  addValidationMessages(validationMessages: object) {

    this.validationSubject.next(validationMessages);
  }
}
