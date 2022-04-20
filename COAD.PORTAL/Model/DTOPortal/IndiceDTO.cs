using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTOPortal
{
    public class IndiceDTO
    {
        public int idc_agregado_id { get; set; }
        public string idc_agregado_nome { get; set; }
        public string idc_agregado_valor { get; set; }
        public string idc_agregado_data { get; set; }
        public Nullable<int> idc_agregado_orientacao { get; set; }
        public Nullable<int> idc_agregado_ordem { get; set; }
    }
}
