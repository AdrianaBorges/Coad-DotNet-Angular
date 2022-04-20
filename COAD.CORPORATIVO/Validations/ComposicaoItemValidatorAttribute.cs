using COAD.CORPORATIVO.Model.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Validations
{
    public class ComposicaoItemValidatorAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IEnumerable<object>)
            {
                IEnumerable<ProdutoComposicaoItemDTO> lst = value as IEnumerable<ProdutoComposicaoItemDTO>;
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var composicaoItem in lst)
                    {

                    }
                }
                else
                {
                    return new ValidationResult(base.ErrorMessage);
                }
            }
            return new ValidationResult("Esse atributo não é uma collection");

        }
    }
}
