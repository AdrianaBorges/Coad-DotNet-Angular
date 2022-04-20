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
    public class PublicacaoRemissaoDTO
    {
        [DisplayName("ID")]
        public Nullable<int> PRE_ID { get; set; }

        [DisplayName("Nº Matéria")]
        public Nullable<int> PUB_ID { get; set; }

        [DisplayName("Colecionador")]
        [Required(ErrorMessage = "Por favor, informe o colecionador!")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("Remissao")]
        [Required(ErrorMessage = "Por favor, informe a remissão!")]
        [Column(TypeName = "varchar(MAX)")]
        public string PRE_REMISSAO { get; set; }

        [DisplayName("Número")]
        [Required(ErrorMessage = "Por favor, informe o número da remissão!")]
        public int PRE_NUMERO { get; set; }

        public virtual PublicacaoAreaConsultoriaDTO PUBLICACAO_AREAS_CONSULTORIA { get; set; }
    }
}
