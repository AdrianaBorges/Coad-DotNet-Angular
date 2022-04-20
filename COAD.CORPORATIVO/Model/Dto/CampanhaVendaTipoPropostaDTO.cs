using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(CAMPANHA_VENDA_TIPO_PROPOSTA))]
    public class CampanhaVendaTipoPropostaDTO
    {
        public int? CVE_ID { get; set; }
        public int? TPP_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual CampanhaVendaDTO CAMPANHA_VENDA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPropostaDTO TIPO_PROPOSTA { get; set; }
    }
}
