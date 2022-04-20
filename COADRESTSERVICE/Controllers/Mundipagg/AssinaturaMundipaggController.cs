using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Service.Mundipagg;
using COAD.CORPORATIVO.Settings.Mundipagg;
using COADRESTSERVICE.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace COADRESTSERVICE.Controllers.Mundipagg
{
    [Route("api/mundipagg/[controller]")]
    [ApiController]
    public class AssinaturaMundipaggController : MundipaggBaseController
    {
        private ClienteIntegracaoSRV _clienteIntegracaoSRV { get; set; }

        private AssinaturaMundipaggSRV _assinaturaMundipaggSRV { get; set; }

        public override MundipaggStt MundipaggStt { get; set; }

        public AssinaturaMundipaggController(IConfiguration configuration, ClienteIntegracaoSRV clienteIntegracaoSRV, AssinaturaMundipaggSRV assinaturaMundipaggSRV) : base (configuration)
        {
            this._clienteIntegracaoSRV = clienteIntegracaoSRV;
            this._assinaturaMundipaggSRV = assinaturaMundipaggSRV;
            _assinaturaMundipaggSRV.MundipaggSTT = MundipaggStt;
        }

        [HttpPost("cadastrar-assinatura-plano")]
        public JSONResponse CadastrarAssinaturaPlano(CriarAssinaturaMundipaggRequestDTO criarAssinaturaRequestDTO)
        {

            var result = new JSONResponse();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var data = _assinaturaMundipaggSRV.CriarAssinatura(criarAssinaturaRequestDTO);
                    result.Add("data", data);
                    result.success = true;
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
            }
            return result;
        }

    }
}