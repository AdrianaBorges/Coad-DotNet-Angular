using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class CadernoConteudoDTO
    {
        public int CON_ID { get; set; }
        public int CAD_ID { get; set; }
        public int CONT_OFFLINE { get; set; }
        public int PUB_ID { get; set; }

        public virtual CadernoDTO CADERNO { get; set; }
    }
}
