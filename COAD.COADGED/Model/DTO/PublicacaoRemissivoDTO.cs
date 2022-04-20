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
    public class PublicacaoRemissivoDTO
    {
        [DisplayName("ID")]
        public Nullable<int> PRE_ID { get; set; }

        [DisplayName("Nº Matéria")]
        public Nullable<int> PUB_ID { get; set; }

        [DisplayName("Colecionador")]
        [Required(ErrorMessage = "Por favor, informe o colecionador!")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("Remissivo")]
        [MaxLength(400, ErrorMessage = "Por favor, informe um remissivo com 400 caracteres no máximo!")]
        [Required(ErrorMessage = "Por favor, informe o Remissivo!")]
        public string PRE_REMISSIVO { get; set; }

        public virtual PublicacaoAreaConsultoriaDTO PUBLICACAO_AREAS_CONSULTORIA { get; set; }
    }
}
