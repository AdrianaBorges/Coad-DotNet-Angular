using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ApuracaoPremiacaoSemanaDTO
    {
        public int REP_ID { get; set; }
        public string REP_NOME { get; set; }
        public int APU_MES { get; set; }
        public int APU_ANO { get; set; }
        public int APU_SEMANA_FAT { get; set; }
        public Nullable<decimal> APU_VLR_PREMIO { get; set; }
        public Nullable<int> APU_QTDE_CONTRATOS { get; set; }
        public Nullable<decimal> APU_VLR_CONTRATOS { get; set; }
        public Nullable<decimal> APU_VLR_META { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
    }
}
