using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class AcaoAtendimentoDTO
    {
        public AcaoAtendimentoDTO()
        {
            this.HISTORICO_ATENDIMENTO = new HashSet<HistoricoAtendimentoDTO>();
        }
    
        public int ACA_ID { get; set; }
        public string ACA_DESCRICAO { get; set; }
        public Nullable<bool> ACA_IMP_ETIQUETA { get; set; }

        public virtual ICollection<HistoricoAtendimentoDTO> HISTORICO_ATENDIMENTO { get; set; }
    }
}
