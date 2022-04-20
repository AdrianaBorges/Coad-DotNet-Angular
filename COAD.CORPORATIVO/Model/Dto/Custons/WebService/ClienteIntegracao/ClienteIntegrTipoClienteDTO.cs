using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao
{
    [DataContract]
    public class ClienteIntegrTipoClienteDTO
    {
        [DataMember]
        public int? CodigoTipoCliente { get; set; }
        
        [DataMember]
        public string DescricaoTipoCliente { get; set; }
    }
}
