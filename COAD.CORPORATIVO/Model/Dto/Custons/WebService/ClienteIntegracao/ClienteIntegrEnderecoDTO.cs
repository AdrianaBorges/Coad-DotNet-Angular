using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao
{
    [DataContract]
    public class ClienteIntegrEnderecoDTO
    {
        public ClienteIntegrEnderecoDTO()
        {
        }

        [DataMember]
        [Required(ErrorMessage = "Informe o cep")]
        [MaxLength(8, ErrorMessage = "O Cep deve conter no máximo 8 caracteres")]
        [MinLength(6, ErrorMessage = "O Cep deve possuir no mínimo 6 caracteres")]
        public string CEP { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Informe o Município")]
        public ClienteIntegrMunicipioDTO Municipio { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Informe a UF")]
        public string UF { get; set; }
        
        [DataMember]
        [Required(ErrorMessage = "Informe o bairro")]
        public string Bairro { get; set; }
        
        [DataMember]
        [Required(ErrorMessage = "Informe o logradouro")]
        public string Logradouro { get; set; }

        [DataMember]
        [MaxLength(6, ErrorMessage = "O número deve ter no máximo 6 dígitos")]
        public string Numero { get; set; }
                
        [DataMember]
        public string Complemento { get; set; }

        public bool IsEmpty()
        {
            return (
                    string.IsNullOrWhiteSpace(CEP) &&
                    Municipio == null &&
                    string.IsNullOrWhiteSpace(UF) &&
                    string.IsNullOrWhiteSpace(Bairro) &&
                    string.IsNullOrWhiteSpace(Logradouro) &&
                    string.IsNullOrWhiteSpace(Numero) &&
                    string.IsNullOrWhiteSpace(Complemento)
                );
        }
    }
}
