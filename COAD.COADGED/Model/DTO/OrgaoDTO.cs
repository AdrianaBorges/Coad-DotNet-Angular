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
    public class OrgaoDTO
    {
        public OrgaoDTO()
        {
            this.PUBLICACAO = new HashSet<PublicacaoDTO>();
            this.PUBLICACAO1 = new HashSet<PublicacaoDTO>();
        }
    
        [DisplayName("ID")]
        public Nullable<int> ORG_ID { get; set; }

        [DisplayName("Órgao emissor de Atos")]
        [Required(ErrorMessage = "Por favor, informe uma descrição para este Órgão!")]
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string ORG_DESCRICAO { get; set; }

        [DisplayName("Ativo")]
        public Nullable<int> ORG_ATIVO { get; set; }

        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }

        public virtual ICollection<PublicacaoDTO> PUBLICACAO { get; set; }
        public virtual ICollection<PublicacaoDTO> PUBLICACAO1 { get; set; }
    }
}
