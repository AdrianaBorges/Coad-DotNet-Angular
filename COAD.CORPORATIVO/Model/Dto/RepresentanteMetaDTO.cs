using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class RepresentanteMetaDTO
    {
        public int REP_ID { get; set; }
        public string REP_NOME { get; set; }
        public int TCO_ID { get; set; }
        public int RME_MES { get; set; }
        public int RME_ANO { get; set; }
        public Nullable<decimal> RME_VLR_META { get; set; }
        public Nullable<decimal> SER_PREMIO_MIN { get; set; }
        public Nullable<decimal> SER_PREMIO_MAX { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual TipoComissaoDTO TIPO_COMISSAO { get; set; }
    }
}
