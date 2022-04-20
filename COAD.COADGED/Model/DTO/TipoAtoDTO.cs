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
    public class TipoAtoDTO
    {
        public TipoAtoDTO()
        {
            this.PUBLICACAO = new HashSet<PublicacaoDTO>();
            this.PUBLICACAO1 = new HashSet<PublicacaoDTO>();
        }
    
        [DisplayName("ID")]
        public Nullable<int> TIP_ATO_ID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Por favor, você precisa informar a descrição deste tipo de ato!")]
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string TIP_ATO_DESCRICAO { get; set; }

        [DisplayName("Ativo")]
        public Nullable<int> TIP_ATIVO { get; set; }

        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }

        public virtual ICollection<PublicacaoDTO> PUBLICACAO { get; set; }
        public virtual ICollection<PublicacaoDTO> PUBLICACAO1 { get; set; }
    }
}
