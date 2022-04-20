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
    public class PlanoMundipaggSRV : MundipaggSRV
    {
        public ParametrosSRV _parametrosSRV { get; set; }
        public override MundipaggStt MundipaggSTT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public GetPlanResponse CadastrarPlano(CreatePlanRequest planoMundipagg)
        {
            var chave = ParametrosChaveMundipaggDTO.SecretKey;
            var client = new MundiAPIClient(chave, "");
            var response = client.Plans.CreatePlan(planoMundipagg);
            return response;
        }

        public void RemoverPlano(string planoMundipaggId)
        {
            var client = new MundiAPIClient(ParametrosChaveMundipaggDTO.SecretKey, "");
            client.Plans.DeletePlan(planoMundipaggId);
        }
    }
}
