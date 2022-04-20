using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Service.Mundipagg;
using COADRESTSERVICE.Auth;
using COADRESTSERVICE.Auth.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace COADRESTSERVICE.Controllers.Mundipagg
{

    [Route("api/[controller]")]
    [ApiController]
    public class MundipaggConsultaController : ControllerBase
    {
        private UserContext UserContext { get; set; }
        private ConsultaSRV _consultaSRV { get; set; }

        public MundipaggConsultaController(
            ConsultaSRV consultaSRV,
            UserContext UserContext
            )
        {
            this._consultaSRV = consultaSRV;
            this.UserContext = UserContext;
        }


        [HttpGet("teste-consulta")]
        public JSONResponse TesteConsulta( string chavePedido )
        {

            JSONResponse response = new JSONResponse();

            if (this._consultaSRV.TesteConsulta( chavePedido ))
                response.message = Message.Success( "O acesso funcionou e achou o solicitado! Data e hora: " + DateTime.Now.ToString() );
            else
                response.message = Message.Success( "O acesso funcionou, mas não achou o solicitado! Data e hora: " + DateTime.Now.ToString());


            return response;

        }

    }
}
