import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProdutoService } from '../../business/services/produto.service';
import { ProdutoComposicao } from '../../business/models/produto-composicao';
import { Pagina } from '../../base/models/page';
import { Subscription } from 'rxjs';
import { CarrinhoComprasService } from '../../business/services/carrinho-compras.service';
import { MessageService } from '../../base/services/message.service';
import { Router } from '@angular/router';
import { ProdutoVenda } from 'src/app/business/models/produto-venda';

@Component({
  selector: 'app-vitrine',
  templateUrl: './vitrine.component.html',
  styleUrls: ['./vitrine.component.css']
})
export class VitrineComponent implements OnInit, OnDestroy {

  lstProdutoVitrine: ProdutoVenda[];
  produtoVitrineSub: Subscription;
  singleLine: boolean;
  paginaVitrine: Pagina;


  constructor(
    private produtoService: ProdutoService,
    private carrinhoService: CarrinhoComprasService,
    private message: MessageService,
    private router: Router
  ) { }

  ngOnInit() {
    this.listarProdutoVitrine();
    this.singleLine = false;
  }

  listarProdutoVitrine(pagina?: number) {

    this.message.apagarMensagem();
    const lstResult = this.produtoService.listarPedidosVitrine(pagina);

    if (lstResult) {

      this.produtoVitrineSub = lstResult.subscribe(result => {

        if (result.success) {

          this.lstProdutoVitrine = result.result;
          this.paginaVitrine = result.pagina;
        } else {
          this.message.adicionarMensagem(result.message);
        }
      });
    }
  }

  setViewList(sigleLine: boolean) {

    this.singleLine = sigleLine;
  }

  ngOnDestroy(): void {
    if (this.produtoVitrineSub) {
      this.produtoVitrineSub.unsubscribe();
    }
  }

  adicionarProdutoCarrinho(produto: ProdutoVenda) {

    this.carrinhoService.adicionarProdutoCarrinho(produto).subscribe(carrinho => {

      if (carrinho.success) {

        this.router.navigate(['', 'carrinho'], {fragment : 'carrinho'});
      } else {
        this.message.adicionarMensagem(carrinho.message);
      }
    });

  }

}
