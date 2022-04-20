using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoFornecedorDTO
    {
        public TipoFornecedorDTO()
        {
            this.FORNECEDOR = new HashSet<FornecedorDTO>();
        }
    
        public int TIPO_FOR_ID { get; set; }
        public string TIPO_FOR_DESCRICAO { get; set; }

        public virtual ICollection<FornecedorDTO> FORNECEDOR { get; set; }
    }
}
