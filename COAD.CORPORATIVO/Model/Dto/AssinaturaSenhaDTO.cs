using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class AssinaturaSenhaDTO
    {
        public string ASN_NUM_ASSINATURA { get; set; }
        public string ASN_SENHA { get; set; }
        public bool ASN_ATIVO { get; set; }
        public System.DateTime ASN_DATA_CADASTRO { get; set; }
        public System.DateTime ASN_DATA_ALTERA { get; set; }
    }

}
