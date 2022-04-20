using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class UnidadeMedidaDTO
    {
        public UnidadeMedidaDTO()
        {
            //this.NOTA_FISCAL_ITEM = new HashSet<NOTA_FISCAL_ITEM>();
            this.PRODUTOS = new HashSet<ProdutosDTO>();
            this.PRODUTOS1 = new HashSet<ProdutosDTO>();
        }
    
        public string UND_ID { get; set; }
        public string UND_DESCRICAO { get; set; }
    
        //public virtual ICollection<NOTA_FISCAL_ITEM> NOTA_FISCAL_ITEM { get; set; }
        public virtual ICollection<ProdutosDTO> PRODUTOS { get; set; }
        public virtual ICollection<ProdutosDTO> PRODUTOS1 { get; set; }
    }
}
