using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class SemanaPremiacaoReprDTO
    {
        public int SPR_SEMANA { get; set; }
        public System.DateTime SPR_DATA_INI { get; set; }
        public System.DateTime SPR_DATA_FIM { get; set; }
        public int REP_ID { get; set; }
        public string REP_NOME { get; set; }

        public Nullable<decimal> SER_VLR_META { get; set; }
        public Nullable<decimal> SER_VLR_PREMIO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual SemanaPremiacaoDTO SEMANA_PREMIACAO { get; set; }
    }
}
