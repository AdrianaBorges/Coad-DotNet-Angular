using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.EnvioEmail
{
    public class EmailDTO
    {
        [Required(ErrorMessage = "Informe o E-Mail")]
        [EmailAddress(ErrorMessage = "O formato do E-Mail não é válido")]
        public string Email { get; set; }
        public bool CadastrarEmail { get; set; }
    }
}
