using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class CarteiraClienteDTO
    {
        public string CAR_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public System.DateTime DATA_ASSOCIACAO { get; set; }
        public bool CCL_ORIGEM_PROSPECT { get; set; }
        public bool Deletar { get; set; }
    
        public virtual AssinaturaDTO ASSINATURA {get; set;}
        public virtual CarteiraDTO CARTEIRA { get; set; }
        public virtual ClienteDto CLIENTES { get; set; }
    }
}
