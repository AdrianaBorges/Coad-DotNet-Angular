import { Injectable, Inject } from '@angular/core';
import { ProdutoComposicao } from '../models/produto-composicao';
import { CarrinhoComprasItem } from '../models/carrinho-compras-item';
import { BaseService } from '../../base/services/base.service';
import { MessageService } from '../../base/services/message.service';
import { HttpClient } from '@angular/common/http';
import { CarrinhoCompras } from '../models/carrinho-compras';
import { ActionResult } from '../../base/models/action-result';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { TypedResult } from '../../base/models/typed-result';
import { forEach } from '@angular/router/src/utils/collection';
import { AuthService } from '../../auth/services/auth.service';

@Injectable()
export class CarrinhoComprasService extends BaseService {

  constructor(public message: MessageService,
    @Inject('ENDPOINT_URL') private url: string,
    private http: HttpClient,
    private auth: AuthService

  ) {

    super(message);
  }

  private getMockObservableCarrinho(carrinho: CarrinhoCompras): Observable<TypedResult<CarrinhoCompras>> {

    let result = new TypedResult<CarrinhoCompras>();
    result.success = true;
    result.result = carrinho;

    return of(result);
  }

  adicionarProdutoCarrinho(produto: ProdutoComposicao): Observable<TypedResult<CarrinhoCompras>> {

    if (produto != null) {

      let carrinhoItens = new CarrinhoComprasItem();
      carrinhoItens.ccI_QTD = 1;
      carrinhoItens.ccI_VALOR_UNITARIO = produto.cmP_VLR_VENDA;
      carrinhoItens.cmP_ID = produto.cmP_ID;
      carrinhoItens.ccI_VALOR_TOTAL = produto.cmP_VLR_VENDA;
      carrinhoItens.data_criacao = new Date();

      if (this.auth.isLogado()) {

        let req = this.http.post<ActionResult>(this.url + "api/carrinho-compras/adicionar-produto-carrinho", carrinhoItens);
        return this.getTypedResult<CarrinhoCompras>(req, "carrinho");
      }
      else {
        let carrinho = this.getCarrinhoLocal();

        if (!carrinho) {

          carrinho = new CarrinhoCompras();
          carrinho.crC_VALOR_BRUTO = 0;
          carrinho.crC_VALOR_DESCONTO = 0;
          carrinho.crC_VALOR_FRETE = 0;
          carrinho.crC_VALOR_LIQUIDO = 0;
          carrinho.data_criacao = new Date();
          carrinho.carrinhO_COMPRAS_ITEM = [];
        }
          carrinho.carrinhO_COMPRAS_ITEM.push(carrinhoItens);
        return this.getMockObservableCarrinho(carrinho);
      }

    }
    
  }

  listarCarrinhoCliente(): Observable<TypedResult<CarrinhoCompras>> {

    if (this.auth.isLogado()) {
      var req = this.http.get<ActionResult>(this.url + "api/carrinho-compras/listar-carrinho-cliente");
      return this.getTypedResult<CarrinhoCompras>(req, "carrinho");
    }
    else {
      var carrinho = this.getCarrinhoLocal();
      return this.getMockObservableCarrinho(carrinho);
    }
  }

  calcularCarrinho(carrinho: CarrinhoCompras) {

    if (carrinho != null &&
      carrinho.carrinhO_COMPRAS_ITEM != null &&
      carrinho.carrinhO_COMPRAS_ITEM.length > 0) {

      carrinho.crC_VALOR_BRUTO = 0;
      carrinho.crC_VALOR_LIQUIDO = 0;

      if (carrinho.crC_VALOR_FRETE == null)
        carrinho.crC_VALOR_FRETE = 0;

      if (carrinho.crC_VALOR_DESCONTO == null)
        carrinho.crC_VALOR_DESCONTO = 0;

      carrinho.carrinhO_COMPRAS_ITEM.forEach(function (item) {

        item.ccI_VALOR_TOTAL = item.ccI_QTD * item.ccI_VALOR_UNITARIO;
        carrinho.crC_VALOR_BRUTO += item.ccI_VALOR_TOTAL;
      });

      carrinho.crC_VALOR_LIQUIDO = (carrinho.crC_VALOR_BRUTO + carrinho.crC_VALOR_FRETE) - carrinho.crC_VALOR_DESCONTO;
      return carrinho;
    }
  }


  atualizarCarrinho(carrinho: CarrinhoCompras): Observable<TypedResult<CarrinhoCompras>> {

    if (carrinho != null) {

      if (this.auth.isLogado()) {

        var req = this.http.put<ActionResult>(this.url + "api/carrinho-compras/atualizar", carrinho);
        return this.getTypedResult<CarrinhoCompras>(req, "carrinho");
      }
      else {
        return this.getMockObservableCarrinho(carrinho);
      }

    }

  }

  removerItem(index: number, carrinho: CarrinhoCompras) {

    if ((index || index == 0) &&
      carrinho &&
      carrinho.carrinhO_COMPRAS_ITEM) {

      carrinho.carrinhO_COMPRAS_ITEM.splice(index, 1);

      if (this.auth.isLogado()) {
        var req = this.http.put<ActionResult>(this.url + "api/carrinho-compras/atualizar", carrinho);
        return this.getTypedResult<CarrinhoCompras>(req, "carrinho");
      }
      else {
        return this.getMockObservableCarrinho(carrinho);
      }
    }
    return null;
  }

  apagarCarrinho(crcId: number) {

    if (crcId) {

      if (this.auth.isLogado()) {

        var req = this.http.delete<ActionResult>(this.url + `api/carrinho-compras/${crcId}`);
        return req;
      }
      else {
        this.setCarrinhoLocal(null);
        let result = new ActionResult();
        result.success = true;
        return of(result);
      }
    }
    return null;
  }

  getCarrinhoLocal(): CarrinhoCompras {

    var carrinhoStr = localStorage.getItem("carrinho");
    if (carrinhoStr) {

      let carrinho = JSON.parse(carrinhoStr) as CarrinhoCompras;
      return carrinho;
    }

    return null;
  }

  setCarrinhoLocal(carrinho: CarrinhoCompras): void{

    if (carrinho) {
      let carrinhoStr = JSON.stringify(carrinho);
      localStorage.setItem("carrinho", carrinhoStr);
    }
  }
}
