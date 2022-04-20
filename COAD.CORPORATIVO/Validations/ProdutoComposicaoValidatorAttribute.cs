using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Validations
{
    public class ProdutoComposicaoValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is int)
            {
                int CMP_ID = (int) value;

                var produtoComposicaoSRV = new ProdutoComposicaoSRV();
                
                var produtoComposto = produtoComposicaoSRV.FindById(CMP_ID);

                if (produtoComposto == null)
                {
                    return ValidationResult.Success;
                }
                //else if (produtoComposto.PRODUTOS != null)
                //{
                //    var produto = produtoComposto.PRODUTOS;

                //    if (produto.DATA_EXCLUSAO != null)
                //    {
                //        return new ValidationResult("O produto selecionado encontra-se desativado.");
                //    }

                //    if (!produto.PRO_VENDA)
                //    {
                //        return new ValidationResult("O produto selecionado não é um produto de venda.");
                //    }

                //}
                else
                {
                    return new ValidationResult("O produto selecionado não pode ser vendido por falta de dados. (Falta de produto base)");
                }
               
            }
            else
            {
                return new ValidationResult("Esse validator só se aplica a ProdutosDTO");
            }

            return ValidationResult.Success;
                
        }
    }
}
