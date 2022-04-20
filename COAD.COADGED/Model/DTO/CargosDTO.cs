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
    public class CargosDTO
    {
        public CargosDTO()
        {
            this.COLABORADOR = new HashSet<ColaboradorDTO>();
            this.PUBLICACAO_FLUXO_ETAPA = new HashSet<PublicacaoFluxoEtapaDTO>();
        }

        [DisplayName("ID")]
        public Nullable<int> CRG_ID { get; set; }

        [DisplayName("Cargo")]
        [Required(ErrorMessage = "Por favor, você precisa informar um nome de cargo!")]
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string CRG_DESCRICAO { get; set; }

        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public string USU_LOGIN { get; set; }

        [DisplayName("Sigla")]
        [MaxLength(3, ErrorMessage = "Por favor, informe uma sigla com 03 caracteres no máximo!")]
        public string CRG_SIGLA { get; set; }

        public virtual ICollection<ColaboradorDTO> COLABORADOR { get; set; }
        public virtual ICollection<PublicacaoFluxoEtapaDTO> PUBLICACAO_FLUXO_ETAPA { get; set; }
    }
}
