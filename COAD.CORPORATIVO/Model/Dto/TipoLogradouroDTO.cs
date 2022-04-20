using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoLogradouroDTO
    {
        public TipoLogradouroDTO()
        {
            this.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();
        }
    
        public int TIPO_LOG_ID { get; set; }
        public string TIPO_LOG_DESCRICAO { get; set; }
    
        public virtual ICollection<ClienteEnderecoDto> CLIENTES_ENDERECO { get; set; }
    }
}
