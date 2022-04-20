using Coad.GenericCrud.Validations;
using GenericCrud.Validations.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Validations.Base
{
    public static class ValidationExpressions
    {
        public static IValidationExpression RequiredIfNullOrEmpty(string property)
        {
            return new RequiredIfNullOrEmptyAttribute(property);
        }
    }
}
