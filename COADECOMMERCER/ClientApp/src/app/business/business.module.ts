import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PedidoService } from './services/pedido.service';
import { ProdutoService } from './services/produto.service';
import { CarrinhoComprasService } from './services/carrinho-compras.service';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [PedidoService,
    ProdutoService,
    CarrinhoComprasService]
})
export class BusinessModule { }
