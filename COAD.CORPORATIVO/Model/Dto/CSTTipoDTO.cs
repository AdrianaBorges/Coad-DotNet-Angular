using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class CSTTipoDTO
    {
        public CSTTipoDTO()
        {
            this.CST = new HashSet<CSTDTO>();
        }
    
        public int CST_TIPO_ID { get; set; }
        public string CST_DESCRICAO { get; set; }
    
        public virtual ICollection<CSTDTO> CST { get; set; }
    }
}
