using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Validations
{
    /// <summary>
    /// Valida se o email ou e telefone do cliente estão presentes.
    /// É necessário uma validação customizado, pois, tanto o telefone
    /// quanto o email, não estão ligados diretamente com o cliente. 
    /// Ele está ligado atravez da clientes.
    /// </summary>
    public class AssinaturaEmailOuTelefoneValidatorAttribute : ValidationAttribute
    {
        public bool ValidarApenasAssinaturaFranquia { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is IEnumerable<AssinaturaDTO>)
            {
                return IsValid((IEnumerable<AssinaturaDTO>)value, validationContext);
            }

            if (value is AssinaturaDTO)
            {
                return IsValid((AssinaturaDTO)value, validationContext);
            }

            return ValidationResult.Success;
        }

        protected ValidationResult IsValid(AssinaturaDTO value, ValidationContext validationContext)
        {

            if (value is AssinaturaDTO)
            {
                AssinaturaDTO assinatura = value as AssinaturaDTO;
                var lstTelefone = assinatura.ASSINATURA_TELEFONE;
                var lstEmail = assinatura.ASSINATURA_EMAIL;

                if (!ValidarApenasAssinaturaFranquia || ValidarApenasAssinaturaFranquia && assinatura.UEN_ID == 1)
                {
                    if ((lstTelefone == null || lstTelefone.Count() < 1) && (lstEmail == null || lstEmail.Count() < 1))
                    {
                        return new ValidationResult("Informe um telefone ou email. É necessário informar pelo menos uma maneira de contatar o cliente.", new List<string>(){
                
                            "email_telefone"                
                        });
                    }
                    
                }                
            }
            return ValidationResult.Success;
        }

        protected ValidationResult IsValid(IEnumerable<AssinaturaDTO> value, ValidationContext validationContext)
        {

            if (value is IEnumerable<AssinaturaDTO>)
            {
                var assinaturas = value as IEnumerable<AssinaturaDTO>;

                foreach (var ass in assinaturas)
                {
                    var result = IsValid(ass, validationContext);

                    if (result != ValidationResult.Success)
                        return result;
                }

            }

            return ValidationResult.Success;
        }


    }
}
