using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService
{
    [DataContract]
    public class LoginUnicoMensagemDTO
    {
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public string Mensagem { get; set; }

        public LoginUnicoMensagemDTO()
        {

        }

        public LoginUnicoMensagemDTO(string tipo, string mensagem)
        {
            this.Tipo = tipo;
            this.Mensagem = mensagem;
        }
    }
}
