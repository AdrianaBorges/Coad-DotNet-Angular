using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class ProdutoFamiliaDTO
    {
        public ProdutoFamiliaDTO()
        {
            this.PRODUTOS = new HashSet<ProdutosDTO>();
        }

        public int FAM_ID { get; set; }
        public string FAM_DESCRICAO { get; set; }

        public virtual ICollection<ProdutosDTO> PRODUTOS { get; set; }
    }
}
