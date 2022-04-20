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
    public class ProdutosDTO
    {
        public ProdutosDTO()
        {
            //this.PUBLICACAO = new HashSet<PUBLICACAO>();
            this.AREAS_CONSULTORIA = new HashSet<AreasDTO>();
        }

        public Nullable<int> PRO_ID { get; set; }
        public Nullable<int> GRD_CONS_ID { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
    
        public virtual GrupoConsultoriaDTO GRUPO_CONSULTORIA { get; set; }
        //public virtual ICollection<PUBLICACAO> PUBLICACAO { get; set; }
        public virtual ICollection<AreasDTO> AREAS_CONSULTORIA { get; set; }
    }
}
