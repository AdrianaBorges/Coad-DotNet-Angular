using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ClienteOrigemCadastroDTO
    {
        public ClienteOrigemCadastroDTO()
        {
            this.CLIENTES = new HashSet<ClienteDto>();
        }

        public int CLI_O_CAD_ID { get; set; }
        public string CLI_O_CAD_DESCRICAO { get; set; }

        public virtual ICollection<ClienteDto> CLIENTES { get; set; }
        
    }
}
