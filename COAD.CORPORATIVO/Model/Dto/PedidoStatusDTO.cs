using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PEDIDO_STATUS))]
    public class PedidoStatusDTO
    {
        public PedidoStatusDTO()
        {
            this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();
            this.ITEM_PEDIDO = new HashSet<ItemPedidoDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>(); 
            this.HISTORICO_PEDIDO = new HashSet<HistoricoPedidoDTO>();
            
        }

        public int PST_ID { get; set; }
        public string PST_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<HistoricoPedidoDTO> HISTORICO_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }
    }
}
