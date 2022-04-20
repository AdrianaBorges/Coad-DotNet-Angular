using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect
{
    public class ClienteProspectEnderecoDTO
    {
        public int? TipoEnd { get; set; }

        [Required(ErrorMessage = "Informe o CEP")]
        [StringLength(8,MinimumLength = 8, ErrorMessage = "O CEP deve possuir 8 caracteres")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Informe o Município")]
        public string Municipio { get; set; }

        [Required(ErrorMessage = "Informe a UF")]
        public string UF { get; set; }

        [Required(ErrorMessage = "Informe o Bairro")]
        [TextValidator(ErrorMessage = "O Bairro não pode possuir caracteres especiais e espaço no início e fim do texto.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O Bairro deve possuir no máximo 60 caracteres")]
        public string Bairro { get; set; }
        public string TipoLogradouro { get; set; }

        [Required(ErrorMessage = "Informe o Logradouro")]
        [TextValidator(ErrorMessage = "O Logradouro não pode possuir caracteres especiais e espaço no início e fim do texto.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O Logradouro deve possuir no mínimo 2 e no máximo 60 caracteres")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "Informe o Número")]
        [MaxLength(6, ErrorMessage = "O número deve ter no máximo 6 dígitos")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Código do Município não Encontrado")]
        public int? MunId { get; set; }

        [TextValidator(ErrorMessage = "O Complemento não pode possuir caracteres especiais e espaço no início e fim do texto.")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O Complemento deve possuir no máximo 60 caracteres")]
        public string Complemento { get; set; }

        public bool Excluir { get; set; }
        public MunicipioDTO Munic { get; set; }

        public bool IsEmpty()
        {
            //return (
            //        string.IsNullOrWhiteSpace(CEP) &&
            //        string.IsNullOrWhiteSpace(Municipio) &&
            //        string.IsNullOrWhiteSpace(UF) &&
            //        string.IsNullOrWhiteSpace(Bairro) &&
            //        string.IsNullOrWhiteSpace(Logradouro) &&
            //        string.IsNullOrWhiteSpace(Numero) &&
            //        string.IsNullOrWhiteSpace(Complemento)
            //    );
            return false;
        }
    }
}
