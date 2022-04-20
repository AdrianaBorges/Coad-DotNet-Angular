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
    public class PublicacaoTitulacaoDTO
    {
        [DisplayName("ID")]
        public Nullable<int> PTI_ID { get; set; }

        [DisplayName("Nº Matéria")]
        public Nullable<int> PUB_ID { get; set; }

        [DisplayName("Colecionador")]
        [Required(ErrorMessage = "Por favor, informe o colecionador!")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("Grande grupo")]
        [Required(ErrorMessage = "Por favor, informe o Grande Grupo!")]
        public Nullable<int> TIT_ID { get; set; }

        [DisplayName("Verbete")]
        [Required(ErrorMessage = "Por favor, informe o Verbete!")]
        public Nullable<int> TIT_ID_VERBETE { get; set; }

        [DisplayName("Subverbete")]
        [Required(ErrorMessage = "Por favor, informe o Subverbete!")]
        public Nullable<int> TIT_ID_SUBVERBETE { get; set; }

        [DisplayName("Principal")]
        public Nullable<bool> PTI_PRINCIPAL { get; set; }

        public virtual PublicacaoAreaConsultoriaDTO PUBLICACAO_AREAS_CONSULTORIA { get; set; }
    }
}
