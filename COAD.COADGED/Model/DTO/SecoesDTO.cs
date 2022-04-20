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
    public class SecoesDTO
    {
        public SecoesDTO()
        {
            this.PUBLICACAO = new HashSet<PublicacaoDTO>();
        }
    
        [DisplayName("ID")]
        public Nullable<int> SEC_ID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Por favor, você precisa informar uma descrição para esta seção!")]
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres para o nome da seção!")]
        public string SEC_DESCRICAO { get; set; }

        [DisplayName("Ativo")]
        public Nullable<int> SEC_ATIVO { get; set; }

        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }

        public virtual ICollection<PublicacaoDTO> PUBLICACAO { get; set; }
    }
}
