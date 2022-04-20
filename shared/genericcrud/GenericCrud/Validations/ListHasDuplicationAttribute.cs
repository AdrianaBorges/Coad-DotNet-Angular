using GenericCrud.Models.Comparators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Validations
{

    public class ListHasDuplicationAttribute : RequiredAttribute
    {
        private string[] keys { get; set; }

        public ListHasDuplicationAttribute(params string[] keys)
        {
            this.keys = keys;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IEnumerable<object>)
            {
                IEnumerable<object> lst = value as IEnumerable<object>;
                if (lst != null && lst.Count() > 0)
                {
                    var group = lst.GroupBy(x => x, new GenericComparator<object>(keys)).
                        Select(x => new { Count = x.Count(), value = x.Key });
                    var count = group.Where(x => x.Count > 1).Count();

                    if (count > 0)
                    {
                        return new ValidationResult(base.ErrorMessage);
                    }

                    return ValidationResult.Success;

                }
                else{
                    return ValidationResult.Success;
                }
            }
            else if (value == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Esse atributo não é uma collection");
                       
        }
    }
}
