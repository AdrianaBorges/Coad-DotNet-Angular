import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, Routes } from '@angular/router';
import { LojaLayoutComponent } from '../layouts/loja-layout/loja-layout.component';
import { VitrineComponent } from '../loja-vitrine/vitrine/vitrine.component';
import { LoginComponent } from '../login/login.component';
import { CarrinhoComponent } from '../loja-vitrine/carrinho/carrinho.component';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CheckoutComponent } from '../loja-vitrine/checkout/checkout.component';
import { CadastroComponent } from '../loja-vitrine/cadastro/cadastro.component';


const routes: Routes = [
  {
    path: '', component: LojaLayoutComponent, children: [
      { path: '', component: VitrineComponent, pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'carrinho', component: CarrinhoComponent },
      { path: 'checkout', component: CheckoutComponent },
      { path: 'cadastro', component: CadastroComponent }
    ]
  },
  // {
  //  //path: 'admin', component: FetchDataComponent, children: [
  //  //  { path: '', component: HomeComponent, pathMatch: 'full' },
  //  //  { path: 'counter', component: CounterComponent },
  //  //  { path: 'fetch-data', component: FetchDataComponent },
  //  //  { path: 'pedidos', component: ListPedidoComponent },
  //  //  { path: 'pedido/:id', component: PedidoEdicaoComponent }
  //  //]
  // }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes, {scrollPositionRestoration : 'enabled'}),
    BrowserModule,
    BrowserAnimationsModule],
  exports: [ RouterModule ],
  declarations: []
})
export class AppRouterModule { }
