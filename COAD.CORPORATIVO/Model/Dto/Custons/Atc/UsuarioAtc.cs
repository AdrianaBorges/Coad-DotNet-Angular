using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Atc
{
    public class UsuarioAtc
    {
        public int? Id { get; set; }

        public string Assinatura { get; set; }

        [Required(ErrorMessage = "Informe a assinatura")]
        public string login { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        public string senha { get; set; }
    }
}
