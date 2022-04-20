using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service.Mundipagg;
/*
using  GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
*/
using COADRESTSERVICE.Auth;
using COADRESTSERVICE.Auth.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COADRESTSERVICE.Controllers.Mundipagg
{

    [Route("api/[controller]")]
    [ApiController]
    public class MundipaggCarrinhoController : ControllerBase
    {

        private UserContext UserContext { get; set; }
        private CarrinhoSRV _carrinhoSRV { get; set; }

        public MundipaggCarrinhoController(
            CarrinhoSRV carrinhoSRV,
            UserContext UserContext
            )
        {
            this._carrinhoSRV = carrinhoSRV;
            this.UserContext = UserContext;
        }
        /*
        [EcommerceBasicAuth]
        [HttpPost("adicionar-produto-carrinho")]
        public JSONResponse AdicionarProdutoCarrinho(GatewayApiClient.DataContracts.ShoppingCart carrinho, string codigoProdutoNaLoja, string nomeProduto, string descricaoProduto, int quantidade, int preco, int desconto, int total)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = UserContext.Usuario;
                var carrinhoRetorno = _carrinhoSRV.AdicionarProduto(carrinho, codigoProdutoNaLoja, nomeProduto, descricaoProduto, quantidade, preco, desconto, total);
                response.Add("carrinho", carrinhoRetorno);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return response;
        }
        */
    }
}
