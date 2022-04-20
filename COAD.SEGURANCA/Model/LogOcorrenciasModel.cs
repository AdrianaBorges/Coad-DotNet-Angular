using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    public class LogOcorrenciasModel
    {
        public int LOG_SEQ { get; set; }
        public string LOG_MESSAGE { get; set; }
        public string LOG_IP_ACESSO { get; set; }
        public string ITM_PATH { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> LOG_DATA { get; set; }
        public string LOG_ID_ERRO { get; set; }
        public string LOG_EMAIL { get; set; }

    }
}
