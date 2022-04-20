using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class TipoComplementoDTO
    {
        public TipoComplementoDTO()
        {
            this.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();
        }
    
        public string TIPO_COMP_ID { get; set; }
        public string TIPO_COMP_DESCRICAO { get; set; }

        public virtual ICollection<ClienteEnderecoDto> CLIENTES_ENDERECO { get; set; }
    }
}
