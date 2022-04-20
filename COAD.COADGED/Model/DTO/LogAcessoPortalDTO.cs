using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class LogAcessoPortalDTO
    {
        public int LAP_SEQ { get; set; }
        public string LAP_IP_ACESSO { get; set; }
        public Nullable<System.DateTime> LAP_DATA_ACESSO { get; set; }
        public string LAP_MSG_ERRO { get; set; }
        public string LAP_URL_ACESSO { get; set; }
        public Nullable<int> NOT_ID { get; set; }
        public Nullable<int> PUB_ID { get; set; }
        public int OAC_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string LSI_EMAIL_ACESSO { get; set; }
        public Nullable<int> NOT_ID_PORTAL { get; set; }
        public Nullable<int> PUB_ID_PORTAL { get; set; }

        public virtual NoticiaDTO NOTICIA { get; set; }
        public virtual OrigemAcessoRefDTO ORIGEM_ACESSO_REF { get; set; }
        public virtual PublicacaoDTO PUBLICACAO { get; set; }
    } 
}
