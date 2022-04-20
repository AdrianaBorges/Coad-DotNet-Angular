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
    public class TipoMateriaDTO
    {
        public TipoMateriaDTO()
        {
            this.PUBLICACAO = new HashSet<PublicacaoDTO>();
        }
    
        [DisplayName("ID")]
        public Nullable<int> TIP_MAT_ID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Por favor, você precisa informar a descrição do tipo da matéria!")]
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string TIP_MAT_DESCRICAO { get; set; }

        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        
        [DisplayName("Ativo")]
        public Nullable<int> TIP_MAT_ATIVO { get; set; }

        [DisplayName("Cabeça da Matéria")]
        public string ARE_CABECA_MATERIA { get; set; }

        public virtual ICollection<PublicacaoDTO> PUBLICACAO { get; set; }
    }
}
