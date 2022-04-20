using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class CadernoCompartilhadoDTO
    {
        public int CAD_ID { get; set; }
        public int CLI_ID { get; set; }
        public System.DateTime DATA_ASSOCIACAO { get; set; }

        public virtual CadernoDTO CADERNO { get; set; }
        public virtual ClientesRefDTO CLIENTES_REF { get; set; }
    }
}
