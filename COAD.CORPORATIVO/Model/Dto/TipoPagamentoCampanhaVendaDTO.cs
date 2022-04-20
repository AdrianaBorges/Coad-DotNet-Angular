using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{

    [Mapping(typeof(TIPO_PAGAMENTO_CAMPANHA_VENDA))]
    public class TipoPagamentoCampanhaVendaDTO
    {
        public int? TPG_ID { get; set; }
        public int? CVE_ID { get; set; }
        public Nullable<System.DateTime> DATA_INCLUSAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual CampanhaVendaDTO CAMPANHA_VENDA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }
    }
}
