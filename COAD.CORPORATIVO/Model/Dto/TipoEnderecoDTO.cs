using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoEnderecoDTO
    {
        public TipoEnderecoDTO()
        {
            this.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();
        }
    
        public int TP_END_ID { get; set; }
        public string TP_END_DESCRICAO { get; set; }
    
        public virtual ICollection<ClienteEnderecoDto> CLIENTES_ENDERECO { get; set; }
    }
}
