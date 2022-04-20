using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class IbgeMunicipioDTO
    {
        public IbgeMunicipioDTO()
        {
            this.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();
        }
    
        public string IBGE_COD_COMPLETO { get; set; }
        public string IBGE_NOME { get; set; }
        public string IBGE_UF { get; set; }
    
        public virtual ICollection<ClienteEnderecoDto> CLIENTES_ENDERECO { get; set; }
    }
}
