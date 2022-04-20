using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class ConsultaEmailDTO
    {
        public int CEM_ID { get; set; }
        public string CEM_NUM_ASSINATURA { get; set; }
        public string CEM_LOGIN_RESPONDENTE { get; set; }
        public string CEM_EMAIL_CONSULENTE { get; set; }
        public string CEM_PERGUNTA { get; set; }
        public string CEM_RESPOSTA { get; set; }
        public string CEM_UF_CONSULTA { get; set; }
        public Nullable<System.DateTime> CEM_DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> CEM_DATE_RESPOSTA { get; set; }
        public Nullable<System.DateTime> CEM_DATA_ULTIMO_ACESSO { get; set; }
        public string CEM_LOGIN_USUARIO_ULTIMO_ACESSO { get; set; }
        public string CEM_LOGIN_USUARIO_QUE_ENCAMINHOU { get; set; }
        public int STS_ID { get; set; }
        public int COLEC_ID { get; set; }
        public Nullable<int> GRG_ID { get; set; }
        public Nullable<int> VERB_ID { get; set; }

        public virtual ConsultaEmailColecionadorDTO CONSULTA_EMAIL_COLECIONADOR { get; set; }
        public virtual ConsultaEmailVerbeteDTO CONSULTA_EMAIL_VERBETE { get; set; }
        public virtual ConsultaEmailGrandeGrupoDTO CONSULTA_EMAIL_GRANDE_GRUPO { get; set; }
        public virtual ConsultaEmailStatusDTO CONSULTA_EMAIL_STATUS { get; set; }
    }
}
