using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class FuncionalidadeNivelAcessoDTO
    {
        public FuncionalidadeNivelAcessoDTO()
        {
            this.FUNCIONALIDADE = new HashSet<FuncionalidadeDTO>();
        }
    
        public int NIV_ID { get; set; }
        public string NIV_DESCRICAO { get; set; }

        public virtual ICollection<FuncionalidadeDTO> FUNCIONALIDADE { get; set; }
    }
}
