using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao
{
    [DataContract]
    public class ClienteIntegrMunicipioDTO
    {
        [DataMember]
        public int? CodigoMunicipio { get; set; }

        [DataMember]
        public string DescricaoMunicipio { get; set; }

        [DataMember]
        public string CodigoIBGE { get; set; }
    }
}
