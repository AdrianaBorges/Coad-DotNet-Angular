using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("TP_ID", "TPG_ID")]
    public class TabelaPrecoTipoPagamentoSRV : ServiceAdapter<TABELA_PRECO_TIPO_PAGAMENTO, TabelaPrecoTipoPagamentoDTO, int>
    {
        public TabelaPrecoTipoPagamentoDAO _dao { get; set; }

        public TabelaPrecoTipoPagamentoSRV()
        {
            this._dao = new TabelaPrecoTipoPagamentoDAO();
            SetDao(_dao);
        }

        public TabelaPrecoTipoPagamentoSRV(TabelaPrecoTipoPagamentoDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }


        public void SalvarEExcluirTabelaPrecoTipoPagamento(TabelaPrecoDTO tabPreco)
        {
            if (tabPreco != null)
            {
                var tabPrecoTipoPagamento = tabPreco.TABELA_PRECO_TIPO_PAGAMENTO;

                if(tabPrecoTipoPagamento != null)
                {
                    CheckAndAssignKeyFromParentToChildsList(tabPreco, tabPrecoTipoPagamento, "TP_ID");
                    ServiceFactory.RetornarServico<TabelaPrecoSRV>().ExcluirPropertyList(tabPreco, "tabPrecoTpPag");
                    SaveOrUpdateNonIdentityKeyEntity(tabPrecoTipoPagamento);
                }
           }
        }

    }
}
