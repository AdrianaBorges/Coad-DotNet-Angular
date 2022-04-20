using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class UnidadeNegocioDTO
    {
        public UnidadeNegocioDTO()
        {
            this.PRODUTO_COMPOSICAO = new HashSet<ProdutoComposicaoDTO>();
        }
    
        public string UND_NEGOCIO_ID { get; set; }
        public string UND_NEGOCIO_DESCR { get; set; }

        public virtual ICollection<ProdutoComposicaoDTO> PRODUTO_COMPOSICAO { get; set; }
    }
}
