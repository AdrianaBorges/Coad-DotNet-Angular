using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class CadernoDTO
    {
        public CadernoDTO()
        {
            this.CADERNO_COMPARTILHADO = new HashSet<CadernoCompartilhadoDTO>();
            this.CADERNO_CONTEUDO = new HashSet<CadernoConteudoDTO>();
            this.CADERNO_NOTA = new HashSet<CadernoNotaDTO>();
        }
    
        public int CAD_ID { get; set; }
        public int CLI_ID { get; set; }
        public string CAD_NOME { get; set; }
        public Nullable<int> CAD_ATIVO { get; set; }
    
        public virtual ClientesRefDTO CLIENTES_REF { get; set; }
        public virtual ICollection<CadernoCompartilhadoDTO> CADERNO_COMPARTILHADO { get; set; }
        public virtual ICollection<CadernoConteudoDTO> CADERNO_CONTEUDO { get; set; }
        public virtual ICollection<CadernoNotaDTO> CADERNO_NOTA { get; set; }
    }
}
