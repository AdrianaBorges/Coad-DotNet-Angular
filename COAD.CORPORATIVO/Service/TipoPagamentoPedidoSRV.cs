using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;

namespace COAD.CORPORATIVO.Service
{
    public enum TipoPagamentoBase 
    {
        Boleto = 0,
        Cartao = 1,
        Cheque = 2
    }

    public enum TipoPagamentoCoorporativoEnum
    {
        Boleto = 7,
        Cartao = 9,
        Cheque = 8
    }
    public class TipoPagamentoSRV : ServiceAdapter<TIPO_PAGAMENTO, TipoPagamentoDTO, int>
    {
        private TipoPagamentoDAO _dao { get; set; }

        public TipoPagamentoSRV()
        {
            this._dao = new TipoPagamentoDAO();
            SetDao(_dao);
        }

        public TipoPagamentoSRV(TipoPagamentoDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public TipoPagamentoDTO BuscarTipoPagTabelaPreco(int? _TPG_ID, int? _TP_ID, int? _CMP_ID)
        {
            return _dao.BuscarTipoPagTabelaPreco(_TPG_ID, _TP_ID, _CMP_ID);
        }

        public IList<TipoPagamentoDTO> BuscarTiposDePagamentoAtivos()
        {
            return _dao.BuscarTiposDePagamentoAtivos();
        }

        public IList<TipoPagamentoDTO> ListarTipoPagamento(int tipoPagamento)
        {
            return _dao.ListarTipoPagamento(tipoPagamento);
        }

        public IList<TipoPagamentoDTO> ListarTipoPagamentoSimples()
        {
            return ListarTipoPagamento(0);
        }

        /// <summary>
        /// Lista todos os tipos de pagamento e, em tipos de pagamentos compostos, traz em uma lista
        /// dentro do DTO tipos de pagamentos singulares onde o tipo de pagamento 
        /// representa. Ex: (Boleto + Cheque).
        /// A lista seria Boleto e Cheque.
        /// </summary>
        /// <returns></returns>
        public IList<TipoPagamentoDTO> ListarTipoPagamentoCompostos()
        {
            var lstPagamento = BuscarTiposDePagamentoAtivos();
            PreencherTiposDePagamentosNoTipoPagamentoComposto(lstPagamento);

            return lstPagamento;

        }

        public override TipoPagamentoDTO FindByIdFullLoaded(params object[] id)
        {
            var obj = FindById(id);

            if(obj != null)
                PreencherTiposDePagamentosNoTipoPagamentoComposto(obj);
            return obj;
        }

        public void PreencherTiposDePagamentosNoTipoPagamentoComposto(TipoPagamentoDTO tipoPagamento)
        {
            PreencherTiposDePagamentosNoTipoPagamentoComposto(new HashSet<TipoPagamentoDTO>() { tipoPagamento});
        }

        public void PreencherTiposDePagamentosNoTipoPagamentoComposto(IEnumerable<TipoPagamentoDTO> lstTipoPagamento)
        {
            if (lstTipoPagamento != null)
            {
                var lstTiposCompostos = lstTipoPagamento.Where(x => x.TPG_TIPO == 1); // pega todos os compostos

                foreach (var tipoPag in lstTiposCompostos)
                {
                    tipoPag.ListaTipoPagamento.Clear(); // limpa
                    AdicionarTipoPagamento(tipoPag);
                }
            }                 
        }

        public IList<TipoPagamentoDTO> BuscarTipoPagamentoDaComposicao(int? tpgId)
        {
            return _dao.BuscarTipoPagamentoDaComposicao(tpgId);
        }

        private void AdicionarTipoPagamento(TipoPagamentoDTO tipoPag)
        {
            if (tipoPag != null)
            {
                var tpgId = tipoPag.TPG_ID;
                var lstTipoPedidosSimples = BuscarTipoPagamentoDaComposicao(tpgId);
                tipoPag.ListaTipoPagamento = lstTipoPedidosSimples;
            }
        }

        public TipoPagamentoDTO BuscarTipoPagamentoPorProposta(int? ppiId)
        {
            return _dao.BuscarTipoPagamentoPorProposta(ppiId);
        }
    }
}
