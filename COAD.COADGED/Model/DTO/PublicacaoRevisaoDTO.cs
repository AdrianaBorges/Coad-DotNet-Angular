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
    public class PublicacaoRevisaoDTO
    {
        [DisplayName("ID")]
        public int? REV_ID { get; set; }
        
        [DisplayName("Nº Matéria")]
        public int PUB_ID { get; set; }

        [DisplayName("Colecionador")]
        [Required(ErrorMessage = "Por favor, informe o Colecionador!")]
        public int ARE_CONS_ID { get; set; }

        [DisplayName("(RVT) Revisão Técnica aprovada?")]
        public string REV_TC { get; set; }

        [DisplayName("(DGT) Digitação aprovada?")]
        public string DIG_TC { get; set; }

        [DisplayName("(RVO) Revisão Ortográfica aprovada?")]
        public string REV_OR { get; set; }

        [DisplayName("Colaborador")]
        [Required(ErrorMessage = "Por favor, informe o Colaborador!")]
        public int COL_ID { get; set; }

        // sobre reprovação...
        [DisplayName("Reprovada em")]
        public string REPROVADA { get; set; }

        public virtual ColaboradorDTO COLABORADOR { get; set; }
        public virtual PublicacaoDTO PUBLICACAO { get; set; }
        public virtual PublicacaoAreaConsultoriaDTO PUBLICACAO_AREAS_CONSULTORIA { get; set; }
    }
}
