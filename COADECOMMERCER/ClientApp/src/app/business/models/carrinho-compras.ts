import { Cliente } from "./cliente";
import { CarrinhoComprasItem } from "./carrinho-compras-item";

export class CarrinhoCompras {

  crC_ID: number;
  clI_ID: number;
  data_criacao: Date;
  crC_VALOR_BRUTO: number;
  crC_VALOR_FRETE: number;
  crC_VALOR_DESCONTO: number;
  crC_VALOR_LIQUIDO: number;
  clientes: Cliente;

  carrinhO_COMPRAS_ITEM: CarrinhoComprasItem[]
}

