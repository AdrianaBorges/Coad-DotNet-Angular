using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class CapitalDTO
    {
        public CapitalDTO()
        {
            this.PUBLICACAO_AREAS_CONSULTORIA = new HashSet<PublicacaoAreaConsultoriaDTO>();
        }
    
        public int CAP_ID { get; set; }
        public string CAP_NOME { get; set; }
    
        public virtual ICollection<PublicacaoAreaConsultoriaDTO> PUBLICACAO_AREAS_CONSULTORIA { get; set; }
    }
}
