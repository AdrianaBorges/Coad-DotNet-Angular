using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class OrigemAcessoRefDTO
    {
        public OrigemAcessoRefDTO()
        {
            this.ORIGEM_FUNCIONALIDADE = new HashSet<OrigemFuncionalidadeDTO>();
            this.TAB_DINAMICA_ORIGEM = new HashSet<TabDinamicaOrigemDTO>();
        }
    
        public int OAC_ID { get; set; }

        public virtual ICollection<OrigemFuncionalidadeDTO> ORIGEM_FUNCIONALIDADE { get; set; }
        public virtual ICollection<TabDinamicaOrigemDTO> TAB_DINAMICA_ORIGEM { get; set; }
    }
}
