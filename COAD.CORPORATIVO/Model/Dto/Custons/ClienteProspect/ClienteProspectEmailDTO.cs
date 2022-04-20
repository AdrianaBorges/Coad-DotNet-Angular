using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect
{
    public class ClienteProspectEmailDTO
    {
        public int? CodEmail { get; set; }
        public int? TipoAtendimento { get; set; }

        [Required(ErrorMessage = "Informe o E-Mail.")]
        [EmailAddress(ErrorMessage = "O E-Email não é válido.")]
        [RegularExpression(@"(.*[^\.\s]$)", ErrorMessage = "Digite um E-Mail válido")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O campo email deve possuir no máximo 60 caracteres")]
        public string Email { get; set; }
    }
}
