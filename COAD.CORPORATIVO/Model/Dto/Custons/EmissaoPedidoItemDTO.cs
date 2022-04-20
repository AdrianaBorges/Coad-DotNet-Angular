using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Coad.GenericCrud.Validations;
using GenericCrud.Validations;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class EmissaoPedidoItemDTO
    {
        
        [Required(ErrorMessage = "Informe a quantidade")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ter no mínimo 1.")]
        public int? QTD { get; set; }

        public string PRT_IDENTIFICACAO_TURMA { get; set; }

        //[RangeIf(, 1, int.MaxValue, ErrorMessage = "A quantidade de parcelas deve ser no mínimo 1.")]
        [RangeIf(1, 9999, "EhCortesia", false)]
        public int? QTD_PARCELAS {get; set;}

        [Required(ErrorMessage = "Informe o valor unitário")]
        public decimal? VALOR_UNITARIO { get; set; }

        public decimal? VALOR_ENTRADA { get; set; }

        [Range(0.01, int.MaxValue, ErrorMessage = "A quantidade de parcela deve ser maior que zero")]
        public decimal? PORCENTAGEM_DESCONTO { get; set; }


        [RequiredIf("EhCortesia", false, ErrorMessage = "Informe o valor Total")]
        [Range(0, int.MaxValue, ErrorMessage = "O total não pode ser menor que 0.")]
        
        public decimal? VALOR_TOTAL { get; set; }

        public decimal? VALOR_PARCELAS { get; set; }
        public bool? ACESSOS_CONCEDIDOS { get; set; }
        
        [RequiredIf("EhCortesia", false, ErrorMessage = "Informe o tipo de pagamento")]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }

        [Required(ErrorMessage = "Informe o produto")]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }

        [RequiredIfNullOrEmpty("TTP_ID", ErrorMessage = "Selecione a tabela de preço")]
        public virtual RegiaoTabelaPrecoDTO REGIAO_TABELA_PRECO { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataVencimentoSegparcela { get; set; }
        
        public Nullable<System.DateTime> DATA_PARA_FATURAMENTO { get; set; }
        public string PERIODO_FAT { get; set; }
        public string SEMANA_FAT { get; set; }

        public int? RG_ID { get; set; }
        public int? TTP_ID { get; set; }
        public int? PPI_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public int? QuantidadeConsulta { get; set; }
        public int? PERIODO_MES_BONUS { get; set; }
        public string CodigoAssinaturaCanc { get; set; }
        public bool? GeraNotaFiscal { get; set; }
        public ICollection<PedidoParticipanteDTO> PEDIDO_PARTICIPANTE { get; set; }

        public Nullable<bool> Cortesia { get; set; }
        
        public bool EhCortesia
        {
            get
            {
                return (Cortesia == true);
            }
        }
        public int? IFF_ID { get; set; }
        public int? IFF_ID_ENTRADA { get; set; }
        public int? LOC_ID { get; set; }
    }
}
