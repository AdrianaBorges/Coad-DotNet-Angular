import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';
import { Subscription } from 'rxjs';
import { Usuario } from '../../auth/models/usuario';

@Component({
  selector: 'app-barra-usuario',
  templateUrl: './barra-usuario.component.html',
  styleUrls: ['./barra-usuario.component.css']
})
export class BarraUsuarioComponent implements OnInit, OnDestroy {

  private userSubscription: Subscription;
  public usuario: Usuario;
  public isLogado: boolean;

  constructor(private auth: AuthService) { }

  ngOnInit() {

    const authUser = this.auth.getUser();
    this.setUsuario(authUser);
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
