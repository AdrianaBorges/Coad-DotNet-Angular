using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(NFE_XML))]
    public class NfeXmlDTO
    {
        public NfeXmlDTO()
        {
            this.CONTRATOS = new HashSet<ContratoDTO>();
        }

        public int NFX_ID { get; set; }
        public string NFX_CHAVE_NOTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public string NFX_PATH_NOTA { get; set; }

        public Nullable<int> IPE_ID { get; set; }
        public Nullable<int> NFX_TIPO { get; set; }
        public Nullable<int> NFX_NUMERO_NOTA { get; set; }
        public Nullable<System.DateTime> NFX_DATA_EMI_NOTA { get; set; }
        public Nullable<System.DateTime> NFX_DATA_EXCLUSAO { get; set; }
        public Nullable<bool> NFX_NUM_EXTORNADO { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ContratoDTO> CONTRATOS { get; set; }

    }
}
