import { Injectable, Inject } from '@angular/core';
import { BaseService } from 'src/app/base/services/base.service';
import { MessageService } from 'src/app/base/services/message.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TypedResult } from 'src/app/base/models/typed-result';
import { TipoCliente } from '../models/tipo-cliente';
import { ActionResult } from 'src/app/base/models/action-result';

@Injectable({
  providedIn: 'root'
})
export class ClienteService extends BaseService {

  constructor(
    public message: MessageService,
    private http: HttpClient,
    @Inject('ENDPOINT_URL')private baseUrl: string
    ) {
      super(message);
    }


  listarTiposCliente(): Observable<TypedResult<TipoCliente[]>> {

    const url = this.baseUrl + 'api/cliente/buscar-tipos-ecommerce';

    const httpRequest = this.http.get<ActionResult>(url);
    return this.getTypedResult<TipoCliente[]>(httpRequest, 'lstTipoCliente');
  }

}
