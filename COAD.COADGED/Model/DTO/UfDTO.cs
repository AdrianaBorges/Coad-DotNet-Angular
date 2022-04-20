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
    public class UfDTO
    {
        public UfDTO()
        {
            //this.AREAS_CONSULTORIA_NEWS = new HashSet<AREAS_CONSULTORIA_NEWS>();
            this.PUBLICACAO_UF = new HashSet<PublicacaoUfDTO>();
        }
    
        [DisplayName("UF")]
        public string UF_ID { get; set; }
        
        [DisplayName("Estados")]
        public string UF_NOME { get; set; }
    
        //public virtual ICollection<AREAS_CONSULTORIA_NEWS> AREAS_CONSULTORIA_NEWS { get; set; }
        public virtual ICollection<PublicacaoUfDTO> PUBLICACAO_UF { get; set; }
    }
}
