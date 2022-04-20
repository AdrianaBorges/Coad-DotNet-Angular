using Coad.GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect
{
    public class ClienteProspectTelefoneDTO
    {
        public int? CodigoTelefone { get; set; }

        [Required(ErrorMessage = "Informe o Tipo")]
        public int? TipoTelefone { get; set; }
        
        [Required(ErrorMessage = "Informe o DDD")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "O DDD deve possuir no máximo 3 caracteres")]
        public string DDD { get; set; }

        [Required(ErrorMessage = "Informe o Telefone")]
        [StringLength(11, MinimumLength = 6, ErrorMessage = "O Telefone deve possuir no mínimo 6 e no máximo 11 caracteres")]
        public string Telefone { get; set; }
        public string Contato { get; set; }
        public string Ramal { get; set; }
        public int? TipoAtendimento { get; set; }
        public bool IsEmpty
        {
            get {
                return (
                
                    string.IsNullOrWhiteSpace(DDD) &&
                    string.IsNullOrWhiteSpace(Telefone) &&
                    string.IsNullOrWhiteSpace(Contato) &&
                    string.IsNullOrWhiteSpace(Ramal)
                );
            }

            set { }
        }
        
    }
}
