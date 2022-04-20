using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericCrud.Validations
{
    public class CpfOrCnpjValidatorAttribute : ValidationAttribute
    { 
        /// <summary>
        /// Nome de um método que retorna um booleado para decidir se a validação é de um CPF ou CNPJ
        /// Retornar true para CPNJ; Retornar false para CPF e retornar null para não validar.
        /// </summary>
        public string MetodoTipoDocumento { get; set; }
        
        public CpfOrCnpjValidatorAttribute(string MetodoTipoDocumento)
        {
            this.MetodoTipoDocumento = MetodoTipoDocumento;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instancia = validationContext.ObjectInstance;
            if(instancia != null)
            {
                bool? CNPJ = null;
                if (!string.IsNullOrWhiteSpace(MetodoTipoDocumento))
                {
                    var tipo = instancia.GetType();
                    var metodo = tipo.GetMethod(MetodoTipoDocumento);
                    if (metodo != null && (metodo.ReturnType == typeof(bool?) || metodo.ReturnType == typeof(bool)))
                        CNPJ = (bool?) metodo.Invoke(instancia, null);
                }
                string strValue = value as string;

                if (string.IsNullOrWhiteSpace(strValue))
                    return ValidationResult.Success;

                Regex rgx = new Regex(@"[\D]");
                strValue = rgx.Replace(strValue, "");
                bool valido = true;

                if(CNPJ != null && value is string)
                {
                    if (CNPJ == true)
                    {
                        valido = StringUtil.ValidarCNPJ(strValue);
                    }
                    else
                    {
                        valido = StringUtil.ValidarCPF(strValue);
                    }

                    if (!valido)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                    else
                        return ValidationResult.Success;
                }
            }

            return ValidationResult.Success;
        }
        
    }
}
