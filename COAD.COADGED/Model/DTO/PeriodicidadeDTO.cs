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
    public class PeriodicidadeDTO
    {
        public PeriodicidadeDTO()
        {
            this.VEICULO = new HashSet<VeiculoDTO>();
        }
    
        [DisplayName("ID")]
        public Nullable<int> PRD_ID { get; set; }

        [DisplayName("Descricao")]
        [Required(ErrorMessage = "Por favor, você precisa informar um período! [mensal/anual/bimestral...]")]
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string PRD_DESCRICAO { get; set; }

        public virtual ICollection<VeiculoDTO> VEICULO { get; set; }
    }
}
