using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class ProdutoComposicaoRefDTO
    {
        public ProdutoComposicaoRefDTO()
        {
            this.FUNCIONALIDADE = new HashSet<FuncionalidadeDTO>();
        }
    
        public int CMP_ID { get; set; }

        public virtual ICollection<FuncionalidadeDTO> FUNCIONALIDADE { get; set; }
    }
}
