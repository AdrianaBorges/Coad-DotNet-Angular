using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class CarteiraAssinaturaDTO
    {
        public string CAR_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public int CLI_ID { get; set; }
        public string CLI_NOME { get; set; }

        public Nullable<int> UEN_ID { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual CarteiraDTO CARTEIRA { get; set; }

    }
}
