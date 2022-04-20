using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{
    public class ClienteMundipaggDTO
    {
        [JsonProperty("client_id")]
        public int? ClientId { get; set; }
        [JsonRequired]
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("document")]
        public string Document { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("address")]
        public EnderecoMundipaggDTO EnderecoMundipaggDTO { get; set; }

    }
}
