using COAD.CORPORATIVO.Service.Mundipagg;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COADRESTSERVICE.Auth;
using Microsoft.AspNetCore.Mvc;
using Coad.GenericCrud.ActionResultTools;

namespace COADRESTSERVICE.Controllers.Mundipagg
{

    [Route("api/[controller]")]
    [ApiController]
    public class MundipaggCancelamentoController
    {

        private UserContext UserContext { get; set; }
        private CancelamentoSRV _cancelamentoSRV { get; set; }

        public MundipaggCancelamentoController(
            CancelamentoSRV cancelamentoSRV,
            UserContext UserContext
            )
        {
            this._cancelamentoSRV = cancelamentoSRV;
            this.UserContext = UserContext;
        }


        [HttpGet("teste-cancelamento")]
        public JSONResponse TesteCancelamento(string chavePedido, string chaveTransacao)
        {

            JSONResponse response = new JSONResponse();

            if ( this._cancelamentoSRV.TesteCancelamento( chavePedido, chaveTransacao ) )
                response.message = Message.Success( "O acesso funcionou e cancelamento feito! Data e hora: " + DateTime.Now.ToString() );
            else
                response.message = Message.Success( "O acesso funcionou, mas não houve cancelamento! Data e hora: " + DateTime.Now.ToString() );

            return response;

        }


    }


}
