using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class NoticiaGrupoDTO
    {
        public NoticiaGrupoDTO()
        {
            this.NOTICIA = new HashSet<NoticiaDTO>();
            this.LINHA_PRODUTO_REF = new HashSet<LinhaProdutoRefDTO>();
        }

        public int NGR_ID { get; set; }
        public string NGR_DESCRICAO { get; set; }

        public virtual ICollection<NoticiaDTO> NOTICIA { get; set; }
        public virtual ICollection<LinhaProdutoRefDTO> LINHA_PRODUTO_REF { get; set; }
    }
}
