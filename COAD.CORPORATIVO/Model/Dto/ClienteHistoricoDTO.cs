using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class ClienteHistoricoDTO
    {
        public int CLI_HIS_ID { get; set; }
        public int CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string CLI_A_C { get; set; }
        public string CLI_TP_PESSOA { get; set; }
        public string CLI_CPF_CNPJ { get; set; }
        public string CLI_INSCRICAO { get; set; }
        public string MXM_CODIGO { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> CLA_ID { get; set; }
        public Nullable<int> TIPO_CLI_ID { get; set; }

        public virtual ClienteDto CLIENTES { get; set; }
    }
}
