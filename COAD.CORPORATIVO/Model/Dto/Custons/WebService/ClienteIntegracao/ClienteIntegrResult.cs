using COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao
{
    [DataContract]
    public class ClienteIntegrResult : WebServiceResult
    {
        [DataMember]
        public ClienteIntegrDTO Cliente { get; set; } 

    }
}
