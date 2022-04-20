using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoProdComportamentoDTO
    {
        public TipoProdComportamentoDTO()
        {
            this.PRODUTOS = new HashSet<ProdutosDTO>();
        }
    
        public int TPC_ID { get; set; }
        public string TPC_DESCRICAO { get; set; }

        public virtual ICollection<ProdutosDTO> PRODUTOS { get; set; }
    }
}
