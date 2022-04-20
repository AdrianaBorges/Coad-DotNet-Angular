using System.ComponentModel.DataAnnotations;
using COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect;

namespace COAD.CORPORATIVO.Validations
{
    /// <summary>
    /// Valida se o cliente é isento de inscrição estadual
    /// </summary>
    public class ClienteIsentoDeInscricaoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is ClienteProspectDTO)
            {
                var clienteProspect = validationContext.ObjectInstance as ClienteProspectDTO;

                // se a inscrição estadual não for preenchida
                if (string.IsNullOrEmpty(clienteProspect.InscricaoEstadual))
                {
                    // verifica se o cliente está marcado como Isento
                    if (clienteProspect.RequerInscricaoEstadual)
                    {
                        return new ValidationResult("Informe o número da Inscrição Estadual ou isente o cliente.");
                    }
                }

            }

            return ValidationResult.Success;

        }

    }
}
