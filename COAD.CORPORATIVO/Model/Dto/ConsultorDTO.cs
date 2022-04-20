using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class ConsultorDTO
    {
        public ConsultorDTO()
        {
            this.HIST_ATEND_EMAIL = new HashSet<HistAtendEmailDTO>();
            this.HIST_ATEND_EMAIL1 = new HashSet<HistAtendEmailDTO>();
        }

        public int CON_ID { get; set; }
        public string CON_NOME { get; set; }
        public string CON_TIPO { get; set; }
        public Nullable<bool> CON_SUPERVISOR { get; set; }
        public string USU_LOGIN { get; set; }

        public virtual ICollection<HistAtendEmailDTO> HIST_ATEND_EMAIL { get; set; }
        public virtual ICollection<HistAtendEmailDTO> HIST_ATEND_EMAIL1 { get; set; }
    }
}
