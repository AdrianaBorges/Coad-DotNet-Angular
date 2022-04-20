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
    public class CriarClienteMundipaggRequestDTO
    {
   
        [JsonProperty("create_customer")]
        public CreateCustomerRequest CreateCustomerRequest { get; set; }

        //        [Required]
        //        [JsonProperty("cliente_integracao")]
        //        public ClienteIntegrDTO ClienteIntegrDTO { get; set; }

        [Required]
        [JsonProperty("client")]
        public ClienteMundipaggDTO ClienteMundipaggDTO { get; set; }

    }
}
