import { ProdutoComposicao } from "./produto-composicao";

export class CarrinhoComprasItem {

  crC_ID: number;
  cmP_ID: number;
  ccI_QTD: number;
  ccI_VALOR_UNITARIO: number;
  ccI_VALOR_TOTAL: number;
  data_criacao: Date;
  produtO_COMPOSICAO: ProdutoComposicao;
}

