using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Prospect
{
    public class TipoTelefoneDTO
    {
        public TipoTelefoneDTO()
        {
            this.PROSPECTS_TELEFONE = new HashSet<ProspectsTelefoneDTO>();
        }
    
        public string TIPO_TEL_ID { get; set; }
        public virtual ICollection<ProspectsTelefoneDTO> PROSPECTS_TELEFONE { get; set; }
    }
}
