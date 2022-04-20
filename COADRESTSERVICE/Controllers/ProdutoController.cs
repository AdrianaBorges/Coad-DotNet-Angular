using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COADRESTSERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {

        private ProdutoComposicaoSRV _produtoComposicaoSRV { get; set; }

        public ProdutoController(ProdutoComposicaoSRV produtoComposicaoSRV)
        {
            _produtoComposicaoSRV = produtoComposicaoSRV;
        }

        [HttpGet("buscar-produto-composicao-por-produto-id")]
        public JSONResponse BuscarProdutoComposicaoPorProdutoId(int produtoId)
        {
            var result = new JSONResponse();
            try
            {
                var produtoComposicaoList = _produtoComposicaoSRV.BuscarProdutoComposicaoAtivoPorProdutoId(produtoId);
                result.Add("produto", produtoComposicaoList);
                result.success = true;
                return result;
            }
            catch(Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
                return result;
            }
        }

        /*[HttpGet("buscar-produto-composicao-venda-online-por-produto-id")]
        public JSONResponse BuscarProdutoComposicaoVendaOnlinePorProdutoId(int produtoId)
        {
            var result = new JSONResponse();
            try
            {
                var produtoComposicaoList = _produtoComposicaoSRV.BuscarProdutoComposicaoVendaOnlineAtivoPorProdutoId(produtoId);
                result.Add("produto", produtoComposicaoList);
                result.success = true;
                return result;
            }
            catch (Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
                return result;
            }
        }*/
    }
}