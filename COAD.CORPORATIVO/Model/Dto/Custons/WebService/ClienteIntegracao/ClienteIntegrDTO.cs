using Coad.GenericCrud.Validations;
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
    public class ClienteIntegrDTO
    {
        public ClienteIntegrDTO()
        {
        }
        
        [DataMember]
        public int? ClienteId { get; set; }
        
        [DataMember]
        //[Required(ErrorMessage = "Informe o nome")]
        [Required(ErrorMessage = "Informe o nome.")]
        public string Nome { get; set; }
        [DataMember]
		[MaxLength(14, ErrorMessage = "O CNPJ/CPF deve conter no máximo 14 dígitos")]
        public string CNPJ_CPF { get; set; }
        [DataMember]
        public string A_C { get; set; }

        [DataMember]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string EmailPagamento { get; set; }
        [DataMember]
        
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string EmailConsulta { get; set; }

        //[DataMember]
        //[Required(ErrorMessage = "Informe o tipo de Cliente")]
        //public int? TipoCliente { get; set; }

        [DataMember]
        public ClienteIntegrTipoClienteDTO TipoCliente { get; set; }

        public string DescricaoTipoCliente { get; set; }

        [DataMember]
        public string EmailNewsLetter { get; set; }

        [DataMember]
        public ClienteIntegrEnderecoDTO EnderecoFaturamento { get; set; }
        [DataMember]
        public ClienteIntegrEnderecoDTO EnderecoEntrega { get; set; }

        [DataMember]
        public ClienteIntegrTelefoneDTO Telefone { get; set; }
        [DataMember]
        public ClienteIntegrTelefoneDTO Celular { get; set; }
        [DataMember]
        public ClienteIntegrTelefoneDTO Fax { get; set; }
    }
}
