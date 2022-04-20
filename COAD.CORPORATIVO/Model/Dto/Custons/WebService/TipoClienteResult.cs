using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService
{
    [DataContract]
    public class TipoClienteResult : WebServiceResult
    {
        [DataMember]
        public IList<ClienteIntegrTipoClienteDTO> TiposClientes { get; set; }
    }
}
