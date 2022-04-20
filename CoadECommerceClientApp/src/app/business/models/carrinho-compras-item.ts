import { ProdutoComposicao } from './produto-composicao';
import { InfoFatura } from './InfoFatura';

export class CarrinhoComprasItem {

  crC_ID: number;
  cmP_ID: number;
  ccI_QTD: number;
  ccI_VALOR_UNITARIO: number;
  ccI_VALOR_TOTAL: number;
  data_criacao: Date;
  produtO_COMPOSICAO: ProdutoComposicao;
  ifF_ID: number;
  infoFatura: InfoFatura;
}

