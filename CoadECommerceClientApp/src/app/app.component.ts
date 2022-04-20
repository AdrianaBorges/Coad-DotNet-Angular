import { Component, OnInit, OnDestroy } from '@angular/core';
import { CarrinhoComprasService } from './business/services/carrinho-compras.service';
import { MessageService } from './base/services/message.service';
import { AuthService } from './auth/services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
    title = 'app';

    userSubscription: Subscription;
    constructor(
      private carrinhoService: CarrinhoComprasService,
      private message: MessageService,
      private auth: AuthService
    ) { }

    ngOnInit(): void {
      this.userSubscription = this.auth.subscribeUser().subscribe(usuario => {
        this._userStateChange();
      });
    }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

  private _userStateChange() {
    if (this.auth.isLogado()) {
      this.carrinhoService.checarCarrinhoUsuarioLogado();
    }
  }
}
