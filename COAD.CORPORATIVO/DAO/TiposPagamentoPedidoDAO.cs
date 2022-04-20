using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class TipoPagamentoDAO : DAOAdapter<TIPO_PAGAMENTO, TipoPagamentoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public TipoPagamentoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public TipoPagamentoDTO BuscarTipoPagTabelaPreco(int? _TPG_ID, int? _TP_ID, int? _CMP_ID)
        {
           // 7 - Boleto
           // 9 - Cartão

            var query = (from a in db.TABELA_PRECO_TIPO_PAGAMENTO
                         join t in db.TABELA_PRECO on a.TP_ID equals t.TP_ID
                         join p in db.TIPO_PAGAMENTO on a.TPG_ID equals p.TPG_ID
                         where p.TPG_ID == _TPG_ID &&
                               a.TP_ID == _TP_ID &&
                               t.CMP_ID == _CMP_ID
                         select p).FirstOrDefault();

            return ToDTO(query);
        }

        public IList<TipoPagamentoDTO> ListarTipoPagamento(int tipoPagamento)
        {
            var listaTiposDePagamentoAtivo = (from x in db.TIPO_PAGAMENTO where x.TPG_ATIVO == 1 && x.TPG_TIPO == tipoPagamento select x);
            return ToDTO(listaTiposDePagamentoAtivo);
        }

        public IList<TipoPagamentoDTO> BuscarTiposDePagamentoAtivos()
        {
            var listaTiposDePagamentoAtivo = (from x in db.TIPO_PAGAMENTO
                                              where 
                                                x.DATA_EXCLUSAO == null
                                              select x);
            return ToDTO(listaTiposDePagamentoAtivo);
        }

        public IList<TipoPagamentoDTO> BuscarTipoPagamentoDaComposicao(int? tpgId)
        {
            var query = (from tpg in db.TIPO_PAGAMENTO_COMPOSICAO 
                         where tpg.TPG_ID_PAI == tpgId 
                         orderby tpg.TPC_ORDEM 
                         select tpg.TIPO_PAGAMENTO1);

            return ToDTO(query);
        }

        public TipoPagamentoDTO BuscarTipoPagamentoPorProposta(int? ppiId)
        {
            var query = (from proItm in db.PROPOSTA_ITEM
                             where proItm.PPI_ID == ppiId
                             select proItm.TIPO_PAGAMENTO)
                             .FirstOrDefault();

            return ToDTO(query);
        }
    }
}
