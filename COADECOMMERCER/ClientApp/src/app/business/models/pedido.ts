import { Cliente } from './cliente';
import { PedidoStatus } from './pedidoStatus'

export class Pedido {

  pedidoId: number;
  valorTotal: number;
  dataPedido: Date;
  cliente: Cliente;
  status: PedidoStatus;
  valorFrete: number;
}

