using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(INFORMATIVO_SEMANAL_ENVIO))]
    public partial class InformativoSemanalEnvioDTO
    {
        public string INF_ANO { get; set; }
        public string INF_REMESSA { get; set; }
        public int INF_ENVIO { get; set; }
        public int INF_PRO_ID { get; set; }
        public int INF_TIPO { get; set; }
        public string INF_ARQUIVO { get; set; }
        public string INF_TEXTO { get; set; }
        public System.DateTime DATA_CADASTRO { get; set; }
        public string USU_LOGIN { get; set; }
    }
}
