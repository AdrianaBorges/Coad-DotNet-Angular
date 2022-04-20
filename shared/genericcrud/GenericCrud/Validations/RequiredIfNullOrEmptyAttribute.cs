using GenericCrud.Validations.interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Coad.GenericCrud.Validations
{
    /// <summary>
    /// Valida um campo caso o atributo do teste seja nulo 
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RequiredIfNullOrEmptyAttribute : RequiredAttribute, IValidationExpression
    {
        private String PropertyName { get; set; }
        private String OriginalMessage { get; set; }
        //private Object DesiredValue { get; set; }           
      
        public RequiredIfNullOrEmptyAttribute(String propertyName)
        {
            
            PropertyName = propertyName;

            if (ErrorMessage != null)
            {
                
                base.ErrorMessage = ErrorMessage;
                OriginalMessage = ErrorMessage;
            }
                
        }


        /// <summary>
        /// Verifica se a propriedade de teste é nula para indicar para o filtro se ele deve ou não validar
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool NeedToValidate(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance);

            // se o valor da propriedade for null ou uma string vazia.
            return (proprtyvalue == null || (proprtyvalue is string && string.IsNullOrWhiteSpace((string)proprtyvalue)));
        }

        private ValidationResult Validar(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance);

            // se o valor da propriedade for null ou uma string vazia.
            if(NeedToValidate(value, context)){

                ValidationResult result = base.IsValid(value, context);
                return result;
            }

            
            return ValidationResult.Success;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {            
            return Validar(value, context);
        }

        public string parseRegex(string index, string message)
        {
            var regexPattern = @"(\{(" + index +  @")\:(\s?[^}]*)\})";    // pega o padrão {numeral: descrição} ex: {2: Cpf} 
            var cleanPattern = @"(\{(\d)\:(\s?[^}]*)\})"; // limpa qualquer outro padrão indicado acima ainda não convertido
            Regex rgx = new Regex(regexPattern);
            Regex clearRgx = new Regex(cleanPattern);
            var result = rgx.Replace(message, "$3");
            result = clearRgx.Replace(result, "");

            return result;
        }

        public string ClearRegex(string message)
        {
            var cleanPattern = @"(\{(\d)\:(\s?[^}]*)\})";
            Regex clearRgx = new Regex(cleanPattern);
            var result = clearRgx.Replace(message, "");

            return result;
        }
    }
}