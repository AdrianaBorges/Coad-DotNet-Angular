using GenericCrud.Validations.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Validations
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RangeIfAttribute : RangeAttribute
    {
        private RequiredIfBase requiredBase { get; set; }
        private string propertyName { get; set; }
        private object[] values { get; set; }

        public RangeIfAttribute(int min, int max, string propertyName, params object[] values) : base(min, max)
        {
            requiredBase = new RequiredIfBase();
            this.propertyName = propertyName;
            this.values = values;
        }

        public RangeIfAttribute(double min, double max, string propertyName, params object[] values) : base(min, max)
        {
            requiredBase = new RequiredIfBase();
            this.propertyName = propertyName;
            this.values = values;
        }

        public RangeIfAttribute(int min, int max) : base(min, max)
        {
            requiredBase = new RequiredIfBase();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(requiredBase.PrecisaValidar(value, propertyName, null, values, validationContext))
                return base.IsValid(value, validationContext);
            return ValidationResult.Success;
        }
    }
}
