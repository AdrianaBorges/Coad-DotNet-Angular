using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class ProdutoRefDTO
    {
        public ProdutoRefDTO()
        {
            this.FUNCIONALIDADE = new HashSet<FuncionalidadeDTO>();
        }

        public int PRO_ID { get; set; }

        public virtual ICollection<FuncionalidadeDTO> FUNCIONALIDADE { get; set; }
    }
}
