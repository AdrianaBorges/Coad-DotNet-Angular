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
    public class PublicacaoPalavraChaveDTO
    {
        [DisplayName("ID")]
        public Nullable<int> PPC_ID { get; set; }
        
        [DisplayName("Nº Matéria")]
        public Nullable<int> PUB_ID { get; set; }

        [DisplayName("Colecionador")]
        [Required(ErrorMessage = "Por favor, informe o Colecionador!")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("Palavra chave")]
        [MaxLength(50, ErrorMessage = "Por favor, informe uma palavra chave com 50 caracteres no máximo!")]
        [Required(ErrorMessage = "Por favor, informe a Palavra Chave!")]
        public string PPC_TEXTO { get; set; }

        public virtual PublicacaoAreaConsultoriaDTO PUBLICACAO_AREAS_CONSULTORIA { get; set; }
    }
}
