import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Usuario } from '../auth/models/usuario';
import { AuthService } from '../auth/services/auth.service';
import { Observable } from 'rxjs/Observable';
import { MessageService } from '../base/services/message.service';
import { Router } from '@angular/router';
import { LoaderService } from '../base/services/loader.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  usuarioSubscription: Subscription;
  @Input() usuario: Usuario;
  showLoader: boolean;
  @Input() loginIsRunning: boolean;
  constructor(
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router,
    private loader: LoaderService
  ) { }

  ngOnInit() {

    this.loginIsRunning = false;
    if (!this.authService.isLogado()) {
      if (!this.usuario) {
        this.usuario = new Usuario();
      }
    }
    else {
      this.router.navigate(['']);
    }
  }


  ngOnDestroy(): void {

    if (this.usuarioSubscription) {
      this.usuarioSubscription.unsubscribe();
    }
  }

  realizarLogin(): void {
    this.loginIsRunning = true;
    var observable = this.authService.realizarLogin(this.usuario);

    if (observable) {
      this.usuarioSubscription = observable.subscribe(result => {
        this.loginIsRunning = false;
        if (result.success) {

          this.router.navigate(['']);
        }

        this.messageService.adicionarMensagem(result.message);
      },
        error => {
          this.loginIsRunning = false;
        });
    }
    else {

      this.messageService.fail("Login Inválido");
    }
  }

  toogleLoader() {

    this.showLoader = !this.showLoader;

    if (this.showLoader)
      this.loader.showLoader();
    else
      this.loader.hideLoader();
  }

}
