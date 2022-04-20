import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';
import { Subscription } from 'rxjs/Subscription';
import { Usuario } from '../../auth/models/usuario';

@Component({
  selector: 'app-barra-usuario',
  templateUrl: './barra-usuario.component.html',
  styleUrls: ['./barra-usuario.component.css']
})
export class BarraUsuarioComponent implements OnInit, OnDestroy {

  private userSubscription: Subscription;
  private usuario: Usuario;
  public isLogado: boolean;

  constructor(private auth: AuthService) { }

  ngOnInit() {

    let user = this.auth.getUser();
    this.setUsuario(user);
    this.userSubscription = this.auth.subscribeUser()
      .subscribe(user => {
        this.setUsuario(user);
      });
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

  setUsuario(usuario: Usuario) {

    this.usuario = usuario;
    this.isLogado = this.auth.isLogado();
  }

  logout(): void {
    this.auth.logout();
  } 

}
