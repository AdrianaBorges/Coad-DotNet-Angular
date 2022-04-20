using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(PRODUTO_COMPOSICAO_ITEM))]
    public class ProdutoComposicaoItemDTO
    {

        public int? CMP_ID { get; set; }

        [DisplayName("Produto")]
        [Required(ErrorMessage = "Selecione o Produto")]
        public int? PRO_ID { get; set; }

        [DisplayName("Quantidade")]
        //[Required(ErrorMessage = "Digite a quantidade")]
        public Nullable<int> CMI_QTDE { get; set; }

        [DisplayName("Preço Unitário")]
        //[Required(ErrorMessage = "Digite o preço unitário")]
        public Nullable<decimal> CMI_PRECO_UNIT { get; set; }

        //[DisplayName("Preço Total")]
        //[Required(ErrorMessage = "Digite o preço total")]
        public string CMI_PRECO_TOTAL { get; set; }

        [DisplayName("Nº de envio/consultas p/ periodo")]
        //[Required(ErrorMessage = "Digite a quantidade de envio/consultas")]
        public Nullable<int> CMI_QTDE_PERIODO { get; set; }

        public Nullable<int> TTP_ID { get; set; }

        public bool CMI_GERA_ASSINATURA_LEGADO { get; set; }
        public string CMI_MUNDIPAGG_PLANO_ITEM_ID { get; set; }

        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutosDTO PRODUTOS { get; set; }
        public virtual TipoPeriodoDTO TIPO_PERIODO { get; set; }
    }
}
