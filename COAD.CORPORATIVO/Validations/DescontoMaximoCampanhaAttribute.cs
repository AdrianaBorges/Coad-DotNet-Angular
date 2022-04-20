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
    public class DescontoMaximoCampanhaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           if(validationContext.ObjectInstance is CampanhaVendaDTO)
            {
                CampanhaVendaDTO campanha = validationContext.ObjectInstance as CampanhaVendaDTO;

                if(campanha.CVE_ACRESCIMO_MINIMO != null && campanha.CVE_DESCONTO_MAX != null)
                {
                    if(campanha.CVE_ACRESCIMO_MINIMO  <= campanha.CVE_DESCONTO_MAX)
                    {
                        return new ValidationResult("A porcentagem de desconto mínima não pode ser maior ou igual ao desconto máximo. Do contrário o desconto anula o acréscimo.");
                    }
                }
            }           

            return ValidationResult.Success;
                
        }
    }
}
