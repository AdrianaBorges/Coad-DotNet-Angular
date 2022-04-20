using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using Newtonsoft.Json;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{
    public class ClienteMindipaggResponseDTO
    {

        [JsonProperty("cliente-integracao")]
        public ClienteIntegrDTO ClienteIntegrDTO { get; set; }
        [JsonProperty("cliente-mundipagg-id")]
        public string ClienteIdMundipagg { get; set; }
    }
}
