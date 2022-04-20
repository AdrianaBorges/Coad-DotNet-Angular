using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(INFORMATIVO_SEMANAL_ANALITICO))]
    public partial class InformativoSemanalAnaliticoDTO
    {
        public string INF_ANO { get; set; }
        public string INF_REMESSA { get; set; }
        public int INF_ENVIO { get; set; }
        public int INF_PRO_ID { get; set; }
        public bool INF_PROTOCOLADA { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string END_UF { get; set; }
        public string USU_LOGIN { get; set; }

        // estatísticos \\
        public System.DateTime INF_DATA { get; set; }
        public System.DateTime INF_ENTREGA { get; set; }
        public string INF_ARQUIVO { get; set; }
        public string PRODUTO { get; set; }
        public int QTD { get; set; }
        public int PROTOCOLADAS { get; set; }
    }
}
