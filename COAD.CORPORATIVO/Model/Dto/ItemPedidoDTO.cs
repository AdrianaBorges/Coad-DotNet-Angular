
using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(ITEM_PEDIDO))]
    public class ItemPedidoDTO : IPedidoItem
    {
        public ItemPedidoDTO()
        {
            this.ITEM_PEDIDO_PEDIDO_PAGAMENTO = new HashSet<ItemPedidoPedidoPagamentoDTO>();
            this.PEDIDO_PARTICIPANTE = new HashSet<PedidoParticipanteDTO>();
            this.CONTRATOS = new HashSet<ContratoDTO>();
            this.HISTORICO_ATENDIMENTO = new HashSet<HistoricoAtendimentoDTO>();
            this.RETORNO_TRANSACAO = new HashSet<RetornoTransacaoDTO>();
            this.HISTORICO_PEDIDO = new HashSet<HistoricoPedidoDTO>();
            this.NomeTipo = "Item de Pedido";
            this.REGISTRO_FATURAMENTO = new HashSet<RegistroFaturamentoDTO>();
            this.PROPOSTA_ITEM_COMPROVANTE = new HashSet<PropostaItemComprovanteDTO>();
            this.IPE_GERA_NOTA = true;
            this.NOTA_FISCAL_LOTE_ITEM = new HashSet<NotaFiscalLoteItemDTO>();
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
        }

        public int? IPE_ID { get; set; }
        public string IPE_OBSERVACAO { get; set; }
        public Nullable<int> PED_CRM_ID { get; set; }
        public Nullable<int> CMP_ID { get; set; }
        public Nullable<int> IPE_QTD { get; set; }
        public Nullable<decimal> IPE_VALOR_PRODUTO { get; set; }

        [Range(0.01, int.MaxValue, ErrorMessage = "A porcentagem de desconto ser maior que zero")]
        public Nullable<decimal> IPE_DESCONTO { get; set; }
        public Nullable<int> TP_ID { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public string IPE_COD { get; set; }
        public Nullable<int> IPE_PARCELA { get; set; }
        public Nullable<decimal> IPE_VALOR_PARCELA { get; set; }
        public Nullable<decimal> IPE_TOTAL { get; set; }
        public Nullable<decimal> IPE_PRECO_UNITARIO { get; set; }    
        public Nullable<int> PROSP_ID { get; set; }
        public Nullable<int> PST_ID { get; set; }
        public Nullable<int> IFF_ID { get; set; }
        public Nullable<decimal> IPE_TOTAL_SEM_IMPOSTO { get; set; }
        public string IPE_LOGIN_APROVOU_DESCONTO { get; set; }
        public string IPE_LOGIN_ALTEROU_STATUS { get; set; }
        public Nullable<int> TTP_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string IPE_COD_LEGADO { get; set; }
        public string IPE_PATH_NFE { get; set; }
        public string ORDER_KEY { get; set; }
        public string ORDER_KEY_REF { get; set; }
        public string AUTHORIZATION_CODE { get; set; }
        public Nullable<short> IPE_DIA_VENCIMENTO_VENDA_RECORRENTE { get; set; }
        public Nullable<bool> IPE_PRIMEIRA_PARCELA_CORTERIA { get; set; }
        public Nullable<bool> IPE_PAGAMENTO_GATEWAY { get; set; }
        public Nullable<bool> IPE_POSSUI_ENTRADA { get; set; }
        public Nullable<decimal> IPE_VALOR_ENTRADA { get; set; }
        public Nullable<int> PPI_ID { get; set; }
        public Nullable<bool> IPE_ACESSOS_CONCEDIDOS { get; set; }
        public Nullable<int> IPE_QTD_CONSULTA { get; set; }
        public Nullable<DateTime> IPE_DATA_VALIDADE { get; set; }
        public Nullable<bool> IPE_CORTESIA { get; set; }
        public Nullable<System.DateTime> IPE_DATA_FATURAMENTO { get; set; }
        public Nullable<System.DateTime> IPE_DATA_FATURAMENTO_SEMANA_FAT { get; set; }
        public string IPE_PERIODO_FAT { get; set; }
        public string IPE_SEMANA_FAT { get; set; }
        public Nullable<System.DateTime> IPE_DATA_FATURAMENTO_ALTERADA { get; set; }
        public Nullable<int> IPE_PERIODO_MES_BONUS { get; set; }
        public Nullable<System.DateTime> IPE_DATA_PRODUCAO { get; set; }
        public Nullable<int> TPG_ID { get; set; }

        public Nullable<bool> IPE_GERA_NOTA { get; set; }

        /// <summary>
        /// Código da Assinatura que será Cancelada.
        /// </summary>
        public string IPE_ASN_NUM_ASS_CANC { get; set; }
        public Nullable<int> IFF_ID_ENTRADA { get; set; }
        public Nullable<decimal> IPE_VALOR_BRUTO_ENTRADA { get; set; }

        public decimal? TotalComDescontoSemImposto {
            get {
                decimal? total = IPE_PRECO_UNITARIO * IPE_QTD;
                decimal porcentagemDesconto = (decimal) ((IPE_DESCONTO != null) ? IPE_DESCONTO : 0);
                if (total != null)
                {
                    total = (total - ((decimal)(porcentagemDesconto / 100) * total));
                    return total;
                }

                return 0.00m;
            }            
            set { } 
        }

        public decimal? DescontoSemImposto
        {
            get
            {
                decimal? total = IPE_PRECO_UNITARIO * IPE_QTD;
                decimal porcentagemDesconto = (decimal)((IPE_DESCONTO != null) ? IPE_DESCONTO : 0);

                if (total != null)
                {
                    var desconto = ((decimal)(porcentagemDesconto / 100) * total);
                    return desconto;
                }

                return 0.00m;
            }
            set { }
        }

        /// <summary>
        /// Esse campo não existe no banco
        /// </summary>
        public virtual string UrlPagamento { get; set; }
        public virtual bool PodeUsarGateway { get; set; }
        public int? UEN_ID { get; set; }// campo virtual não existe no banco

        /// <summary>
        /// Esse campo não existe no banco
        /// </summary>
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataVencimentoSegParcela { get; set; }
        public Nullable<int> LOC_ID { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ItemPedidoPedidoPagamentoDTO> ITEM_PEDIDO_PEDIDO_PAGAMENTO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PedidoCRMDTO PEDIDO_CRM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoTabelaPrecoDTO REGIAO_TABELA_PRECO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PedidoStatusDTO PEDIDO_STATUS { get; set; }

        [RequiredListIf("PRODUTO_COMPOSICAO.EhCurso", 1, true, ErrorMessage = "Preencha os participantes")]
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoParticipanteDTO> PEDIDO_PARTICIPANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual InfoFaturaDTO INFO_FATURA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPeriodoDTO TIPO_PERIODO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ContratoDTO> CONTRATOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<HistoricoAtendimentoDTO> HISTORICO_ATENDIMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual AssinaturaDTO ASSINATURA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RetornoTransacaoDTO> RETORNO_TRANSACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<NfeXmlDTO> NFE_XML { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<HistoricoPedidoDTO> HISTORICO_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PropostaItemDTO PROPOSTA_ITEM { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RegistroFaturamentoDTO> REGISTRO_FATURAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaItemComprovanteDTO> PROPOSTA_ITEM_COMPROVANTE { get; set; }

        /// <summary>
        /// Assinatura de Cancelamento
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AssinaturaDTO ASSINATURA1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalLoteItemDTO> NOTA_FISCAL_LOTE_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual InfoFaturaDTO INFO_FATURA1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual LocalizacaoCursoDTO LOCALIZACAO_CURSO { get; set; }

        public bool? AcessosConcedidos
        {
            get
            {
                return this.IPE_ACESSOS_CONCEDIDOS;
            }
            set
            {
                this.IPE_ACESSOS_CONCEDIDOS = value;
            }
        }

        public int? PstId
        {
            get
            {
                return this.PST_ID;
            }
            set
            {
                this.PST_ID = value;
            }
        }

        public int? CmpId
        {
            get
            {
                return this.CMP_ID;
            }
            set
            {
                this.CMP_ID = value;
            }
        }


        public string NomeTipo { get; set; }

        public ICollection<PropostaItemComprovanteDTO> Comprovantes
        {
            get
            {
                return this.PROPOSTA_ITEM_COMPROVANTE;
            }
            set
            {
                this.PROPOSTA_ITEM_COMPROVANTE = value;
            }
        }

        public ICollection<PedidoParticipanteDTO> Participantes { get => PEDIDO_PARTICIPANTE; set => PEDIDO_PARTICIPANTE = value; }
        public ProdutoComposicaoDTO ProdutoComposicao { get => PRODUTO_COMPOSICAO; set => PRODUTO_COMPOSICAO = value; }
        public decimal? ValorUnitario { get => IPE_PRECO_UNITARIO; }
        public int? Qtd { get => IPE_QTD; }
        public decimal? ValorEntradaBruto { get => IPE_VALOR_BRUTO_ENTRADA; }
        public decimal? ValorEntrada { get => IPE_VALOR_ENTRADA; }
        public decimal? ValorParcelaBruto { get => null; }
        public decimal? ValorDaParcela { get => IPE_VALOR_PARCELA; }
        public int? QtdParcelas { get => IPE_PARCELA; }

        public Nullable<int> MPG_ASN_ID { get; set; }
        /*
        */
    }
}
