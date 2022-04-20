using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(ITEM_PEDIDO_PEDIDO_PAGAMENTO))]
    public class ItemPedidoPedidoPagamentoDTO
    {
        public int? IPE_ID { get; set; }
        public int? PGT_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PedidoPagamentoDTO PEDIDO_PAGAMENTO { get; set; }
    }
}
