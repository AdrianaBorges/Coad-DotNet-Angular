import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VitrineComponent } from './vitrine/vitrine.component';
import { BaseModule } from '../base/base.module';
import { CarrinhoComponent } from './carrinho/carrinho.component';
import { FormsModule } from '@angular/forms';
import { AppRouterModule } from '../app-router/app-router.module';

import { LayoutScriptsService } from '../layouts/services/layout-scripts.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HorizontalMenuComponent } from '../layouts/horizontal-menu/horizontal-menu.component';
import { CarrinhoAcessoRapidoComponent } from '../layouts/carrinho-acesso-rapido/carrinho-acesso-rapido.component';
import { BarraUsuarioComponent } from '../layouts/barra-usuario/barra-usuario.component';

@NgModule({
  imports: [
    CommonModule,
    BaseModule,
    FormsModule,
    AppRouterModule,
    BrowserAnimationsModule
  ],
  declarations: [
    VitrineComponent,
    CarrinhoComponent,
    HorizontalMenuComponent,
    CarrinhoAcessoRapidoComponent,
    BarraUsuarioComponent
  ],
  providers: [LayoutScriptsService],
  exports: [
    CarrinhoComponent,
    HorizontalMenuComponent,
    BarraUsuarioComponent
  ]
})
export class LojaVitrineModule { }
