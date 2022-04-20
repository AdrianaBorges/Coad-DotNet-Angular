using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.FISCAL.Model.Enumerados;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(TIPO_PAGAMENTO))]
    public class TipoPagamentoDTO
    {
        public TipoPagamentoDTO()
        {
            this.PEDIDO_PAGAMENTO = new HashSet<PedidoPagamentoDTO>();
            this.TABELA_PRECO_TIPO_PAGAMENTO = new HashSet<TabelaPrecoTipoPagamentoDTO>();
            this.ITEM_PEDIDO_TIPO_PAGAMENTO = new HashSet<ItemPedidoPedidoPagamentoDTO>();
            this.ListaTipoPagamento = new HashSet<TipoPagamentoDTO>();
            this.TIPO_PAGAMENTO_COMPOSICAO = new HashSet<TipoPagamentoComposicaoDTO>();
            this.TIPO_PAGAMENTO_COMPOSICAO1 = new HashSet<TipoPagamentoComposicaoDTO>();
            this.RETORNO_TRANSACAO = new HashSet<RetornoTransacaoDTO>();
            this.PROPOSTA_ITEM_COMPROVANTE = new HashSet<PropostaItemComprovanteDTO>();
            this.TIPO_PAGAMENTO_CAMPANHA_VENDA = new HashSet<TipoPagamentoCampanhaVendaDTO>();

        }
    
        public int? TPG_ID { get; set; }
        public string TPG_DESCRICAO { get; set; }
        public int TPG_ATIVO { get; set; }
        public int TPG_TIPO { get; set; }
        public Nullable<int> TPG_BOLETO { get; set; }
        public Nullable<int> TPG_CARTAO { get; set; }
        public Nullable<int> TPG_CHEQUE { get; set; }
        public string DLI_SIGLA { get; set; }
        public Nullable<bool> TPG_PARCELAVEL_NA_ENTRADA { get; set; }

        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoPagamentoDTO> PEDIDO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<TabelaPrecoTipoPagamentoDTO> TABELA_PRECO_TIPO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ItemPedidoPedidoPagamentoDTO> ITEM_PEDIDO_TIPO_PAGAMENTO { get; set; }

        /// <summary>
        /// Essa relação não existe no banco.
        /// É usado para determinar quais tipos de pagamentos um tipo de pagamento composto representa
        /// </summary>
        public virtual ICollection<TipoPagamentoDTO> ListaTipoPagamento { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<TipoPagamentoComposicaoDTO> TIPO_PAGAMENTO_COMPOSICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<TipoPagamentoComposicaoDTO> TIPO_PAGAMENTO_COMPOSICAO1 { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RetornoTransacaoDTO> RETORNO_TRANSACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaItemComprovanteDTO> PROPOSTA_ITEM_COMPROVANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<TipoPagamentoCampanhaVendaDTO> TIPO_PAGAMENTO_CAMPANHA_VENDA { get; set; }

        /// <summary>
        /// Não existe no banco.
        /// </summary>
        public int? CodigoPagamento { 
            get {

                if (ListaTipoPagamento != null && ListaTipoPagamento.Count() > 0)
                {
                    var tipoPagamento = ListaTipoPagamento.FirstOrDefault();
                    return tipoPagamento.TPG_ID;
                }
                else
                {
                    return TPG_ID;
                }
                
            }
            private set { }
        }

        public string NomeTipoPagamento
        {
            get
            {
                if (ListaTipoPagamento != null && ListaTipoPagamento.Count() > 0)
                {
                    var tipoPagamento = ListaTipoPagamento.FirstOrDefault();
                    return tipoPagamento.TPG_DESCRICAO;
                }
                else
                {
                    return TPG_DESCRICAO;
                }

            }
            private set { }
        }

        public TipoPagamentoEnum TipoPagamento {
            get {

                switch (TPG_ID)
                {
                    case 7: return TipoPagamentoEnum.BOLETO_BANCARIO;
                    case 8: return TipoPagamentoEnum.CHEQUE;
                    case 9: return TipoPagamentoEnum.CARTAO_CREDITO;
                    case 10: return TipoPagamentoEnum.OUTROS;
                }

                return TipoPagamentoEnum.OUTROS;
            }
        }
    }
}
