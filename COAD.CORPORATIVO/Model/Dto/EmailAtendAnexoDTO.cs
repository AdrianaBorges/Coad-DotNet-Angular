using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class EmailAtendAnexoDTO
    {
        public int ANX_ID { get; set; }
        public Nullable<int> EAT_ID { get; set; }
        public byte[] ANX_ANEXO { get; set; }
        public string ANX_ANEXO_NOMEARQ { get; set; }
    
        public virtual EmailAtendDTO EMAIL_ATEND { get; set; }
    }
}
