using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Validations
{

    public class RequiredListAttribute : RequiredAttribute
    {
        public int? MinSize { get; set; }
        public int? MaxSize { get; set; }

        public RequiredListAttribute()
        {
            MinSize = 1;
            MaxSize = null;
        }
        public RequiredListAttribute(int MinSize = 1)
        {
            this.MinSize = MinSize;

            if (MaxSize != null)
            {
                this.MaxSize = (int) MaxSize;
            }            
        }

        public RequiredListAttribute(int MinSize, int maxSize)
        {
            this.MinSize = MinSize;

            if (MaxSize != null)
            {
                this.MaxSize = (int)MaxSize;
            }
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IList<string> memberName = new List<string>() { validationContext.MemberName };
            if (value is IEnumerable<object>)
            {
                IEnumerable<object> lst = value as IEnumerable<object>;
                if (lst != null && lst.Count() >= MinSize)
                {
                    if (MinSize != null && (MaxSize == null || lst.Count() <= MaxSize))
                    {
                        ValidationResult result = ValidationResult.Success;

                        return result;
                    }
                    else
                    {
                        return new ValidationResult(base.ErrorMessage, memberName);
                    }                    
                }
                else{
                    return new ValidationResult(base.ErrorMessage, memberName);
                }
            }
            else if (value == null)
            {
                return new ValidationResult(base.ErrorMessage, memberName);
            }
            return new ValidationResult("Esse atributo não é uma collection");
                       
        }
    }
}
