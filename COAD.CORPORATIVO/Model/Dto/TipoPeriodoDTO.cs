using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(TIPO_PERIODO))]
    public class TipoPeriodoDTO
    {
        //
        public TipoPeriodoDTO()
        {
            this.PRODUTO_COMPOSICAO_ITEM = new HashSet<ProdutoComposicaoItemDTO>();
            this.PRODUTO_COMPOSICAO_TIPO_PERIODO = new HashSet<ProdutoComposicaoTipoPeriodoDTO>();
            this.ITEM_PEDIDO = new HashSet<ItemPedidoDTO>();
            this.CONTRATOS = new HashSet<ContratoDTO>();
        }
    
        public int? TTP_ID { get; set; }
        public string TTP_DESCRICAO { get; set; }
        public Nullable<int> TTP_QTDE_DIAS { get; set; }
        public Nullable<int> TTP_QTD_MESES { get; set; }
        public bool TTP_RECORRENTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ProdutoComposicaoTipoPeriodoDTO> PRODUTO_COMPOSICAO_TIPO_PERIODO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ProdutoComposicaoItemDTO> PRODUTO_COMPOSICAO_ITEM { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ContratoDTO> CONTRATOS { get; set; }

    }

}
