using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COADRESTSERVICE.Auth;
using COADRESTSERVICE.Auth.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COADRESTSERVICE.Controllers
{
    [Route("api/carrinho-compras")]
    [ApiController]
    public class CarrinhoComprasController : ControllerBase
    {
        private UserContext UserContext { get; set; }
        private CarrinhoComprasSRV _carrinhoComprasSRV { get; set; }

        private ProdutoComposicaoSRV ProdutoComposicao { get; set; }
        public CarrinhoComprasController(
            CarrinhoComprasSRV carrinhoComprasSRV,
            UserContext UserContext, ProdutoComposicaoSRV ProdutoComposicao
            )
        {
            this._carrinhoComprasSRV = carrinhoComprasSRV;
            this.UserContext = UserContext;
            this.ProdutoComposicao = ProdutoComposicao;
        }

        // GET: api/CarrinhoCompras/listar-carrinho-cliente
        [EcommerceBasicAuth]
        [HttpGet("listar-carrinho-cliente")]
        public JSONResponse ListarCarrinhoCliente()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = UserContext.Usuario;
                var carrinho = _carrinhoComprasSRV.RetornarCarrinhoDoClienteComItens(usuario.CliId);
                response.Add("carrinho", carrinho);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return response;
        }

        [EcommerceBasicAuth]
        [HttpPost("adicionar-produto-carrinho")]
        public JSONResponse AdicionarProdutoCarrinho([FromBody] CarrinhoComprasItemDTO item)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = UserContext.Usuario;
                var carrinho = _carrinhoComprasSRV.AdicionarProdutoCarrinho(item, usuario);
                response.Add("carrinho", carrinho);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return response;
        }

        // put: api/CarrinhoCompras
        [HttpPut("atualizar")]
        public JSONResponse AtualizarCarrinho([FromBody] CarrinhoComprasDTO carrinho)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = UserContext.Usuario;
                _carrinhoComprasSRV.SalvarCarrinho(carrinho);
                response.Add("carrinho", carrinho);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return response;
        }

        // DELETE: api/carrinho-compras/5
        [HttpDelete("{id}")]
        public JSONResponse Delete(int id)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                _carrinhoComprasSRV.ApagarCarrinho(id);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return response;
        }

        [EcommerceBasicAuth]
        [HttpGet("adicionar-produto-carrinho-id")]
        public JSONResponse AdicionarProdutoCarrinhoPorIdProduto( int pro_id, int qtd )
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var produto = ProdutoComposicao.BuscarProduto(pro_id);

                CarrinhoComprasItemDTO item = new CarrinhoComprasItemDTO();

                item.CCI_QTD = qtd;
                item.CCI_VALOR_TOTAL = produto.ValorVenda * qtd;
                item.CCI_VALOR_UNITARIO = produto.ValorVenda;
                item.CMP_ID = produto.ProdutoOriginal.CMP_ID;
                //item.CRC_ID = produto.ProdutoOriginal.
                item.DATA_CRIACAO = DateTime.Now;

                response = AdicionarProdutoCarrinho(item);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return response;
        }
    }
}
