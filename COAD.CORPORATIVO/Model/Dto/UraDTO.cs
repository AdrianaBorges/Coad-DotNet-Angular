using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{

    public partial class UraDTO
    {
 
        public UraDTO()
        {
            this.HIST_ATEND_URA = new HashSet<HistAtendUraDTO>();
            this.URA_COAD = new HashSet<UraCoadDTO>();
            this.URA_CONFIG = new HashSet<UraConfigDTO>();
            this.URA_LOG = new HashSet<UraLogDTO>();
            this.URA_PRODUTO = new HashSet<UraProdutoDTO>();

        }

        public string URA_ID { get; set; }
        public Nullable<int> URA_STATUS { get; set; }
        public string UF_SIGLA { get; set; }
        public string URA_CONNECTION_STRING { get; set; }
        public Nullable<System.DateTime> URA_DATA_ATUALIZACAO { get; set; }
        public Nullable<int> URA_TIPO_ID { get; set; }

        public virtual ICollection<HistAtendUraDTO> HIST_ATEND_URA { get; set; }
        public virtual UFDTO UF { get; set; }
        public virtual ICollection<UraCoadDTO> URA_COAD { get; set; }
        public virtual ICollection<UraConfigDTO> URA_CONFIG { get; set; }
        public virtual ICollection<UraLogDTO> URA_LOG { get; set; }
        public virtual ICollection<UraProdutoDTO> URA_PRODUTO { get; set; }
       // public virtual URA_TIPO URA_TIPO { get; set; }
    
    }
}
