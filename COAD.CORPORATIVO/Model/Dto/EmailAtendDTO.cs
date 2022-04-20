using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{

    public partial class EmailAtendDTO
    {
        public EmailAtendDTO()
        {
            this.EMAIL_ATEND_ANEXO = new HashSet<EmailAtendAnexoDTO>();
        }

        public int EAT_ID { get; set; }
        public string EAT_ASSUNTO { get; set; }
        public Nullable<int> HAE_ID { get; set; }
        public Nullable<System.DateTime> EAT_DATA { get; set; }
        public string EAT_FROM_NOME { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> EAT_DATA_ENVIO { get; set; }
        public string EAT_EMAIL_CC { get; set; }
        public string EAT_EMAIL_BCC { get; set; }
        public string EAT_TEXTO_EMAIL { get; set; }
        public string EAT_EMAIL_FROM { get; set; }

        public virtual ICollection<EmailAtendAnexoDTO> EMAIL_ATEND_ANEXO { get; set; }
        public virtual HistAtendEmailDTO HIST_ATEND_EMAIL { get; set; }
    }

}
