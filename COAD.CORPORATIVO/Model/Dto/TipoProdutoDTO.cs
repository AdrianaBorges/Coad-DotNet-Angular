using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoProdutoDTO
    {
        public TipoProdutoDTO()
        {
            this.PRODUTOS = new HashSet<ProdutosDTO>();
        }
    
        public int TIPO_PRO { get; set; }
        public string TIPO_DESCRICAO { get; set; }
    
        public virtual ICollection<ProdutosDTO> PRODUTOS { get; set; }
    }
}
