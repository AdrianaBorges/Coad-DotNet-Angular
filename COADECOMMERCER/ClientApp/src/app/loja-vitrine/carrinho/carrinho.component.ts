import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CarrinhoCompras } from '../../business/models/carrinho-compras';
import { CarrinhoComprasService } from '../../business/services/carrinho-compras.service';
import { MessageService } from '../../base/services/message.service';
import { LayoutScriptsService } from '../../layouts/services/layout-scripts.service';
import { SlideInOutAnimation } from '../../base/animations/animations';

@Component({
  selector: 'app-carrinho',
  templateUrl: './carrinho.component.html',
  styleUrls: ['./carrinho.component.css'],
  animations: SlideInOutAnimation
})
export class CarrinhoComponent implements OnInit {
    

  carrinho: CarrinhoCompras;
  calculoTimeout: any;

  //Controle de animações
  resumoCarrinhoIsOpen: boolean;

  constructor(
    private carrinhoService: CarrinhoComprasService,
    private message: MessageService
  ) { }

  ngOnInit() {

    this.resumoCarrinhoIsOpen = true;
    this.listarCarrinhoCliente();
  }
  
  listarCarrinhoCliente() {

    this.message.apagarMensagem();

    var observable = this.carrinhoService.listarCarrinhoCliente();

    if (observable) {
      observable.subscribe(result => {

        if (result.success) {

          this.carrinho = result.result;
        }
        else {

          this.message.adicionarMensagem(result.message);
        }
      });
    }
  }

  calcularCarrinho() {

    if (this.calculoTimeout) {

      clearTimeout(this.calculoTimeout);
    }

    this.carrinho = this.carrinhoService.calcularCarrinho(this.carrinho);

    this.calculoTimeout = setTimeout(() => {

      this.atualizarCarrinho();

    }, 1000);
    
  }

  atualizarCarrinho() {

    this.carrinhoService.atualizarCarrinho(this.carrinho).subscribe(result => {

      if (result.success) {

        this.listarCarrinhoCliente();
      }
      else {
        this.message.adicionarMensagem(result.message);
      }
    });

  }

  removerItem(index: number) {

    var req = this.carrinhoService.removerItem(index, this.carrinho);

    if (req) {

      req.subscribe(result => {

        if (result.success) {
          this.listarCarrinhoCliente();
        }
        else {
          this.message.adicionarMensagem(result.message);
        }
      });
    }
  }

  apagarCarrinho() {

    if (this.carrinho && this.carrinho.crC_ID) {
      var req = this.carrinhoService.apagarCarrinho(this.carrinho.crC_ID);

      if (req) {

        req.subscribe(result => {

          if (result.success) {
            this.listarCarrinhoCliente();
          }
          else {
            this.message.adicionarMensagem(result.message);
          }
        });
      }
    }
  }
}
