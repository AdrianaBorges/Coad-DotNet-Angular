using System;
using System.Web;
using System.Globalization;
using System.Web.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COAD.SEGURANCA.Repositorios.Base
{
    public class AlterarSenha
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha não foi informada!")]
        [StringLength(12, ErrorMessage = "A senha informada deve possuir entre 6 e 12 caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }
                
        [DataType(DataType.Password)]
        [Display(Name = "Confirma Senha")]
        [Compare("Senha", ErrorMessage = "Os campos senha e confirma senha não conferem. Verifique!")]
        public string ConfirmaSenha { get; set; }
    }
 
}
