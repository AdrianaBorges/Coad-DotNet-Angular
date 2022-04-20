using MundiAPI.PCL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{
    public class BuscarPlanoDTO
    {

        [JsonProperty("plan")]
        public GetPlanResponse GetPlanResponse { get; set; }

    }
}
