import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VitrineComponent } from './vitrine/vitrine.component';
import { BaseModule } from '../base/base.module';
import { CarrinhoComponent } from './carrinho/carrinho.component';
import { FormsModule } from '@angular/forms';

import { LayoutScriptsService } from '../layouts/services/layout-scripts.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HorizontalMenuComponent } from '../layouts/horizontal-menu/horizontal-menu.component';
import { CarrinhoAcessoRapidoComponent } from '../layouts/carrinho-acesso-rapido/carrinho-acesso-rapido.component';
import { BarraUsuarioComponent } from '../layouts/barra-usuario/barra-usuario.component';
import { LojaLayoutComponent } from '../layouts/loja-layout/loja-layout.component';
import { RouterModule } from '@angular/router';
import { CheckoutComponent } from './checkout/checkout.component';
import { ClienteEditorComponent } from './cliente-editor/cliente-editor.component';
import { CadastroComponent } from './cadastro/cadastro.component';

@NgModule({
  imports: [
    CommonModule,
    BaseModule,
    FormsModule,
    RouterModule,
    BrowserAnimationsModule
  ],
  declarations: [
    VitrineComponent,
    CarrinhoComponent,
    HorizontalMenuComponent,
    CarrinhoAcessoRapidoComponent,
    BarraUsuarioComponent,
    LojaLayoutComponent,
    CheckoutComponent,
    ClienteEditorComponent,
    CadastroComponent,
  ],
  providers: [LayoutScriptsService],
  exports: [
    CarrinhoComponent,
    HorizontalMenuComponent,
    BarraUsuarioComponent
  ]
})
export class LojaVitrineModule { }
