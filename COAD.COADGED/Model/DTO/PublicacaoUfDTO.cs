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
    public class PublicacaoUfDTO
    {
        public Nullable<int> PUB_ID { get; set; }

        [Required(ErrorMessage = "Por favor, informe o Colecionador!")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("UF")]
        [Required(ErrorMessage = "Por favor, informe a UF!")]
        public string UF_ID { get; set; }

        [DisplayName("Ano")]
        [Required(ErrorMessage = "Por favor, informe o Ano do Informativo!")]
        public string INF_ANO { get; set; }

        [DisplayName("Número")]
        [Required(ErrorMessage = "Por favor, informe o Número do Informativo!")]
        public Nullable<int> INF_NUMERO { get; set; }

        [DisplayName("Ativo")]
        public Nullable<bool> PUB_ATIVO { get; set; }

        public virtual InformativoDTO INFORMATIVO { get; set; }
        public virtual PublicacaoAreaConsultoriaDTO PUBLICACAO_AREAS_CONSULTORIA { get; set; }
        public virtual UfDTO UF_REF { get; set; }
    }
}
