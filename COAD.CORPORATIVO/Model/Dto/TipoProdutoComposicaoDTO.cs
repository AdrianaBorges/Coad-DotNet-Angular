using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoProdutoComposicaoDTO
    {
        public TipoProdutoComposicaoDTO()
        {
            this.PRODUTO_COMPOSICAO = new HashSet<ProdutoComposicaoDTO>();
        }
    
        public int TIPO_PRO_ID { get; set; }
        public string TIPO_PRO_DESCRICAO { get; set; }
    
        public virtual ICollection<ProdutoComposicaoDTO> PRODUTO_COMPOSICAO { get; set; }
    }
}
