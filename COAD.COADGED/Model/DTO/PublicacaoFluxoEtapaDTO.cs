using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class PublicacaoFluxoEtapaDTO
    {
        public PublicacaoFluxoEtapaDTO()
        {
            this.PUBLICACAO_CICLO_APROVACAO = new HashSet<PublicacaoCicloAprovacaoDTO>();
        }
    
        public int FLU_ID { get; set; }
        public int FLU_ETAPA_ID { get; set; }
        public Nullable<bool> FLU_PRE_REQUISITO { get; set; }
        public Nullable<bool> FLU_OBRIGATORIO { get; set; }
        public Nullable<int> CRG_ID { get; set; }
    
        public virtual CargosDTO CARGOS { get; set; }
        public virtual PublicacaoFluxoDTO PLUBLICACAO_FLUXO { get; set; }
        public virtual ICollection<PublicacaoCicloAprovacaoDTO> PUBLICACAO_CICLO_APROVACAO { get; set; }
    }
}
