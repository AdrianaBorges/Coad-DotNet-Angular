using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Service.Mundipagg;
using COAD.CORPORATIVO.Settings.Mundipagg;
using COADRESTSERVICE.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MundiAPI.PCL.Models;
using System;

namespace COADRESTSERVICE.Controllers.Mundipagg
{
    [Route("api/mundipagg/[controller]")]
    [ApiController]
    public class CartaoMundipaggController : MundipaggBaseController
    {
        public override MundipaggStt MundipaggStt { get; set; }
        public CartaoMundipaggSRV CartaoMundipaggSRV { get; }

        public CartaoMundipaggController(IConfiguration configuration, CartaoMundipaggSRV cartaoMundipaggSRV) : base(configuration)
        {
            this.CartaoMundipaggSRV = cartaoMundipaggSRV;
            this.CartaoMundipaggSRV.MundipaggSTT = this.MundipaggStt;
        }

        [HttpPost("cadastrar-cartao")]
        public JSONResponse CadastrarCartao(CriarCartaoMundipaggDTO criarCartaoMundipaggDTO)
        {

            var result = new JSONResponse();

            try
            {
                var cartao = CartaoMundipaggSRV.CadastrarCartao(criarCartaoMundipaggDTO);
                result.success = true;
                result.Add("data", cartao);
            }
            catch(Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
            }

            return result;
        }

        [HttpPost("cadastrar-plano-item")]
        public JSONResponse CadastrarPlanoItem(CriarPlanoItemMundipaggDTO criarPlanoItemMundipaggDTO)
        {
            var result = new JSONResponse();
            try
            {
                var cartao = CartaoMundipaggSRV.CadastrarPlanoItem(criarPlanoItemMundipaggDTO);
                result.success = true;
                result.Add("data", cartao);
            }
            catch (Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
            }
            return result;
        }

        [HttpPost("cadastrar-plano")]
        public JSONResponse CadastrarPlano(CriarPlanoMundipaggDTO criarPlanoMundipaggDTO)
        {
            var result = new JSONResponse();
            try
            {
                var cartao = CartaoMundipaggSRV.CadastrarPlano(criarPlanoMundipaggDTO);
                result.success = true;
                result.Add("data", cartao);
            }
            catch (Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
            }
            return result;
        }

    }

}
