using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class LinhaProdutoRefDTO
    {
        public LinhaProdutoRefDTO()
        {
            this.NOTICIA_GRUPO = new HashSet<NoticiaGrupoDTO>();
        }
    
        public int LIN_PRO_ID { get; set; }

        public virtual ICollection<NoticiaGrupoDTO> NOTICIA_GRUPO { get; set; }
    }
}
