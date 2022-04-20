import { Injectable, Inject } from '@angular/core';
import { Pedido } from '../models/pedido';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class PedidoService {

  http: HttpClient;
  baseUrl: string;
  constructor(http: HttpClient, @Inject("ENDPOINT_URL") baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  getPedidos(): Observable<Pedido[]> {

    return this.http.get<Pedido[]>(this.baseUrl + 'api/pedido');
  }

  getPedido(pedidoId: number): Observable<Pedido>{

    if (pedidoId) {
      return this.http.get<Pedido>(this.baseUrl + 'api/pedido/' + pedidoId);
    }
    return null;
  }
}

