using COAD.CORPORATIVO.Service.Base;
using COAD.CORPORATIVO.Settings.Mundipagg;
using COAD.SEGURANCA.Service;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Mundipagg
{
    public class PlanoItemMundipaggSRV : MundipaggSRV
    {

        public override MundipaggStt MundipaggSTT { get; set; }

        public void DeletarPlanoItemMundipagg(string planoId, string planoItemId)
        {
            try { 
                var client = new MundiAPIClient(ParametrosChaveMundipaggDTO.SecretKey, "");
            var response = client.Plans.DeletePlanItem(planoId, planoItemId);
            }catch(Exception)
            {

            }
        }

        public GetPlanItemResponse CadastrarPlanoItem(string planoId, CreatePlanItemRequest createPlanItemRequest)
        {
            var client = new MundiAPIClient(ParametrosChaveMundipaggDTO.SecretKey, "");
            var response = client.Plans.CreatePlanItem(planoId, createPlanItemRequest);
            return response;
        }
    }
}
