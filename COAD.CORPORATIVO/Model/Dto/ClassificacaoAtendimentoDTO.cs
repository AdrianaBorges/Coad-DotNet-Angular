using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class ClassificacaoAtendimentoDTO
    {
        public ClassificacaoAtendimentoDTO()
        {
            this.TIPO_ATENDIMENTO = new HashSet<TipoAtendimentoDTO>();
        }
    
        public int CLA_ATEND_ID { get; set; }
        public string CLA_ATEND_DESCRICAO { get; set; }

        public virtual ICollection<TipoAtendimentoDTO> TIPO_ATENDIMENTO { get; set; }
    }
}
