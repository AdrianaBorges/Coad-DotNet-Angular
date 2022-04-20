using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ClassificacaoClienteDTO
    {
        public ClassificacaoClienteDTO()
        {
           this.CLIENTES = new HashSet<ClienteDto>();
        }
    
        public int CLA_CLI_ID { get; set; }
        public string CLA_CLI_DESCRICAO { get; set; }
    
        public virtual ICollection<ClienteDto> CLIENTES { get; set; }

      
    }
}
