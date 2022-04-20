using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(TABELA_PRECO))]
    public class TabelaPrecoDTO
    {
        public TabelaPrecoDTO()
        {
            this.REGIAO_TABELA_PRECO = new HashSet<RegiaoTabelaPrecoDTO>();
            this.TABELA_PRECO_TIPO_PAGAMENTO = new HashSet<TabelaPrecoTipoPagamentoDTO>();
        }

        public int? TP_ID { get; set; }

        [Required(ErrorMessage = "O produto não foi encontrado na tabela de preço.")]
        public int? CMP_ID { get; set; }

        [Required(ErrorMessage = "Digite a margem de negociação")]
        [Range(1, 1000, ErrorMessage = "A porcentagem mínima da margem de negociação deve ser 1%")]
        public Nullable<int> TP_MARGEM_NEGOCIACAO { get; set; }

        [Required(ErrorMessage = "Digite a margem de lucro")]
        [Range(1, 1000, ErrorMessage = "A porcentagem mínima da margem de lucro deve ser 1%")]
        public Nullable<int> TP_MARGEM_LUCRO { get; set; }

        [Required(ErrorMessage = "Digite o preço de venda")]
        [Range(0.05, Double.MaxValue, ErrorMessage = "O valor mínimo de venda deve ser R$ 0,05")]
        public Nullable<decimal> TP_PRECO_VENDA { get; set; }

        [Range(1, 100, ErrorMessage = "A quantidade mínima de parcela é de 1x")]
        public Nullable<int> TP_NUM_PARCELAS_MIN { get; set; }

        [Range(1, 100, ErrorMessage = "A quantidade mínima de parcela é de 1x")]
        public Nullable<int> TP_NUM_PARCELAS_MAX { get; set; }
        public string TP_DESCRICAO { get; set; }

        public Nullable<decimal> TP_PORCENTAGEM_SERVICO { get; set; }
        public Nullable<bool> TP_PERMITIR_CORTESIA_PRIMEIRA_PARCELA { get; set; }

        public Nullable<System.DateTime> TP_DATA_EXCLUSAO { get; set; }

        public Nullable<int> TTP_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<RegiaoTabelaPrecoDTO> REGIAO_TABELA_PRECO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<TabelaPrecoTipoPagamentoDTO> TABELA_PRECO_TIPO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPeriodoDTO TIPO_PERIODO { get; set; }
    }
}
