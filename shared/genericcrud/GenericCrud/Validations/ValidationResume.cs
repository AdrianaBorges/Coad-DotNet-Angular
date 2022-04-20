using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GenericCrud.Validations
{
    public class ValidationResume
    {
        public bool IsValid { get; set; }
        public ICollection<ValidationResult> ValidationResult { get; set; }
        public ICollection<ValidationContext> validationContext { get; set; }
        public ModelStateDictionary ModelState { get; set; }
        
        public ValidationResume()
        {
            this.validationContext = new HashSet<ValidationContext>();
            ModelState = new ModelStateDictionary();
        }

        public ValidationResume(bool isValid, ICollection<ValidationResult> validationResult, ICollection<ValidationContext> validationContext) 
            : this() 
        {
            
            IsValid = isValid;
            ValidationResult = validationResult;
            this.validationContext = validationContext;
            PreencherModelState();
        }

        public ValidationResume(bool isValid, ICollection<ValidationResult> validationResult, ValidationContext validationContext)
            : this() 
        {
            IsValid = isValid;
            ValidationResult = validationResult;
            this.validationContext = new HashSet<ValidationContext>() { validationContext};
            PreencherModelState();
        }

        public void PreencherModelState()
        {
            if (ValidationResult != null)
            {
                foreach (var valResult in ValidationResult)
                {
                    foreach(var name in valResult.MemberNames)
                    {
                        ModelState.AddModelError(name, valResult.ErrorMessage);
                    }
                }
            }
        }

        public override string ToString()
        {
            string Message = "";

            if(ValidationResult != null)
            {
                var sb = new StringBuilder();
                foreach(var validationResult in ValidationResult)
                {
                    
                    sb.Append(validationResult.ErrorMessage);
                    sb.Append("\r\n, ");
                }

                return Message + sb.ToString();
            }
                
                return Message;
        }
        
    }
}
