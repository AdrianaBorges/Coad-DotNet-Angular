using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class GrupoDTO
    {
        public GrupoDTO()
        {
            this.PRODUTOS = new HashSet<ProdutosDTO>();
        }
    
        public int GRUPO_ID { get; set; }
        public string GRU_DESCRICAO { get; set; }
    
        public virtual ICollection<ProdutosDTO> PRODUTOS { get; set; }
    }
}
