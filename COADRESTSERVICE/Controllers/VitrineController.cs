using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Service.Custons;
using COADRESTSERVICE.Auth;
using COADRESTSERVICE.Auth.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COADRESTSERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VitrineController : ControllerBase
    {
        private UserContext UserContext { get; set; }

        public VitrineController(
            ProdutoComposicaoSRV ProdutoComposicao,
            UserContext UserContext)
        {
            this.ProdutoComposicao = ProdutoComposicao;
            this.UserContext = UserContext;
        }

        private ProdutoComposicaoSRV ProdutoComposicao { get; set; }

        // GET: api/Vitrine/listarProdutosVitrine
        [EcommerceBasicAuth(false)]
        [HttpGet("[action]")]
        public JSONResponse ListarProdutosVitrine(
            [FromQuery] string nomeProduto = null,
            [FromQuery] int pagina = 1,
            [FromQuery] int registrosPorPagina = 12)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                int? cliId = null;
                if (User.Identity.IsAuthenticated)
                {
                    cliId = UserContext.Usuario.CliId;
                }

                var lstProdutosVitrine = ProdutoComposicao.ListarProdutosVitrine(nomeProduto, pagina, registrosPorPagina, cliId);
                response.AddPage("lstProdutosVitrine", lstProdutosVitrine);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return response;
        }
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Vitrine/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // GET: api/Vitrine/buscarProduto
        [EcommerceBasicAuth(false)]
        [HttpGet("[action]")]
        public JSONResponse BuscarProduto(
            [FromQuery] int id = 0, [FromQuery] int idCli = 0)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                var produto = ProdutoComposicao.BuscarProduto(id, idCli);
                response.Add("produto", produto);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return response;
        }

        // POST: api/Vitrine
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Vitrine/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
