using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class AreaConsultoriaDTO
    {
        public AreaConsultoriaDTO()
        {
            this.URA_PRODUTO_AREA = new HashSet<UraProdutoAreaDTO>();
            this.PRODUTOS = new HashSet<ProdutosDTO>();
        }
    
        public int ACO_ID { get; set; }
        public string ACO_DESCRICAO { get; set; }

        public virtual ICollection<UraProdutoAreaDTO> URA_PRODUTO_AREA { get; set; }
        public virtual ICollection<ProdutosDTO> PRODUTOS { get; set; }
    }
}
