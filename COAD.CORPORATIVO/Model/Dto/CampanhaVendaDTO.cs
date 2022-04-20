using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Validations;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(CAMPANHA_VENDA))]
    public class CampanhaVendaDTO
    {
        public CampanhaVendaDTO()
        {
            this.CAMPANHA_VENDA_TIPO_PROPOSTA = new HashSet<CampanhaVendaTipoPropostaDTO>();
            this.PROPOSTA_ITEM = new HashSet<PropostaItemDTO>();
            this.TIPO_PAGAMENTO_CAMPANHA_VENDA = new HashSet<TipoPagamentoCampanhaVendaDTO>();

            this.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO = new HashSet<CampanhaVendasProdutoComposicaoDTO>();
        }

        public int? CVE_ID { get; set; }

        [Required(ErrorMessage = "Informe o Data Inicial de Validade")]
        public Nullable<System.DateTime> CVE_PERIODO_INICIAL { get; set; }

        [Required(ErrorMessage = "Informe o Data Inicial de Validade")]
        public Nullable<System.DateTime> CVE_PERIODO_FINAL { get; set; }
        public string CVE_DESCRICAO { get; set; }
        public Nullable<int> CVE_DIAS_MIN_PRIMEIRA_PARCELA { get; set; }
        public Nullable<int> CVE_DIAS_MIN_SEGUNDA_PARCELA { get; set; }

        [DescontoMaximoCampanha]
        public Nullable<int> CVE_DESCONTO_MAX { get; set; }
        public Nullable<decimal> CVE_DESCONTO_FIXO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> CVE_ACRESCIMO_MINIMO { get; set; }
        public Nullable<int> CVE_DIAS_MAX_PRIMEIRA_PARCELA { get; set; }
        public Nullable<int> CVE_DIAS_FIXO_SEGUNDA_PARCELA { get; set; }
        public Nullable<int> CVE_NUM_PARCELA_MIN { get; set; }
        public Nullable<int> CVE_NUM_PARCELA_MAX { get; set; }
        public Nullable<bool> CVE_CAMPANHA_ATIVA { get; set; }
        

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CampanhaVendaTipoPropostaDTO> CAMPANHA_VENDA_TIPO_PROPOSTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaItemDTO> PROPOSTA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<TipoPagamentoCampanhaVendaDTO> TIPO_PAGAMENTO_CAMPANHA_VENDA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CampanhaVendasProdutoComposicaoDTO> CAMPANHA_VENDAS_PRODUTO_COMPOSICAO { get; set; }


    }
}
