using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class CadernoNotaDTO
    {
        public int NTA_ID { get; set; }
        public int CAD_ID { get; set; }
        public string NTA_CONTEUDO { get; set; }
        public Nullable<int> NTA_OFFLINE { get; set; }

        public virtual CadernoDTO CADERNO { get; set; }
    }
}
