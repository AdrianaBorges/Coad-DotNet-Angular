using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService
{
    [DataContract]
    public class LoginUnicoClienteDTO
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string CPF { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Telefone { get; set; }
        [DataMember]
        public string Trab { get; set; }
        [DataMember]
        public string Profissao { get; set; }
        [DataMember]
        public string Sexo { get; set; }
        [DataMember]
        public DateTime? DataNascimento { get; set; }
        [DataMember]
        public string AreaAtuacao { get; set; }
        
        [DataMember]
        public LoginUnicoEnderecoClienteDTO Endereco {get; set;}
    
    }
}
