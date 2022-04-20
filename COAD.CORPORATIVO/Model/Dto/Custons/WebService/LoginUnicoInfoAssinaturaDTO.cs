using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService
{
    [DataContract]
    public class LoginUnicoInfoAssinaturaDTO
    {
        [DataMember]
        public string CodAssinatura { get; set; }
        [DataMember]
        public string Senha { get; set; }
    }
}
