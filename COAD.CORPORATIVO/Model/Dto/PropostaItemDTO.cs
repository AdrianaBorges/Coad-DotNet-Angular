using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.SessionUtils;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Validations;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(PROPOSTA_ITEM))]
    public class PropostaItemDTO : IPedidoItem
    {
        public PropostaItemDTO()
        {
            this.PEDIDO_PARTICIPANTE = new HashSet<PedidoParticipanteDTO>();
            this.PROPOSTA_ITEM_COMPROVANTE = new HashSet<PropostaItemComprovanteDTO>();
            this.PARCELAS = new HashSet<ParcelasDTO>();
            this.ITEM_PEDIDO = new HashSet<ItemPedidoDTO>();
            this.NomeTipo = "Proposta";
            this.PPI_GERA_NOTA = true;
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
            this.NOTA_FISCAL_LOTE_ITEM = new HashSet<NotaFiscalLoteItemDTO>();

        }
        
        public int? PPI_ID { get; set; }
        public Nullable<int> PRT_ID { get; set; }

        public Nullable<bool> PPI_CORTESIA { get; set; }
        public bool EhCortesia => (PPI_CORTESIA == true);

        [RequiredIf("RequerLocalidade", true, ErrorMessage = "Informe a Localidade")]
        public Nullable<int> LOC_ID { get; set; }

        [RequiredIf("RequerTurma", true, ErrorMessage = "Informe o nome da Turma")]
        public string PRT_IDENTIFICACAO_TURMA { get; set; }

        [Required(ErrorMessage = "Informe o valor unitário")]
        public Nullable<decimal> PPI_VALOR_UNITARIO { get; set; }        
        [Required(ErrorMessage = "Informe a quantidade")]
        [Range(1, int.MaxValue, ErrorMessage =  "A quantidade mínima é 1")]
        public Nullable<int> PPI_QTD { get; set; }

        [RangeIf(0.01, double.MaxValue, "EhCortesia", false, ErrorMessage = "O valor mínimo do total deve ser R$ 0,01")]
        public Nullable<decimal> PPI_TOTAL { get; set; }
        
        [Required(ErrorMessage = "Selecione um produto.")]
        public Nullable<int> CMP_ID { get; set; }
        
        public Nullable<int> PRO_ID { get; set; }
        public Nullable<decimal> PPI_DESCONTO { get; set; }
        public Nullable<int> CMP_ID_1 { get; set; }

        [RequiredIf("EhCortesia", false, ErrorMessage = "Selecione o Tipo de Pagamento")]
        public Nullable<int> TPG_ID { get; set; }

        public Nullable<int> PPI_QTD_PARCELAS { get; set; }

        [RangeIf(0.01, double.MaxValue, "RequerEntrada", true, ErrorMessage = "O valor mínimo de entrada deve ser R$ 0,01")]
        [RequiredIf("RequerEntrada", true, ErrorMessage = "O valor mínimo da entrada deve ser R$ 0,01")]
        public Nullable<decimal> PPI_VALOR_ENTRADA { get; set; }

        [RangeIf(0.01, double.MaxValue, "EhCortesia", false, ErrorMessage = "O valor mínimo da parcela deve ser R$ 0,01")]
        [RequiredIf("EhCortesia", false, ErrorMessage = "O valor mínimo da parcela deve ser R$ 0,01")]
        public Nullable<decimal> PPI_VALOR_PARCELA { get; set; }


        [RequiredIf("EntradaInValida", true, ErrorMessage = "Valor da parcela inválido. Uma venda á prazo não pode possuir uma entrada igual ao total.")]
        public Nullable<decimal> valorParcela
        {
            get
            {
                if (PPI_VALOR_PARCELA > 0)
                    return null;
                return PPI_VALOR_PARCELA;
            }
            set { }
        }

        [RequiredIf("EhCortesia", false, ErrorMessage = "Informe a Data de Vencimento")]
        public Nullable<System.DateTime> PPI_DATA_VENCIMENTO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> PST_ID { get; set; }
        public string COD_LEGADO { get; set; }
        public string PAR_OBSERVACOES { get; set; }
        public Nullable<bool> PPI_ACESSOS_CONCEDIDOS { get; set; }

        [RequiredIf("RequerEntrada", true, ErrorMessage = "Informe a Data de Vencimento da 2° parcela")]
        public Nullable<System.DateTime> PPI_DATA_VENCIMENTO_SEG_PARCELA { get; set; }

        public Nullable<int> PPI_QTD_CONSULTA { get; set; }

        public string ASN_NUM_ASSINATURA { get; set; }
        public Nullable<System.DateTime> PPI_DATA_PARA_FATURAMENTO { get; set; }
        public string PPI_PERIODO_FAT { get; set; }
        public string PPI_SEMANA_FAT { get; set; }

        public Nullable<int> PPI_PERIODO_MES_BONUS { get; set; }

       // [RequiredIf("PROPOSTA.TPP_ID", 3, ErrorMessage = "Informe a assinatura que será transferida.")]
        public string PPI_ASN_NUM_ASS_CANC { get; set; } // Código de Assinatura para Cancelar
        public Nullable<bool> PPI_GERA_NOTA { get; set; }
        public Nullable<int> RLI_ID { get; set; }
        public Nullable<decimal> PPI_VALOR_UNITARIO_PRODUTO { get; set; }
        // Não existe no banco
        public bool Nova { get; set; }
        // Não existe no banco
        public bool Alterada { get; set; }
        public Nullable<int> IFF_ID { get; set; }
        public Nullable<decimal> PPI_VALOR_BRUTO_PARCELA { get; set; }
        public Nullable<int> IFF_ID_ENTRADA { get; set; }
        public Nullable<decimal> PPI_VALOR_BRUTO_ENTRADA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoItemDTO PRODUTO_COMPOSICAO_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PropostaDTO PROPOSTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PedidoStatusDTO PEDIDO_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PedidoParticipanteDTO> PEDIDO_PARTICIPANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaItemComprovanteDTO> PROPOSTA_ITEM_COMPROVANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AssinaturaDTO ASSINATURA { get; set; }
        /// <summary>
        /// Código Da Assinatura á ser Cancelada
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AssinaturaDTO ASSINATURA1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual CampanhaVendaDTO CAMPANHA_VENDA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalLoteItemDTO> NOTA_FISCAL_LOTE_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual InfoFaturaDTO INFO_FATURA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual InfoFaturaDTO INFO_FATURA1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual LocalizacaoCursoDTO LOCALIZACAO_CURSO { get; set; }

        public Nullable<int> MPG_ASN_ID { get; set; }
        public bool? AcessosConcedidos
        {
            get
            {
                return this.PPI_ACESSOS_CONCEDIDOS;
            }
            set
            {
                this.PPI_ACESSOS_CONCEDIDOS = value;
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

        // ------------ Métodos de Validação
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

        public bool RequerEntrada {
            get {
                return (!EhCortesia && TIPO_PAGAMENTO != null && TIPO_PAGAMENTO.TPG_TIPO == 1);                   
            }
            set { }
        }

		public bool RequerTurma
        {
            get
            {
                if (SessionUtil.GetUenId().Equals(1))
                {
                    return true;
                }

                return false;
                
            }
        }

        public bool RequerLocalidade
        {
            get
            {
                if (SessionUtil.GetUenId().Equals(1))
                {
                    return true;
                }

                return false;

            }
        }
        
        public bool EntradaInValida
        {
            get
            {
                if (!EhCortesia && TIPO_PAGAMENTO != null && TIPO_PAGAMENTO.TPG_TIPO == 1){
                    if((PPI_TOTAL > 0 && PPI_TOTAL == PPI_VALOR_ENTRADA)){
                        if (PPI_VALOR_PARCELA == 0)
                            PPI_VALOR_PARCELA = null;
                        return true;
                    }
                }
                return false;
            }
            set { }
        }

        public ICollection<PedidoParticipanteDTO> Participantes { get => PEDIDO_PARTICIPANTE; set => PEDIDO_PARTICIPANTE = value; }
        public ProdutoComposicaoDTO ProdutoComposicao { get => PRODUTO_COMPOSICAO; set => PRODUTO_COMPOSICAO = value; }
        public decimal? ValorUnitario { get => PPI_VALOR_UNITARIO; }
        public int? Qtd { get => PPI_QTD; }
        public decimal? ValorEntradaBruto { get => PPI_VALOR_BRUTO_ENTRADA; }
        public decimal? ValorEntrada { get => PPI_VALOR_ENTRADA; }
        public decimal? ValorParcelaBruto { get => PPI_VALOR_BRUTO_PARCELA; }
        public decimal? ValorDaParcela { get => PPI_VALOR_PARCELA; }
        public int? QtdParcelas { get => PPI_QTD_PARCELAS; }
        /*
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual MUNDIPAGG_ASSINATURA MUNDIPAGG_ASSINATURA { get; set; } */
    }
}
