using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Service.Base;
using COAD.CORPORATIVO.Settings.Mundipagg;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Mundipagg
{
    public class CartaoMundipaggSRV : MundipaggSRV
    {
        public override MundipaggStt MundipaggSTT { get; set; }

        public GetCardResponse CadastrarCartao(CriarCartaoMundipaggDTO criarCartaoMundipaggDTO)
        {
            try { 
            var IdClienteMundipagg = criarCartaoMundipaggDTO.IdCliente;
            var CartaoMundipagg = criarCartaoMundipaggDTO.Cartao;

            var apiCliente = new MundiAPIClient(MundipaggSTT.SecretKey, "");

            var response = apiCliente.Customers.CreateCard(IdClienteMundipagg, CartaoMundipagg);

            return response;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao processar a operação: " + e.Message);
            }

        }

        public BuscarPlanoItemMundipaggDTO CadastrarPlanoItem(CriarPlanoItemMundipaggDTO criarPlanoItemMundipaggDTO)
        {
            BuscarPlanoItemMundipaggDTO buscarPlanoItemMundipaggDTO = new BuscarPlanoItemMundipaggDTO();
            try
            {
                var apiCliente = new MundiAPIClient(MundipaggSTT.SecretKey, "");

                var getPlanItemResponse = apiCliente.Plans.CreatePlanItem(criarPlanoItemMundipaggDTO.PlanId, criarPlanoItemMundipaggDTO.CreatePlanItemRequest);

                buscarPlanoItemMundipaggDTO.GetPlanItemResponse = getPlanItemResponse;

                return buscarPlanoItemMundipaggDTO;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao processar a operação: " + e.Message);
            }
        }

        public BuscarPlanoDTO CadastrarPlano(CriarPlanoMundipaggDTO criarPlanoMundipaggDTO)
        {
            BuscarPlanoDTO buscarPlanoDTO = new BuscarPlanoDTO();
            try
            {
                var apiCliente = new MundiAPIClient(MundipaggSTT.SecretKey, "");

                var plan = criarPlanoMundipaggDTO.CreatePlanRequest;

                var response = apiCliente.Plans.CreatePlan(plan);
                buscarPlanoDTO.GetPlanResponse = response;

                return buscarPlanoDTO;

            }
            catch(Exception e)
            {
                throw new Exception("Erro ao processar a operação: " + e.Message);
            }

        }
    }
}
