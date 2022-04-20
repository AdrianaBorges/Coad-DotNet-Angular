

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Util;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("CRC_ID")]
	public class CarrinhoComprasSRV : GenericService<CARRINHO_COMPRAS, CarrinhoComprasDTO, Int32>
	{

        public CarrinhoComprasDAO _dao { get; set; }
        public CarrinhoComprasItemSRV _carrinhoComprasItemSRV { get; set; }
        public ParametrosSRV _parametros { get; set; }

        public CarrinhoComprasSRV(CarrinhoComprasDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

        public CarrinhoComprasDTO RetornarCarrinhoDoCliente(int? cliId)
        {
            var carrinho = _dao.RetornarCarrinhoDoCliente(cliId);
            return carrinho;
        }

        public CarrinhoComprasDTO RetornarCriarCarrinho(int? cliId)
        {
            var carrinho = RetornarCarrinhoDoClienteComItens(cliId);

            if(carrinho == null)
            {
                carrinho = new CarrinhoComprasDTO()
                {
                    CLI_ID = cliId,
                    DATA_CRIACAO = DateTime.Now,
                    CRC_VALOR_BRUTO = 0.00m,
                    CRC_VALOR_DESCONTO = 0.00m,
                    CRC_VALOR_FRETE = 0.00m,
                    CRC_VALOR_LIQUIDO = 0.00m
                };
                return carrinho;

            }
            return carrinho;
        }

        public CarrinhoComprasDTO RetornarCarrinhoDoClienteComItens(int? cliId)
        {
            var carrinho = _dao.RetornarCarrinhoDoCliente(cliId);
            _carrinhoComprasItemSRV.PreencherItensNoCarrinho(carrinho);
            return carrinho;
        }

        public CarrinhoComprasDTO AdicionarProdutoCarrinho(CarrinhoComprasItemDTO itens, UsuarioAutenticadoDTO usuario)
        {
            if(usuario != null)
            {
                return AdicionarItens(itens, usuario.CliId);
            }
            return null;
        }

        public CarrinhoComprasDTO AdicionarItens(CarrinhoComprasItemDTO itens, int? cliId)
        {
            CarrinhoComprasDTO carrinho = null;
            using(var scope = new TransactionScope())
            {
                carrinho = RetornarCriarCarrinho(cliId);
                if(carrinho != null)
                {
                    carrinho.CARRINHO_COMPRAS_ITEM.Add(itens);
                    _calcularTotais(carrinho);
                    _processarSalvamentoCarrinho(carrinho);
                }

                scope.Complete();
            }

            return carrinho;
        }

        public void SalvarCarrinho(CarrinhoComprasDTO carrinhoCompras)
        {
            if (carrinhoCompras != null)
            {
                //Validar(carrinhoCompras);
                using (TransactionScope scope = new TransactionScope())
                {
                    _processarSalvamentoCarrinho(carrinhoCompras);
                    scope.Complete();
                }
            }
        }

        public void ApagarCarrinho(int? crcId)
        {
            if (crcId != null)
            {
                var carrinhoCompras = FindById(crcId);

                if (carrinhoCompras != null)
                {
                    carrinhoCompras.DATA_CANCELAMENTO = DateTime.Now;
                    SaveOrUpdate(carrinhoCompras);
                }
            }
        }

        private void Validar(IEnumerable<NotaFiscalConfigDTO> lstNotaFiscalConfig)
        {
            var porcentagemSomatorio = 0;

            foreach (var nfConfig in lstNotaFiscalConfig)
            {
                if (nfConfig.NFC_PORCENTAGEM_VALOR != null)
                {
                    porcentagemSomatorio += (int)nfConfig.NFC_PORCENTAGEM_VALOR;
                }

                if (porcentagemSomatorio > 100)
                {
                    throw new Exception($"Não é possível salvar a configuração da nota fiscal. A porcentagem das configurações excederam 100%. Porcentagem: {porcentagemSomatorio}%");
                }
            }
        }

        private void _processarSalvamentoCarrinho(CarrinhoComprasDTO carrinho)
        {
            if (carrinho == null)
            {
                throw new ValidacaoException("Carrinho não encontrado");
            }

            if (carrinho.CLI_ID == null)
            {
                throw new ValidacaoException("Cliente não encontrado");
            }


            var itens = carrinho.CARRINHO_COMPRAS_ITEM;
            carrinho.CARRINHO_COMPRAS_ITEM = null;

            var carrinhoSalvo = SaveOrUpdate(carrinho);
            carrinho.CRC_ID = carrinhoSalvo.CRC_ID;

            carrinho.CARRINHO_COMPRAS_ITEM = itens;
            _carrinhoComprasItemSRV.SalvarEExcluirCarrinhoItens(carrinho);
        }

        public CarrinhoComprasDTO FindByIdFullLoaded(int? crcId, bool trazItens = false)
        {
            var carrinho = FindById(crcId);

            if(trazItens && carrinho != null)
            {
                _carrinhoComprasItemSRV.PreencherItensNoCarrinho(carrinho);
            }
            return carrinho;
        }


        private void _calcularTotais(CarrinhoComprasDTO carrinho)
        {
            if(carrinho != null && carrinho.CARRINHO_COMPRAS_ITEM != null)
            {
                carrinho.CRC_VALOR_BRUTO = 0;
                carrinho.CRC_VALOR_LIQUIDO = 0;

                if (carrinho.CRC_VALOR_FRETE == null)
                    carrinho.CRC_VALOR_FRETE = 0;

                if (carrinho.CRC_VALOR_DESCONTO == null)
                    carrinho.CRC_VALOR_DESCONTO = 0;

                foreach (var item in carrinho.CARRINHO_COMPRAS_ITEM)
                {
                    item.CCI_VALOR_TOTAL = item.CCI_QTD * item.CCI_VALOR_UNITARIO;
                    carrinho.CRC_VALOR_BRUTO += item.CCI_VALOR_TOTAL;
                }

                carrinho.CRC_VALOR_LIQUIDO = (carrinho.CRC_VALOR_BRUTO + carrinho.CRC_VALOR_FRETE) - carrinho.CRC_VALOR_DESCONTO;
            }
        }

        public EmissaoPedidoDTO PrepararCarrinhoCheckout(int? cliId)
        {
            EmissaoPedidoDTO result = null;

            if (cliId != null)
            {
                var carrinho = RetornarCarrinhoDoClienteComItens(cliId);
                var empId = _parametros.RetornarEmpresaPadraoECommerce();

                if(carrinho != null)
                {
                    result = new EmissaoPedidoDTO()
                    {
                        EMP_ID = empId,
                        REP_ID = 1,
                        TipoDePedido = TipoDePedidoEnum.VENDA_NOVA,
                        TneId = 1,
                        CarId = "ON1VN01",
                        REP_ID_EMITENTE = 1,
                        RG_ID = 19,
                        UEN_ID = 4
                    };

                    if(carrinho.CARRINHO_COMPRAS_ITEM != null)
                    {
                        foreach(var itm in carrinho.CARRINHO_COMPRAS_ITEM)
                        {
                            result.EMISSAO_PEDIDO_ITEM.Add(new EmissaoPedidoItemDTO() {

                                DataVencimento = DateUtil.AdicionaDia(DateTime.Now, 2),
                                GeraNotaFiscal = true,
                                QTD = itm.CCI_QTD,
                                RG_ID = 19,
                                TTP_ID = 1,
                                VALOR_TOTAL = itm.CCI_VALOR_TOTAL,
                                VALOR_UNITARIO = itm.CCI_VALOR_UNITARIO,
                                PRODUTO_COMPOSICAO = itm.PRODUTO_COMPOSICAO,
                            
                            });
                        }
                    }
                }
            }
            return result;
        }

    }
}
