using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce
{
    public class UsuarioEcommerce
    {

       public int? Id { get; set; }

       [Required(ErrorMessage = "Informe o E-Mail")]
       [EmailAddress(ErrorMessage = "E-Mail inválido")]
       public string login { get; set; }

       [Required(ErrorMessage = "Informe a senha")]
       public string senha { get; set; }

        public int? cli_id { get; set; }

    }
}
