using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class CepBairroDTO
    {
        public CepBairroDTO()
        {
            this.CEP_LOGRADOURO = new HashSet<CepLogradouroDTO>();
        }
    
        public int BAR_ID { get; set; }
        public string BAR_DESCRICAO { get; set; }
        public string BAR_DESCRICAO_ABREV { get; set; }

        public virtual ICollection<CepLogradouroDTO> CEP_LOGRADOURO { get; set; }
    }
}
