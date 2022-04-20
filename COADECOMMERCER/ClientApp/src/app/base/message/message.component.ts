import { Component, OnInit, OnDestroy } from '@angular/core';
import { Message } from '../models/message';
import { MessageService } from '../services/message.service';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit, OnDestroy {
    
  message: Message;
  subscription: Subscription;

  constructor(private messageService: MessageService) { }

  ngOnInit() {

    this.subscription = this.messageService.getObservable()
      .subscribe(message => this.message = message);
  }
  fechar(): void {

    this.message = null;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
