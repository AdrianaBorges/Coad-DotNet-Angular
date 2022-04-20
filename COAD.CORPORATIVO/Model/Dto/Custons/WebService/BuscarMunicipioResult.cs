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
    public class BuscarMunicipioResult : WebServiceResult
    {
        [DataMember]
        public IList<ClienteIntegrMunicipioDTO> Municipios { get; set; }
    }
}
