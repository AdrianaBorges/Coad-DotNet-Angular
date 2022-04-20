using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ClienteTelefoneDTO
    {   
        public int? CLI_TEL_ID { get; set; }
        public string CLI_TEL_TELEFONE { get; set; }
        public string CLI_TEL_DDD { get; set; }
        public string TIPO_TEL_ID { get; set; }
        public int CLI_ID { get; set; }

        public virtual TipoTelefoneDTO TIPO_TELEFONE { get; set; }
        public virtual ClienteDto CLIENTES { get; set; }

    }
}
