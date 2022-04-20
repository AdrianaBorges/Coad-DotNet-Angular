using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    [Mapping(Source = typeof(LOG_SIMULADOR))]
    public class LogSimuladorDTO
    {
        public int LSI_SEQ { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public System.DateTime LSI_DATA_ACESSO { get; set; }
        public string LSI_URL_ACESSO { get; set; }
        public string LSI_IP_ACESSO { get; set; }
        public string USU_LOGIN { get; set; }
        public string LSI_TIPO_ACESSO { get; set; }
        public string TDC_ID { get; set; }
        public string LSI_EMAIL_ACESSO { get; set; }
    }
}
