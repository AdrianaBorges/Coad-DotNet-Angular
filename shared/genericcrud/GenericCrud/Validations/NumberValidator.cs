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
    public class NumberValidatorAttribute : ValidationAttribute
    { 
        
        public NumberValidatorAttribute()
        {
            
        }

        public override bool IsValid(object value)
        {
            if (value != null && value is string)
            {
                Regex rgx = new Regex(@"([^\d\s])");
                var result = rgx.IsMatch((string)value);
                return !result;


            }    
                return true;

        }
    }
}
