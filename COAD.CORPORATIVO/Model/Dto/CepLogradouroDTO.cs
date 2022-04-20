using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class CepLogradouroDTO
    {
        public int CEP_ID { get; set; }
        public string CEP_UF { get; set; }
        public string CEP_LOG { get; set; }
        public int BAR_ID { get; set; }
        public string CEP_NUMERO { get; set; }
        public string CEP_TIPO_LOGRADOURO { get; set; }
        public string CEP_LOG_SEM_ACENTO { get; set; }
        public Nullable<int> MUN_ID { get; set; }

        public virtual CepBairroDTO CEP_BAIRRO { get; set; }
        public virtual MunicipioDTO MUNICIPIO { get; set; }
    }
}
