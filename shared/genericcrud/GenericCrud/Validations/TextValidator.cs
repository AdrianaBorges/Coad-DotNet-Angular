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
    public class TextValidatorAttribute : ValidationAttribute
    { 
        
        public TextValidatorAttribute()
        {
            
        }

        public override bool IsValid(object value)
        {
            if (value != null && value is string)
            {
                Regex rgx = new Regex(@"(\s)*[\/\\\^=#$%¨&*+§!?@\{\}\[\]](\s)*");
                var result = rgx.IsMatch((string)value);
                return !result;


            }    
                return true;

        }
    }
}
