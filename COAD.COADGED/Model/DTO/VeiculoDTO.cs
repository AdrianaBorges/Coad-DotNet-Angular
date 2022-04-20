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
    public class VeiculoDTO
    {
        public VeiculoDTO()
        {
            this.PUBLICACAO = new HashSet<PublicacaoDTO>();
        }
    
        [DisplayName("ID")]
        public Nullable<int> TVI_ID { get; set; }

        [DisplayName("Nome do veículo publicador")]
        [Required(ErrorMessage = "Por favor, você precisa informar o nome do veículo!")]
        [MaxLength(50, ErrorMessage = "Por favor, preencha no máximo 50 caracteres!")]
        public string TVI_DESCRICAO { get; set; }

        [DisplayName("Periodicidade")]
        [Required(ErrorMessage = "Por favor, você precisa informar a periodicidade de publicação deste veículo!")]
        public Nullable<int> PRD_ID { get; set; }

        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }

        [DisplayName("Ativo")]
        public Nullable<int> TVI_ATIVO { get; set; }
    
        public virtual PeriodicidadeDTO PERIODICIDADE { get; set; }
        public virtual ICollection<PublicacaoDTO> PUBLICACAO { get; set; }
    }
}
