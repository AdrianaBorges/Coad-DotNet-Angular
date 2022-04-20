using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class HistAtendEmailDTO
    {
        public int HAE_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public int CON_ID { get; set; }
        public int CON_ID_SUPERVISOR { get; set; }
        public int HAE_COD_CQ { get; set; }
        public string HAE_EMAIL { get; set; }
        public string HAE_UF { get; set; }
        public string HAE_PERGUNTA { get; set; }
        public string HAE_RESPOSTA_CONSULTOR { get; set; }
        public string HAE_RESPOSTA_SUPERVISOR { get; set; }
        public string HAE_RESPOSTA_CQ { get; set; }
        public string HAE_STATUS { get; set; }
        public System.DateTime HAE_DTCADASTRO { get; set; }
        public Nullable<System.DateTime> HAE_DTRESP_CONSULTOR { get; set; }
        public Nullable<System.DateTime> HAE_DTRESP_SUPERVISOR { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
    }
}
