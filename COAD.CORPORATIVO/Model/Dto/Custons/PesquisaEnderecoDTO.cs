using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{

    public partial class PesquisaEnderecoDTO
    {
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CLI_NOME { get; set; }
        public string END_LOGRADOURO { get; set; }
        public string END_COMPLEMENTO { get; set; }
        public string END_NUMERO { get; set; }
        public string END_BAIRRO { get; set; }
        public string END_CEP { get; set; }
        public string END_MUNICIPIO { get; set; }
        public string END_UF { get; set; }
    }

}
