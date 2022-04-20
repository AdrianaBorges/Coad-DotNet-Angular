using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class GridCarteiramentoDTO
    {
        public string CAR_ID { get; set; }
        public string REGIAO_ID { get; set; }
        public string SEQ_REG { get; set; }
        public Nullable<int> AREA_ID { get; set; }
        public string REGIAO_UF { get; set; }
        public string REPRESENTANTE { get; set; }
    }
}
