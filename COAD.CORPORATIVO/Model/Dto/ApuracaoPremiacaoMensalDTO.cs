using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ApuracaoPremiacaoMensalDTO
    {
        public int REP_ID { get; set; }
        public string REP_NOME { get; set; }
        public int APU_MES { get; set; }
        public int APU_ANO { get; set; }
        public Nullable<decimal> APU_VLR_RENOVACAO { get; set; }
        public Nullable<decimal> APU_VLR_VENDA_NOVA { get; set; }
        public Nullable<decimal> APU_VLR_PRODUTOS { get; set; }
        public Nullable<decimal> APU_VLR_AVISTA { get; set; }
        public Nullable<decimal> APU_VLR_4PARCELAS { get; set; }
        public Nullable<decimal> APU_PERC_4PARCELAS { get; set; }
        public Nullable<decimal> APU_VLR_TOTAL { get; set; }
        public Nullable<decimal> RME_VLR_META { get; set; }
        public Nullable<decimal> SER_VLR_PREMIO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
    }
}
