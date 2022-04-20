using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(INFORMATIVO_SEMANAL))]
    public partial class InformativoSemanalDTO
    {
        public string INF_ANO { get; set; }
        public string INF_REMESSA { get; set; }
        public int INF_ENVIO { get; set; }
        public System.DateTime INF_DATA { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> INF_ENTREGA { get; set; }
        public Nullable<int> INF_QTDE_ARQUIVOS { get; set; }
        public Nullable<System.DateTime> INF_DATA_REPROCESSO { get; set; }
        public string USU_LOGIN_REPROCESSO { get; set; }
    }
}
