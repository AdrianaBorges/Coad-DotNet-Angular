using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Validations.interfaces
{
    public interface IValidationExpression
    {
        bool NeedToValidate(object value, ValidationContext context);
    }
}
