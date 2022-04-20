import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActionResult } from '../../base/models/action-result';
import { Observable } from 'rxjs/Observable';
import { TypedResult } from '../../base/models/typed-result';
import { ProdutoComposicao } from '../models/produto-composicao';
import { MessageService } from '../../base/services/message.service';
import { BaseService } from '../../base/services/base.service';

@Injectable()
export class ProdutoService extends BaseService {

  constructor(
    private http: HttpClient,
    @Inject('ENDPOINT_URL') private baseUrl: string,
    public message: MessageService
  ) {
    super(message)
  }

  listarPedidosVitrine(pagina?: number): Observable<TypedResult<ProdutoComposicao[]>> {

    var url = this.baseUrl + "api/vitrine/listarProdutosVitrine";

    if (pagina > 0) {
      url += "?pagina=" + pagina;
    }

    var httpRequest = this.http.get<ActionResult>(url);
    return this.getTypedResult<ProdutoComposicao[]>(httpRequest, "lstProdutosVitrine");
  }

}
