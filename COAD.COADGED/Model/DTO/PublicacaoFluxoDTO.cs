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
    public class PublicacaoFluxoDTO
    {
        public PublicacaoFluxoDTO()
        {
            this.PUBLICACAO_FLUXO_ETAPA = new HashSet<PublicacaoFluxoEtapaDTO>();
            this.PUBLICACAO = new HashSet<PublicacaoDTO>();
        }
    
        public int FLU_ID { get; set; }
        public string FLU_DESCRICAO { get; set; }
        public Nullable<bool> FLU_ATIVO { get; set; }
    
        public virtual ICollection<PublicacaoFluxoEtapaDTO> PUBLICACAO_FLUXO_ETAPA { get; set; }
        public virtual ICollection<PublicacaoDTO> PUBLICACAO { get; set; }
    }
}
