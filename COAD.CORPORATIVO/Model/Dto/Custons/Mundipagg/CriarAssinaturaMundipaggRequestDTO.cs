using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using MundiAPI.PCL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{
    public class CriarAssinaturaMundipaggRequestDTO
    {

        [JsonProperty("create_subscription")]
        public CreateSubscriptionRequest CreateSubscriptionRequest { get; set; }

//        [JsonProperty("cliente_integracao")]
//        public ClienteIntegrDTO ClienteIntegrDTO { get; set; }

        [JsonProperty("client")]
        public ClienteMundipaggDTO ClienteMundipaggDTO { get; set; }
        [JsonProperty("plan_coad_id")]
        public string PlanoCoadId { get; set; }

    }
}
