using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class HistAtendUraDTO
    {

        public string URA_ID { get; set; }
        public string HAU_ID { get; set; }
        public System.DateTime HAU_DATA_CADASTRO { get; set; }
        public Nullable<int> HAU_RAMAL { get; set; }
        public string HAU_TELEFONE { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string HAU_ATENDIDO { get; set; }
        public string HAU_ATENDIDO_SISTEMA { get; set; }
        public Nullable<System.DateTime> HAU_DATA_LOG { get; set; }
        public Nullable<int> HAU_USUARIO_ID { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual UraDTO URA { get; set; }

    }
}
