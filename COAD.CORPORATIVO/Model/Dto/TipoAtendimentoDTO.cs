using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class TipoAtendimentoDTO
    {
        public TipoAtendimentoDTO()
        {
            this.HISTORICO_ATENDIMENTO = new HashSet<HistoricoAtendimentoDTO>();
        }

        public int TIP_ATEND_ID { get; set; }
        public string TIP_ATEND_DESCRICAO { get; set; }
        public string TIP_ATEND_GRUPO { get; set; }
        public Nullable<int> CLA_ATEND_ID { get; set; }
        public Nullable<bool> TIP_EMITE_ETIQUETA { get; set; }

        public virtual ClassificacaoAtendimentoDTO CLASSIFICACAO_ATENDIMENTO { get; set; }
        public virtual ICollection<HistoricoAtendimentoDTO> HISTORICO_ATENDIMENTO { get; set; }
    }
}
