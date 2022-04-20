using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public enum TipoDePedidoEnum
    {
        VENDA_NOVA = 1,
        RENOVACAO = 2,
        MIGRACAO = 3
    }

    [Mapping(typeof(TIPO_PEDIDO_CRM))]
    public class TipoPedidoCRMDTO
    {
        public TipoPedidoCRMDTO()
        {
            this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();
        }
        
        public int TPD_ID { get; set; }
        public string TPD_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }
    }
}
