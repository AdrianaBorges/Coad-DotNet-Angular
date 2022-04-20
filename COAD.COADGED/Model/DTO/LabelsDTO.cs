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
    public class LabelsDTO
    {
        public LabelsDTO()
        {
            this.PUBLICACAO = new HashSet<PublicacaoDTO>();
        }

        [DisplayName("ID")]
        public Nullable<int> LBL_ID { get; set; }

        [DisplayName("Label")]
        [Required(ErrorMessage = "Por favor, você precisa informar o nome da label!")]
        [MaxLength(70, ErrorMessage = "Por favor, informe no máximo 70 caracteres!")]
        public string LBL_NOME { get; set; }

        [DisplayName("Descrição")]
        //[Required(ErrorMessage = "Por favor, você precisa informar a descrição desta label!")]
        [MaxLength(400, ErrorMessage = "Por favor, informe no máximo 400 caracteres!")]
        public string LBL_DESCRICAO { get; set; }

        [DisplayName("Ativo")]
        [Required(ErrorMessage = "Por favor, informe se a Label está ativa!")]
        public Nullable<int> LBL_ATIVO { get; set; }

        public virtual ICollection<PublicacaoDTO> PUBLICACAO { get; set; }
    }
}
