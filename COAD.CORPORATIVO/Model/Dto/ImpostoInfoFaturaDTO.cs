using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(IMPOSTO_INFO_FATURA))]
    public class ImpostoInfoFaturaDTO
    {
        public int? IMP_ID { get; set; }
        public int? IFF_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }
        public Nullable<decimal> IIF_VALOR_DESCONTO { get; set; }
        public Nullable<decimal> IIF_PERCENTUAL_DESCONTO { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ImpostoDTO IMPOSTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual InfoFaturaDTO INFO_FATURA { get; set; }
    }
}
