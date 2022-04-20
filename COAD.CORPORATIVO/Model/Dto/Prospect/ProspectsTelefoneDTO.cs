using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Prospect
{
    public class ProspectsTelefoneDTO
    {
        public int? PTEL_ID { get; set; }

        [Required(ErrorMessage = "Digite o telefone")]
        [MaxLength(12, ErrorMessage = "O telefone deve possuir no máximo 12 caracteres")]
        [MinLength(8, ErrorMessage = "O telefone deve possuir no mínimo 8 caracteres")]
        public string PTEL_TELEFONE { get; set; }

        [Required(ErrorMessage = "Selecione o tipo de Telefone")]
        public string TIPO_TEL_ID { get; set; }
        public Nullable<int> CLIENTE_ID { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }

        public virtual ProspectDTO PROSPECT { get; set; }
        public virtual TipoTelefoneDTO TIPO_TELEFONE { get; set; }
    }
}

