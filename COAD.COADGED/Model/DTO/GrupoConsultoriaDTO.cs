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
    public class GrupoConsultoriaDTO
    {
        public GrupoConsultoriaDTO()
        {
            this.PRODUTO_REF = new HashSet<ProdutosDTO>();
        }
    
        [DisplayName("ID")]
        public Nullable<int> GRD_CONS_ID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Por favor, você precisa informar o nome do grupo da consultoria!")]
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string GRD_CONS_DESCRICAO { get; set; }

        [DisplayName("Ativo")]
        [Required(ErrorMessage = "Por favor, informe se o grupo de consultoria está ativo escolhendo [Sim] ou [Não]!")]
        public Nullable<int> GRD_CONS_ATIVO { get; set; }

        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public string USU_LOGIN { get; set; }
    
        public virtual ICollection<ProdutosDTO> PRODUTO_REF { get; set; }
    }
}
