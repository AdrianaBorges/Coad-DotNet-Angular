using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using COADRESTSERVICE.Auth;
using COADRESTSERVICE.Auth.Filters;
using Microsoft.AspNetCore.Mvc;
using COAD.CORPORATIVO.Service.Mundipagg;
using Coad.GenericCrud.ActionResultTools;

namespace COADRESTSERVICE.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MundipaggCartaoCreditoController
    {

        private UserContext UserContext { get; set; }
        private CartaoCreditoSRV _cartaoCreditoSRV { get; set; }

        public MundipaggCartaoCreditoController(
            CartaoCreditoSRV cartaoCreditoSRV,
            UserContext UserContext
            )
        {
            this._cartaoCreditoSRV = cartaoCreditoSRV;
            this.UserContext = UserContext;
        }

        [HttpGet("teste-cartao-credito")]
        public JSONResponse TesteCartaoCredito()
        {

            JSONResponse response = new JSONResponse();

            if (this._cartaoCreditoSRV.TesteCartaoCredito())
                response.message = Message.Success( "O acesso funcionou e gerou uma operação! Data e hora: " + DateTime.Now.ToString());
            else
                response.message = Message.Success( "O acesso funcionou, mas não gerou operação! Data e hora: " + DateTime.Now.ToString());


            return response;

        }

    }
}
