using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoletoNet;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Service.Mundipagg;
using COADRESTSERVICE.Auth;
using COADRESTSERVICE.Auth.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COADRESTSERVICE.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MundipaggBoletoController : ControllerBase
    {

        private UserContext UserContext { get; set; }
        private BoletoSRV _boletoSRV { get; set; }

        public MundipaggBoletoController(
            BoletoSRV boletoSRV,
            UserContext UserContext
            )
        {
            this._boletoSRV = boletoSRV;
            this.UserContext = UserContext;
        }

        [HttpGet("teste-boleto")]
        public JSONResponse TesteEnviarBoleto()
        {

            JSONResponse response = new JSONResponse();

            if ( this._boletoSRV.TesteEnviarBoleto() )
                response.message = Message.Success( "O acesso funcionou e gerou um boleto! Data e hora: " + DateTime.Now.ToString() );
            else
                response.message = Message.Success( "O acesso funcionou, mas não gerou boleto! Data e hora: " + DateTime.Now.ToString() );


            return response;

        }

        [HttpGet("teste")]
        public void teste()
        {

            Console.Write("EEEEEEEEEEEEEEEEEEEEEEE");

        }


    }


}
