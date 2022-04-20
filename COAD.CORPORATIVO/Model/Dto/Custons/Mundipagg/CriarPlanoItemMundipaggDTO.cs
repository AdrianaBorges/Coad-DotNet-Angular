using MundiAPI.PCL.Models;
using Newtonsoft.Json;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{
    public class CriarPlanoItemMundipaggDTO
    {
        [JsonProperty("plan-id")]
        public string PlanId { get; set; }

        [JsonProperty("plan-item")]
        public CreatePlanItemRequest CreatePlanItemRequest { get; set;}
    }
}
