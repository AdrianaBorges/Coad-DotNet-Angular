using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class ClientesRefDTO
    {
        public ClientesRefDTO()
        {
            this.CADERNO = new HashSet<CadernoDTO>();
            this.CADERNO_COMPARTILHADO = new HashSet<CadernoCompartilhadoDTO>();
        }
    
        public int CLI_ID { get; set; }

        public virtual ICollection<CadernoDTO> CADERNO { get; set; }
        public virtual ICollection<CadernoCompartilhadoDTO> CADERNO_COMPARTILHADO { get; set; }
    }
}
