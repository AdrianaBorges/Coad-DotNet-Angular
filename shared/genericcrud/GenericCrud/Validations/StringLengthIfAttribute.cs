using GenericCrud.Validations.Base;
using GenericCrud.Validations.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Coad.GenericCrud.Validations
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class StringLengthIfAttribute : StringLengthAttribute
    {
        private String PropertyName { get; set; }
        private String OriginalMessage { get; set; }
        public int minimunLength {get; set;}
        //private Object DesiredValue { get; set; }        
        private Object[] DesiredValues { get; set; }
        public string NullOrEmptyPropertyName { get; set; }

        public StringLengthIfAttribute(int maximunLength, String propertyName, params Object[] desiredvalue) : base(maximunLength)
        {
            
            PropertyName = propertyName;
            DesiredValues = desiredvalue;

            if (ErrorMessage != null)
            {
                
                base.ErrorMessage = ErrorMessage;
                OriginalMessage = ErrorMessage;
            }
                
        }
                

        private ValidationResult Validar(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance);

            if (proprtyvalue == null || DesiredValues == null || DesiredValues.Count() <= 0)
            {
                return ValidationResult.Success;
            }


            int index = 0;

            
            foreach (var desiredValue in DesiredValues)
            {
                if (proprtyvalue.ToString() == desiredValue.ToString())
                {
                    if (!string.IsNullOrWhiteSpace(NullOrEmptyPropertyName))
                    {

                        var _extraExpression = ValidationExpressions.RequiredIfNullOrEmpty(NullOrEmptyPropertyName);

                        if (_extraExpression != null && !_extraExpression.NeedToValidate(value, context))
                        {
                            return ValidationResult.Success;
                        }

                    }

                    if(string.IsNullOrWhiteSpace(OriginalMessage)){

                        OriginalMessage = base.ErrorMessage;
                    }

                    if (!string.IsNullOrWhiteSpace(OriginalMessage))
                    {
                        string parsedMessage = parseRegex(index.ToString(), OriginalMessage);
                        base.ErrorMessage = parsedMessage;
                    }
                    else
                    {
                        base.ErrorMessage = ClearRegex(base.ErrorMessage);
                    }

                    ValidationResult result = base.IsValid(value, context);
                    return result;
                }
                index++;
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

        public bool NeedToValidate(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance);

            return !(proprtyvalue != null && DesiredValues != null && DesiredValues.Count() > 0) ;
            
        }
    }
}