using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Validations
{
    public class PresentDateAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime || value is Nullable<DateTime>)
            {
                if (((DateTime) value).Date >= DateTime.Now.Date)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else 
            if (value == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Esse campo não é do tipo datetime");
            }

            //return ValidationResult.Success;
        }
    }
}
