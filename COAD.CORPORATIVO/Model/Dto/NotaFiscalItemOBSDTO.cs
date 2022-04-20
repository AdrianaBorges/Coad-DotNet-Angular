using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class NotaFiscalItemOBSDTO
    {
        public int NF_TIPO { get; set; }
        public int NF_NUMERO { get; set; }
        public string NF_SERIE { get; set; }
        public int PRO_ID { get; set; }
        public string NFO_OBS { get; set; }
        public int NFO_ID { get; set; }

        public virtual NotaFiscalItemDTO NOTA_FISCAL_ITEM { get; set; }
    }
}
