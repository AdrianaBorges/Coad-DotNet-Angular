using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Model.Dto.Custons;
using System.Transactions;
using COAD.PROSPECTADOS.Service;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Exceptions;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Service.Utils;
using GenericCrud.Util;
using COAD.CORPORATIVO.Util;
using COAD.CORPORATIVO.Service.Custons;
using GenericCrud.Service;
using COAD.CORPORATIVO.Config;
using COAD.SEGURANCA.Service.Interfaces;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.FISCAL.Service.Integracoes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PED_CRM_ID")]
    public class PedidoCRMSRV : GenericService<PEDIDO_CRM, PedidoCRMDTO, int>
    {
        private PedidoCRMDAO _dao; 
        public AgendamentoSRV _agendamentoSRV { get; set; } 
        public ClienteSRV _clienteSRV { get; set; } 
        public CartCoadSRV _cartCoadSRV { get; set; } 
        public PrioridadeAtendimentoSRV _prioridadeAtendimento { get; set; } 
        public RepresentanteSRV _representanteSRV { get; set; } 
        public TipoPagamentoSRV _tipoPagamentoSRV { get; set; } 
        public AssinaturaSRV _assinaturaSRV { get; set; } 
        public AssinaturaEmailSRV _assinaturaEmailSRV { get; set; } 
        public ContratoSRV _contratoSRV { get; set; } 
        public ParcelasSRV _parcelasSRV { get; set; }
        public ProdutosSRV _produtosSRV { get; set; } 

        [ServiceProperty("PED_CRM_ID", Name = "itemPedido", PropertyName = "ITEM_PEDIDO")]
        public ItemPedidoSRV _itemPedidoSRV { get; set; } 

        public InfoFaturaSRV _infoFatura { get; set; } 
        public HistoricoNotificacaoSRV _historicoSRV { get; set; } // = new PedidoCRMDAO();= new HistoricoNotificacaoSRV();
        public IEmailSRV _emailSRV { get; set; }
        public EmpresaSRV EmpresaSRV { get; set; }



        public PedidoCRMSRV()
        {
            _dao = new PedidoCRMDAO();
            _agendamentoSRV = new AgendamentoSRV();
            _clienteSRV = new ClienteSRV();
            _cartCoadSRV = new CartCoadSRV();
            _prioridadeAtendimento = new PrioridadeAtendimentoSRV();
            _representanteSRV = new RepresentanteSRV();
            _tipoPagamentoSRV = new TipoPagamentoSRV();
            _assinaturaSRV = new AssinaturaSRV();
            _assinaturaEmailSRV = new AssinaturaEmailSRV(); 
            _contratoSRV = new ContratoSRV();
            _parcelasSRV = new ParcelasSRV();
            _produtosSRV = new ProdutosSRV();

            _itemPedidoSRV = new ItemPedidoSRV();
            _infoFatura = new InfoFaturaSRV();
            _historicoSRV = new HistoricoNotificacaoSRV();
            this._emailSRV = new SEGURANCA.Service.EmailSRV();
            
            this.Dao = _dao;

            if(this._itemPedidoSRV != null)
                this._itemPedidoSRV.PedidoCRMSRV = this;
        }

        public PedidoCRMSRV(PedidoCRMDAO dao)
        {
            this._dao = dao;
            this.Dao = dao;

            if (this._itemPedidoSRV != null)
                this._itemPedidoSRV.PedidoCRMSRV = this;
        }
               

        public string RetornarURLPagamento(int _ipe_id)
        {

            try
            {
                var _path = SysUtils.RetornarCoadPagURL();
                var _ipevalidahash = SessionContext.HashMD5(_ipe_id.ToString());

                var _urlpagamento = _path + _ipe_id.ToString() + "/" + _ipevalidahash;

                if (SysUtils.InHomologation())
                    _urlpagamento = "http://corp.coad.com.br:9999/Checkout/Pagamento/" + _ipe_id.ToString() + "/" + _ipevalidahash;
                else
                    _urlpagamento = "http://corp.coad.com.br/Checkout/Pagamento/" + _ipe_id.ToString() + "/" + _ipevalidahash;
       

                return _urlpagamento;

            }
            catch (Exception e)
            {
                SysException.RegistrarLog("Erro ao retornar URL de pagamento (" + SysException.Show(e) + ")", "", null);

                throw new PedidoException("Erro ao retornar URL de pagamento.", e);
            }

        }

        public void LiberarParcelaGateway(int? IPE_ID, string USU_LOGIN, int? REP_ID)
        {
            ItemPedidoSRV _pedidosrv = new ItemPedidoSRV();

            var _pedido = _pedidosrv.FindById(IPE_ID);
           // var pedidoStatus = _pedido.PST_ID;

            ParcelasDTO parcela = _parcelasSRV.PrepararParcelaGateway(_pedido, null, true, null);
        }

        public string EnviarEmailConfirmaPagamento(int _ipe_id, string _email, string USU_LOGIN, int? REP_ID, bool throwException = false)
        {
            try
            {
                var _ipevalidahash = SessionContext.HashMD5(_ipe_id.ToString());

                var _pedido =  _itemPedidoSRV.FindById(_ipe_id);
                var pedidoCRM = FindById(_pedido.PED_CRM_ID);
                var _cliente = _clienteSRV.FindById(pedidoCRM.CLI_ID);
                var pedidoStatus = _pedido.PST_ID;

                ParcelasDTO parcela = _parcelasSRV.PrepararParcelaGateway(_pedido, null, true, null);

                if(USU_LOGIN != null && REP_ID != null)
                    new HistoricoPedidoSRV().RegistrarHistoricoEnvioLinkGateway(USU_LOGIN, REP_ID, _email, _ipe_id, pedidoStatus);

                _email = SysUtils.DecidirEnderecoDeEmail(_email);


                var _urlpagamento = RetornarURLPagamento(_ipe_id);
                
                string _mensagem = @"<table align=""center"" border=""1"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""border: 1px solid #cccccc;"">
                                <tr colspan=""2"">
                                    <td colspan=""2"" align=""center"" style=""padding: 40px 0 30px 0; background-color: #345e82;"">
                                        <img src=""http://www.coad.com.br/images/logo-coad-informacoes-confiaveis.png"" style=""display: block;"" />
                                    </td>
                                </tr>
                                <tr >
                                    <td colspan=""2"" valign=""top"" align=""center"" style=""padding: 25px 0 0 0;"">
                                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""400px"">
                                            <tr>
                                                <td><H2><strong>Agora falta pouco !!!</strong></H2>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style=""text-align: left;"">
                                                        Para confirmar a sua compra, confira os dados abaixo e em seguida clique no link abaixo 
                                                        ou copie a URL para acessar a interface de pagamento. Fique tranquilho, até a confirmação 
                                                        do pagamento nenhum débito sera realizado.
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style=""padding: 25px 0 0 0;"">
                                                    <ol style=""list-style-type: none;"">
                                                        <li>CONTROLE: " + _ipevalidahash + @"</li>
                                                        <li>DATA: " + DateTime.Now.ToString("dd/MM/yyyy") + @"</li>
                                                        <li>NOME: " + _cliente.CLI_NOME + @"</li>
                                                        <li>TOTAL: R$ " + String.Format("{0:F}", _pedido.IPE_TOTAL) + @"</li>
                                                    </ol>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style=""text-align: left;"">
                                                        URL para pagamento <a href=" + _urlpagamento + @">" + _urlpagamento + @"</a>
                                                    </p>
                                                    <p>
                                                        <center>Seja bem vido a COAD!</center>
                                                    </p>
                                                    <p>
                                                        <center>Time COAD</center>
                                                    </p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style=""background-color: #345e82;"">
                                    <td style=""color: #ffffff; font-family: Arial, sans-serif; font-size: 14px; padding-left:10px; padding-top:10px;"" >
                                            <a href=""http://www.coad.com.br/"">
                                                <img src=""http://www.coad.com.br/images/logo-coad-informacoes-confiaveis.png"" alt="" />
                                            </a>
                                            <p style=""text-align: justify; color: #ffffff;"">
                                               <br>
                                               <label style=""color: #ffffff;""> Estrada do Timdiba, 455 </label><br>
                                               <label style=""color: #ffffff;""> Pechincha, Rio de Janiro, RJ  </label>  <br>
                                               <label style=""color: #ffffff;""> Telefone: (21) 2156-5907 / (21) 3389-6902  </label><br>
                                               <a href=""mailto:coad@coad.com.br"" target=""_top"" style=""color: #ffffff;"" >coad@coad.com.br</a>
                                            </p>
                                    </td>
                                    <td style=""color: #ffffff; font-family: Arial, sans-serif; font-size: 14px; padding-left:10px; padding-top:10px;"" >
                                        <h4 style=""color: #ffffff;"">CONHEÇA A COAD</h4>
                                        <ul style=""list-style-type: none;"">
                                            <li><a href=""http://www.coad.com.br/"" style=""color: #ffffff; text-decoration: none;"">Portal Coad</a></li>
                                            <li><a href=""http://www.advocaciadinamica.com/"" style=""color: #ffffff; text-decoration: none;"">Advocacia Dinâmica</a></li>
                                            <li><a href=""http://www.substituicaotributaria.com/"" style=""color: #ffffff; text-decoration: none;"">Substituição Tributária</a></li>
                                            <li><a href=""http://www.coadeducacao.com.br/"" style=""color: #ffffff; text-decoration: none;"">COAD Educação</a></li>
                                            <li><a href=""https://www.coadeducacaoead.com.br/"" style=""color: #ffffff;text-decoration: none;"">COAD Educação - EAD</a></li>
                                            <li><a href=""http://www.coad.com.br/contato"" style=""color: #ffffff;text-decoration: none;"">Fale Conosco</a></li>
                                        </ul>
                                    </td>
                                </tr>
                            </table>";

                _emailSRV.EnviarEmail(new SEGURANCA.Model.EmailRequestDTO()
                {
                    EmailDestino = _email,
                    Assunto = "COAD",
                    CorpoEmail = _mensagem
                
                } //_email, "COAD", _mensagem);
                );
                SysException.RegistrarLog("Comprovante de venda enviado (" + _email + ")", "", null);
                return _urlpagamento;

            }
            catch (Exception e)
            {
                
                    SysException.RegistrarLog("Comprovante de venda não enviado (" + _email + " -- " + SysException.Show(e) + ")", "", null);
                if (throwException)
                {
                    throw new Exception("Comprovante de venda não enviado ", e);
                }
            }
            return null;
        }
        public void InformarVendaEfetuada(PedidoCRMDTO pedido, string USUARIO)
        {
            using (var scope = new TransactionScope())
            {
                if (pedido != null)
                {
                    DateTime date = DateTime.Now;
                    var CLI_ID = pedido.CLI_ID;
                    var REP_ID = pedido.REP_ID;
                    var usuario = USUARIO;

                    pedido.PED_CRM_DATA = date;
                    pedido.PED_CRM_VENDA_INFORMADA = true;
                    var cliente = _clienteSRV.FindByIdFullLoaded((int) CLI_ID, false, true, true);
                    _clienteSRV.VerificarDadosCliente(cliente, "informar venda");
                    cliente.CLA_CLI_ID = 3;

                    //if (!_cartCoadSRV.HasCartCoadByCliId(CLI_ID))
                    //{
                        var cartCoad = _clienteSRV.SalvarComoProspectado(cliente, REP_ID, null, true);

                        if (cartCoad != null)
                        {
                            var codigo = cartCoad.CODIGO;

                            int codigoInt;
                            if (int.TryParse(codigo, out codigoInt))
                            {
                                //cliente.PROSP_ID = codigoInt;
                                pedido.PROSP_ID = codigoInt;
                            }
                        }
                    //}

                    _clienteSRV.SaveOrUpdate(cliente);
                    var pedidoSalvo = SaveOrUpdate(pedido);

                    int? codPedido = pedidoSalvo.PED_CRM_ID;
                    _historicoSRV.histAtend.RegistrarHistoricoVendaEfetuada(usuario, REP_ID, CLI_ID, codPedido, pedido.PED_CRM_DESCRICAO);
                    _prioridadeAtendimento.ConfirmarAtendimentoDePrioridade((int)REP_ID, (int)CLI_ID);


                }
                scope.Complete();
            }
        }

        public Pagina<PedidoCRMDTO> ListPedidoCRM(int? REP_ID, int? CLI_ID, int? CMP_ID, int pagina = 1, int registrosPorPagina = 7)
        {
            return _dao.ListPedidoCRM(REP_ID, CLI_ID, CMP_ID);
        }
        
        public void SalvarPedido(PedidoCRMDTO pedido)
        {
            using (var scope = new TransactionScope())
            {
                _processarSalvamento(pedido);
                scope.Complete();
            }
        }

        public void GerarHistorico(PedidoCRMDTO pedido)
        {
            var repId = pedido.REP_ID;
            var cliId = pedido.CLI_ID;
            var usuario = pedido.USU_LOGIN;
            var observacao = pedido.PED_CRM_DESCRICAO;
            var online = pedido.EhOnline;

            if(pedido != null)
            {
                if (pedido.ITEM_PEDIDO != null)
                {
                    foreach (var itemPedido in pedido.ITEM_PEDIDO)
                    {
                        var cmpId = itemPedido.CMP_ID;
                        var codItemPedido = itemPedido.IPE_ID;

                        if (online)
                        {
                            if (usuario == null)
                                usuario = "COADSYS";

                            _historicoSRV.RegistrarHistoricoPedidoEmitidoOnline(usuario, cliId, codItemPedido, cmpId);
                        }
                        else
                        {
                            _historicoSRV.RegistrarHistoricoPedidoEmitido(usuario, repId, cliId, codItemPedido, cmpId, observacao);
                        }
                    }
                }
            }
    
        }

        private void _processarCliente(PedidoCRMDTO pedido, bool validarCliente = true)
        {
            if (pedido.CLIENTES == null && pedido.CLI_ID == null)
            {
                throw new PedidoException("Não há informações do cliente");
            }
            
            var cliId = pedido.CLI_ID;

            if (pedido.CLIENTES == null && pedido.CLI_ID != null)
            {
                var clienteDoBanco = _clienteSRV.FindByIdFullLoaded((int)cliId, false, true, true);
                clienteDoBanco.CLA_CLI_ID = 3;
                _clienteSRV.SaveOrUpdate(clienteDoBanco);
                pedido.CLIENTES = clienteDoBanco;
            }
            else
            if (pedido.CLIENTES != null)
            {
                var clienteDoPedido = pedido.CLIENTES;
                clienteDoPedido.CLA_CLI_ID = 3;
                
                if (pedido.CLIENTES.CLI_ID != null)
                {
                    _clienteSRV.SaveOrUpdate(clienteDoPedido);
                }
                else
                {
                    var clienteSalvo = _clienteSRV.SalvarClienteAgenda(clienteDoPedido, true, true);
                    if(clienteSalvo.Cliente != null)
                        clienteDoPedido.CLI_ID = clienteSalvo.Cliente.CLI_ID;
                }

                if (pedido.CLI_ID == null)
                {
                    pedido.CLI_ID = clienteDoPedido.CLI_ID;
                }
            }

            if(validarCliente)
                _clienteSRV.VerificarDadosCliente(pedido.CLIENTES, "emitir pedido");
        }

        private void _processarSalvamento(PedidoCRMDTO pedido, bool validarCliente = true)
        {
            //var cliId = pedido.CLI_ID;
            var repId = pedido.REP_ID;
            var usuario = pedido.USU_LOGIN;
            DateTime date = DateTime.Now;
            ClienteDto cliente = null;

            pedido.PED_CRM_DATA = date;
            pedido.PED_CRM_VENDA_INFORMADA = false;
            pedido.PST_ID = 1;

            if (_representanteSRV == null)
                _representanteSRV = ServiceFactory.RetornarServico<RepresentanteSRV>();

            var representante = _representanteSRV.FindById(repId);

            _processarCliente(pedido, validarCliente);
            cliente = pedido.CLIENTES;
            
            var cliId = pedido.CLI_ID;

            var lstItemPedido = pedido.ITEM_PEDIDO;
            pedido.ITEM_PEDIDO = null;
            var pedidoSalvo = SaveOrUpdate(pedido);

            GerarCodLegado(pedidoSalvo);
            
            pedido.PED_CRM_COD_LEGADO = pedidoSalvo.PED_CRM_COD_LEGADO;
            pedido.ITEM_PEDIDO = lstItemPedido;
            pedido.PED_CRM_ID = pedidoSalvo.PED_CRM_ID;

            _itemPedidoSRV.SalvarItemPedido(pedido);

            int? codPedido = pedidoSalvo.PED_CRM_ID;

            if (repId != null)
            {
                GerarHistorico(pedido);
                _prioridadeAtendimento.ConfirmarAtendimentoDePrioridade((int)repId, (int)cliId);
            }
        }

        private void GerarCodLegado(PedidoCRMDTO pedido)
        {
            if (pedido != null && pedido.PED_CRM_ID != null)
            {
                int? idCorrente = pedido.PED_CRM_ID;
                
                string idString = MathUtil.PreencherZeroEsquerda((int)idCorrente, 5);
                idString += "F";
                pedido.PED_CRM_COD_LEGADO = idString;

                SaveOrUpdate(pedido);
            }
        }

        public Pagina<PedidoCRMDTO> ListarPedidos(
            PesquisaPedidoDTO pesquisaDTO,
            int pagina = 1,
            int registrosPorPagina = 7)
        {
            var listPedidos = _dao.ListarPedidos(
                pesquisaDTO,
                pagina, 
                registrosPorPagina);
            GetAssociations(listPedidos.lista, "itemPedido");
            PreencherEmpresa(listPedidos.lista);

            return listPedidos;
        }

        public void GerarUrlsDePagamento(IEnumerable<PedidoCRMDTO> lstPedidos)
        {
            if (lstPedidos != null)
            {
                foreach (var pedido in lstPedidos)
                {
                    GerarUrlsDePagamento(pedido);
                }
            }
        }


        public void GerarUrlsDePagamento(PedidoCRMDTO pedido)
        {
            if (pedido != null)
            {
                var lstItemPedido = pedido.ITEM_PEDIDO;

                if (lstItemPedido != null)
                {
                    foreach (var itemPedido in lstItemPedido)
                    {
                        if (itemPedido.PST_ID == 1)
                        {
                            var ipe_id = itemPedido.IPE_ID;
                            var url = RetornarURLPagamento((int) ipe_id);
                            itemPedido.UrlPagamento = url;
                        }
                    }
                }
            }
        }

        public PedidoCRMDTO FindByIdFullLoaded(int? pedCrmId, 
            bool trazDadosClienteCompleto = false, 
            bool trazPedidoPagamento = false, 
            bool trazInfoFaturaImposto = false, 
            bool trazComprovanteNoItem = false)
        {
            var pedido = FindById(pedCrmId);
            GetAssociations(pedido, "itemPedido");

            if (trazDadosClienteCompleto)
            {
                _preencherClienteComDadosCompletos(pedido);
            }

            if (trazPedidoPagamento)
            {
                if (pedido.ITEM_PEDIDO != null)
                {
                    _itemPedidoSRV.PreencherPedidoPagamentoNoItemPedido(pedido.ITEM_PEDIDO);
                }
            }

            if(trazInfoFaturaImposto){

                _infoFatura.PreencherInfoFaturaImposto(pedido.ITEM_PEDIDO);
            }

            if (trazComprovanteNoItem)
            {
                ServiceFactory.RetornarServico<PropostaItemComprovanteSRV>()
                    .PreencherPropostaItemComprovanteNoPedido(pedido.ITEM_PEDIDO);
            }

            return pedido;
        }

        private void _preencherClienteComDadosCompletos(PedidoCRMDTO pedido)
        {
            if (pedido != null && pedido.CLIENTES != null && pedido.CLIENTES.CLI_ID != null)
            {
                var clienteCompleto = _clienteSRV.FindByIdFullLoaded((int)pedido.CLIENTES.CLI_ID, false, true, true, true);
                pedido.CLIENTES = clienteCompleto;

            }
        }
        
        
        private void AlterarStatusPedido(int? pedCrmId, int? pstId)
        {
            var pedidoCRM = FindById(pedCrmId);

            if (pedidoCRM != null)
            {
                pedidoCRM.PST_ID = pstId;
            }

            Merge(pedidoCRM);
        }


        public void ConfirmarPedidoFaturado(int? PED_CRM_ID)
        {
            using (var scope = new TransactionScope())
            {
                AlterarStatusPedido(PED_CRM_ID, 3);
                scope.Complete();
            }
        }

        public PedidoCRMDTO ConverterPedidoParaPedidoCRM(EmissaoPedidoDTO emissaoDTO)
        {
            if (emissaoDTO != null)
            {
                var result = ValidatorProxy.RecursiveValidate<EmissaoPedidoDTO>(emissaoDTO);

                if (result.IsValid)
                {
                    //if (emissaoDTO.CLIENTE != null && emissaoDTO.CLIENTE.CLI_ID != null)
                    if (emissaoDTO.CLIENTE == null)
                    {
                        throw new NullReferenceException("O objeto do cliente possui uma referência nula.");
                    }
                            
                    PedidoCRMDTO pedido = new PedidoCRMDTO()
                    {
                        CLI_ID = emissaoDTO.CLIENTE.CLI_ID,
                        CLIENTES = emissaoDTO.CLIENTE,
                        PED_CRM_DATA = DateTime.Now,
                        RG_ID = emissaoDTO.RG_ID,
                        UEN_ID = emissaoDTO.UEN_ID,
                        REP_ID = emissaoDTO.REP_ID,
                        REP_ID_EMITENTE = emissaoDTO.REP_ID_EMITENTE,
                        PRT_ID = emissaoDTO.PRT_ID,
                        EMP_ID = emissaoDTO.EMP_ID,
                        PED_CRM_DESCRICAO = emissaoDTO.Observacoes,
                        ASN_NUM_ASSINATURA = emissaoDTO.ASN_NUM_ASSINATURA,
                        TPD_ID = (emissaoDTO.TipoDePedido != null) ? (int) emissaoDTO.TipoDePedido : 1,
                        CAR_ID = emissaoDTO.CarId,
                        PED_CRM_EMAIL_CONTATO = emissaoDTO.EmailContato,
                        PED_CRM_EMAIL_NOTA_FISCAL = emissaoDTO.EmailNotaFiscal,
                        PED_CEM_POR_CENTO_FATURADO = emissaoDTO.CemPorCentoFaturado,
                        PED_OBSERVACOES_NOTA_FISCAL = emissaoDTO.ObservacoesNotaFiscal,
                        PED_EMPRESA_DO_SIMPLES = emissaoDTO.EmpresaDoSimples,
                        TNE_ID = emissaoDTO.TneId
                    };

                    var lstItemPedido = _itemPedidoSRV.ConverterParaItemPedido(emissaoDTO.EMISSAO_PEDIDO_ITEM);
                    pedido.ITEM_PEDIDO = lstItemPedido;

                    return pedido;
                }
                else
                {
                    throw new ValidacaoException("Foram encotrados problemas ao validar as informações do pedido.", result);
                }
            }

            throw new NullReferenceException("Não é possível gerar o pedido. Objeto do pedido possui refencia nulla.");
        }

        /// <summary>
        /// Emiti um pedido baseado no número da assinatura, valor e quantidade e envia um email para o cliente
        /// </summary>
        /// <param name="numeroAssinatura"></param>
        /// <param name="valorPedido"></param>
        /// <param name="qtdParcelas"></param>
        /// <returns>Lista de URLS para o pagamento desse pedido</returns>
        public IList<string> EmitirPedidoRenovacaoDaMala(string numeroAssinatura, decimal valorPedido, int qtdParcelas)
        {
            IList<string> lstURLs = new List<string>();
            
            var corpConfig = ServiceConfig
                .GetConfig(ServiceConfigNomes.COOPORATIVO)
                .LigarOtimizacaoOparacoesDeUpdateInsert();

            var corpLegado = ServiceConfig
                .GetConfig(ServiceConfigNomes.COORPORATIVO_LEGADO)
                .LigarOtimizacaoOparacoesDeUpdateInsert();
            try
            {
                if (string.IsNullOrWhiteSpace(numeroAssinatura))
                    throw new ArgumentNullException("O número da assínatura não foi informado");
                
                if(valorPedido < 0)
                    throw new ArgumentNullException("O valor do pedido não pode ser menor que 0");

                if(qtdParcelas <= 0)
                    throw new ArgumentNullException("A quantidade de parcela não foi informada ou é menor que 1. Informe uma parcela de no mínimo 1.");
                
                TransactionOptions txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    var qtd = 1;
                    var assinatura = _assinaturaSRV.FindByIdFullLoaded(numeroAssinatura);

                    if (assinatura == null)
                    {
                        throw new PedidoException("A Assinatura não foi encontrada.");
                    }

                    var cliente = assinatura.CLIENTES;
                    var produto = _produtosSRV.FindById(assinatura.PRO_ID);
                    var email = _assinaturaEmailSRV.RetornarEmailDeContato(cliente.CLI_ID);

                    //var produtoComposto = new ProdutoComposicaoSRV().ObterProdutoPorNome(produto.PRO_NOME);

                    var produtoComposto = new ProdutoComposicaoSRV().ObterProdutoRenovacao(produto.PRO_ID);
                    var tipoPagamento = _tipoPagamentoSRV.FindByIdFullLoaded(7);

                    var pedido = new EmissaoPedidoDTO()
                    {
                        CLIENTE = cliente,
                        RG_ID = 11,
                        UEN_ID = 2,
                        REP_ID = 2601
                    };

                    ICollection<EmissaoPedidoItemDTO> lstEmissaoPedidoItem = new List<EmissaoPedidoItemDTO>();

                    decimal total = (valorPedido * qtd);
                    decimal? precoParcela = MathUtil.TruncarCasasDecimais((total / qtdParcelas), 2);

                    var emisaoPedidoItem = new EmissaoPedidoItemDTO()
                    {
                        QTD = qtd,
                        QTD_PARCELAS = qtdParcelas,
                        VALOR_PARCELAS = (decimal)precoParcela,
                        VALOR_TOTAL = total,
                        VALOR_UNITARIO = valorPedido,
                        PRODUTO_COMPOSICAO = produtoComposto,
                        TIPO_PAGAMENTO = tipoPagamento,
                        RG_ID = 12,
                        TTP_ID = 6
                    };

                    lstEmissaoPedidoItem.Add(emisaoPedidoItem);
                    pedido.EMISSAO_PEDIDO_ITEM = lstEmissaoPedidoItem;

                    var pedidoSalvo = EmitirPedidoOnline(pedido);

                    if (pedidoSalvo != null && pedidoSalvo.ITEM_PEDIDO != null)
                    {
                        foreach (var itmPedido in pedidoSalvo.ITEM_PEDIDO)
                        {
                            if (email != null)
                            {
                                var url = EnviarEmailConfirmaPagamento((int)itmPedido.IPE_ID, email.AEM_EMAIL, null, null, true);
                                //var url = RetornarURLPagamento((int) itmPedido.IPE_ID);
                                lstURLs.Add(url);
                            }
                        }
                    }

                    scope.Complete();              
                }
                return lstURLs;
            }
            catch (Exception e)
            {
                throw new PedidoException("Ocorreu um erro ao processar sua compra.", e);
            }
            
        }

        /// <summary>
        /// Emite pedidos on-line. Pode ser usado como método de integração com outros sistemas.
        /// </summary>
        /// <param name="emissaoPedido"></param>
        public PedidoCRMDTO EmitirPedidoOnline(EmissaoPedidoDTO emissaoPedido)
        {
            try
            {
                var pedido = ConverterPedidoParaPedidoCRM(emissaoPedido);
                pedido.EhOnline = true;

                _processarSalvamento(pedido, emissaoPedido.ValidarCliente);
                var pedidoSalvo = FindByIdFullLoaded(pedido.PED_CRM_ID, true, true, true);

                return pedidoSalvo;
            }
            catch (Exception e)
            {
                throw new PedidoException("Não é possível emitir o pedido", e);
            }
            
        }
        public PedidoCRMDTO GerarPedido(DadosPedidoIntegracaoDTO _comprador)
        {
            var pedidoSalvo = new PedidoCRMDTO();

            try
            {
                TransactionOptions txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {

                    var _cepdto = new CepLogradouroSRV().BuscarCep(_comprador.CEP);

                    if (_cepdto == null)
                        throw new Exception("CEP não encontrado !!");

                    ClienteSRV _clisrv = new ClienteSRV();

                    ClienteDto _cliente = null;

                    if (_comprador.cli_id != null)
                        _cliente = new ClienteSRV().FindById(_comprador.cli_id);

                    if (_cliente == null)
                    {

                        if (_clisrv.VerificarCPFCNPJ(_comprador.numeroDocumento) != null)
                            throw new Exception("CPF/CNPJ já cadastrado.");

                        _cliente = new ClienteDto();
                        _cliente.CLI_NOME = _comprador.nome;
                        _cliente.CLI_LOGIN = _comprador.email;
                        _cliente.CLI_SENHA = _comprador.senha;
                        _cliente.CLI_TP_PESSOA = (_comprador.tipoDocumento == 0) ? "F" : "J";
                        _cliente.CLI_EMAIL = _comprador.email;
                        _cliente.CLI_CPF_CNPJ = _comprador.numeroDocumento;
                        _cliente.CLI_COD_PAIS = "1058";
                        _cliente.TIPO_CLI_ID = (_comprador.tipoDocumento == 0) ? 2 : 3;
                        //-----------
                        var _endereco = new ClienteEnderecoDto();
                        _endereco.END_CEP = _comprador.CEP;
                        _endereco.END_TIPO = 2;
                        _endereco.END_BAIRRO = _comprador.bairro;
                        _endereco.END_COMPLEMENTO = _comprador.complemento;
                        _endereco.END_LOGRADOURO = _comprador.endereco;
                        _endereco.END_MUNICIPIO = _comprador.cidade;
                        _endereco.END_NUMERO = _comprador.numero;
                        _endereco.END_UF = _comprador.UF;
                        _endereco.MUN_ID = _cepdto.MUN_ID;

                        //------------
                        _cliente.CLIENTES_ENDERECO.Add(_endereco);
                        //------------

                        var _telefone = new AssinaturaTelefoneDTO();
                        _telefone.ATE_DDD = _comprador.dddComercial;
                        _telefone.ATE_TELEFONE = _comprador.foneComercial;
                        _telefone.TIPO_TEL_ID = 4;
                        _cliente.ASSINATURA_TELEFONE.Add(_telefone);

                        if (!String.IsNullOrWhiteSpace(_comprador.dddCelular))
                        {
                            var _celular = new AssinaturaTelefoneDTO();
                            _celular.ATE_DDD = _comprador.dddCelular;
                            _celular.ATE_TELEFONE = _comprador.foneCelular;
                            _celular.TIPO_TEL_ID = 1;
                            _cliente.ASSINATURA_TELEFONE.Add(_celular);
                        }

                        //------------
                        var _email = new AssinaturaEmailDTO();
                        _email.AEM_EMAIL = _comprador.email;
                        _email.OPC_ID = 6;
                        _cliente.ASSINATURA_EMAIL.Add(_email);
                        //------------

                    }

                    var tpg_id = _comprador.formapgto == 1 ? 7 : 9;

                    // TTP_ID (_comprador.recorrente) = 1 -- Mensal Recorrente
                    // TTP_ID (_comprador.recorrente) = 6 -- Anual

                    var _tipopagamento = new RegiaoTabelaPrecoSRV().ListarResumoDeParcelamento(11, _comprador.cmp_id, tpg_id, 1, _comprador.recorrente, _comprador.numeroparcelas);

                    if (_tipopagamento == null)
                        throw new Exception("Quantidade de parcelas não permitida!!");

                    var _prodComposto = new ProdutoComposicaoSRV().FindById(_comprador.cmp_id);

                    var _regiaoTabelaPreco = _tipopagamento.REGIAO_TABELA_PRECO;
                    var _tipoPagamento = _tipopagamento.TIPO_PAGAMENTO;

                    var _pedido = new EmissaoPedidoDTO()
                    {
                        CLIENTE = _cliente,
                        RG_ID = 11,
                        UEN_ID = 2
                    };

                    ICollection<EmissaoPedidoItemDTO> lstEmissaoPedidoItem = new List<EmissaoPedidoItemDTO>();

                    var emisaoPedidoItem = new EmissaoPedidoItemDTO()
                    {
                        QTD = 1,
                        QTD_PARCELAS = _comprador.numeroparcelas, //_cartao.numeroParcelas,
                        VALOR_PARCELAS = (decimal)_tipopagamento.ValorParcela,
                        VALOR_TOTAL = (decimal)_tipopagamento.Total,
                        VALOR_UNITARIO = (decimal)_tipopagamento.PrecoUnitario,
                        PRODUTO_COMPOSICAO = _prodComposto,
                        REGIAO_TABELA_PRECO = _regiaoTabelaPreco,
                        TIPO_PAGAMENTO = _tipoPagamento,

                    };

                    lstEmissaoPedidoItem.Add(emisaoPedidoItem);

                    _pedido.EMISSAO_PEDIDO_ITEM = lstEmissaoPedidoItem;

                    _pedido.REP_ID = 2601;

                    pedidoSalvo = this.EmitirPedidoOnline(_pedido);

                    scope.Complete();

                    return pedidoSalvo;
                }

            }
            catch (Exception e)
            {
                pedidoSalvo.erro = 1;
                pedidoSalvo.mensagem = SysException.Show(e);
                return pedidoSalvo;
            }

        }

        public PedidoCRMDTO TestarEmissaoDePedido()
        {
            using (var scope = new TransactionScope())
            {
                var produtoComposto = new ProdutoComposicaoSRV().FindById(289);
                var cliente = _clienteSRV.FindByIdFullLoaded(931161);
                var tipoPagamento = _tipoPagamentoSRV.FindByIdFullLoaded(3);
                var regiaoTabelaPreco = new RegiaoTabelaPrecoSRV().FindById(22, 12);

                var pedido = new EmissaoPedidoDTO()
                {
                    CLIENTE = cliente,
                    RG_ID = 12,
                    UEN_ID = 2
                };

                ICollection<EmissaoPedidoItemDTO> lstEmissaoPedidoItem = new List<EmissaoPedidoItemDTO>();

                var emisaoPedidoItem = new EmissaoPedidoItemDTO()
                {
                    QTD = 1,
                    QTD_PARCELAS = 1,
                    VALOR_PARCELAS = 2303.33m,
                    VALOR_TOTAL = 2303.33m,
                    VALOR_UNITARIO = 2303.33m,
                    PRODUTO_COMPOSICAO = produtoComposto,
                    REGIAO_TABELA_PRECO = regiaoTabelaPreco,
                    TIPO_PAGAMENTO = tipoPagamento,

                };

                lstEmissaoPedidoItem.Add(emisaoPedidoItem);
                pedido.EMISSAO_PEDIDO_ITEM = lstEmissaoPedidoItem;

                var pedidoSalvo = EmitirPedidoOnline(pedido);
                scope.Complete();

                return pedidoSalvo;
            }
        }

        public void FaturarPedido(ContextoFaturamentoDTO faturamentoRequest)
        {
            if (faturamentoRequest != null && faturamentoRequest.PEDIDO != null)
            {
                var pedido = faturamentoRequest.PEDIDO;
                var lstItemPedido = pedido.ITEM_PEDIDO;
                var regiao = pedido.REGIAO;

              
                if (regiao == null)
                {
                    throw new FaturamentoException("Não é possível faturar. Não é possível achar a região ao qual você está logado. Seu usuário possui região cadastrada?");
                }

                if (regiao.EMP_ID == null)
                {
                    throw new FaturamentoException("Não é possível faturar. Não há empresa cadastrada em sua região. Contate seu gerente e solicite as configurações.");
                }

                _itemPedidoSRV.FaturarItemPedido(pedido.CLIENTES, lstItemPedido, faturamentoRequest);

                pedido.PST_ID = 3; 
                SaveOrUpdate(pedido);

                AssociarNotasAntecipadas(lstItemPedido);
                
                var batch = new BatchContext();
                ServiceFactory.RetornarServico<ItemPedidoSRV>().AdicionarVariasNotasAoLote(new NotaFiscalBatchDTO()
                {ListCodPedidos = new List<NotaFiscaBatchItemDTO>()
                    {
                        new NotaFiscaBatchItemDTO()
                        {
                            CodPedido = pedido.PED_CRM_ID,
                        }
                    },
                }, batch);
            }
        }

        private void AssociarNotasAntecipadas(ICollection<ItemPedidoDTO> lstPedidosItm)
        {
            var _nfSRV = ServiceFactory.RetornarServico<NotaFiscalSRV>();
            if (lstPedidosItm != null && lstPedidosItm.Count > 0)
            {
                foreach(var itm in lstPedidosItm)
                {
                    var possuiNota = _nfSRV.AssociarNotasFiscaisAntecipadas(itm.PPI_ID, itm.IPE_ID);
                }
            }
        }
        /// <summary>
        /// Fatura um Pedido
        /// </summary>
        /// <param name="pedCrmId"></param>
        /// <param name="regiao"></param>
        /// <returns></returns>
        /// <exception cref="COAD.CORPORATIVO.Exceptions.FaturamentoException"></exception>
        public ContextoFaturamentoDTO FaturarPedido(RequisicaoFaturamentoDTO requisicaoFaturamento, string pathNotaFiscal, int? REP_ID_QUE_EXECUTOU_ACAO = null, string usuLogin = null)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (requisicaoFaturamento == null)
                        throw new FaturamentoException("Não é possível faturar o pedido. O objeto de requisição não está disponível.");

                    int? pedCrmId = requisicaoFaturamento.PrtId;
                    var pedido = FindByIdFullLoaded(pedCrmId, true, true);

                    if (pedido != null)
                    {
                        if (pedido.PST_ID == 3)
                        {
                            throw new FaturamentoException("Não é possível faturar um pedido já faturado.");
                        }

                        if (pedido.PST_ID == 5)
                        {
                            throw new FaturamentoException("Não é possível faturar. Esse pedido foi cancelado.");
                        }

                        if(pedido.RG_ID == null)
                        {
                            throw new FaturamentoException("Não é possível faturar. Esse pedido não possui região.");
                        }

                        _clienteSRV.VerificarDadosCliente(pedido.CLIENTES, "emitir proposta");

                        var regiaoDoPedido = ServiceFactory
                            .RetornarServico<RegiaoSRV>()
                            .FindByIdFullLoaded(pedido.RG_ID, true, true);

                        var contextoFaturamento = new ContextoFaturamentoDTO()
                        {
                            PEDIDO = pedido,
                            REGIAO = regiaoDoPedido,
                            PathNotaFiscal = pathNotaFiscal,
                            REP_ID_QUE_EXECUTOU_ACAO = REP_ID_QUE_EXECUTOU_ACAO,
                            USU_LOGIN = usuLogin,
                            EMP_ID = pedido.EMP_ID,
                            RequisicaoFaturamento = requisicaoFaturamento
                        };

                        FaturarPedido(contextoFaturamento);

                        scope.Complete();

                        return contextoFaturamento;
                    } 
                    else
                    {
                        var msg = "Não é possível faturar. Não é possível encontrar o pedido de código {0}.";
                        msg = string.Format(msg, pedCrmId);
                        throw new FaturamentoException(msg);                
                    }
                }

                //return null;
            }
            catch (Exception e)
            {
                throw new FaturamentoException("Ocorreu um erro ao tentar faturar. Motivos:", e);
            }

        }       

        public void AlterarObservacoesItem(AlteracaoStatusDTO alteracoes)
        {
            if (alteracoes != null)
            {
                using (var scope = new TransactionScope())
                {
                    int? pedCrmId = alteracoes.PED_CRM_ID;


                    PedidoCRMDTO pedidoCrm = FindByIdFullLoaded(pedCrmId);

                    string observacaoOriginal = DataUtil.ReturnNotNull(pedidoCrm.PED_CRM_DESCRICAO, "'Não havia nenhuma observação anterior'");
                    string observacoes = alteracoes.OBSERVACOES;


                    pedidoCrm.PED_CRM_DESCRICAO = observacoes;

                    SaveOrUpdate(pedidoCrm);

                    if (pedidoCrm.ITEM_PEDIDO != null && observacoes != observacaoOriginal)
                    {
                        foreach (var itemPedido in pedidoCrm.ITEM_PEDIDO)
                        {
                            _historicoSRV.RegistrarHistoricoPedidoAlteracaoObservacoes(alteracoes.USU_LOGIN, alteracoes.REP_ID, itemPedido.IPE_ID, observacaoOriginal, observacoes);
                        }
                    }
                    scope.Complete();
                }
            }
        }

        public int? RetornarEmpIdDoPedido(PedidoCRMDTO pedidoCRM)
        {
            int? empId = null;
            if (pedidoCRM != null)
            {
                empId = pedidoCRM.EMP_ID;
                if(empId == null)
                {
                    throw new FaturamentoException("Não é possível faturar. Não foi possível encontrar a empresa escolhida.");
                }
            }
            return empId;
        }

        public PedidoCRMDTO ChecarEPreencherPedido(ItemPedidoDTO itemPedido)
        {
            if (itemPedido != null && itemPedido.PEDIDO_CRM == null)
            {
                var pedido = FindById(itemPedido.PED_CRM_ID);
                itemPedido.PEDIDO_CRM = pedido;
                return pedido;
            }
            return itemPedido.PEDIDO_CRM;
        }

        /// <summary>
        /// Retorna o Id do cliente que pertence ao Item de Pedido Informado
        /// </summary>
        /// <param name="ipeId"></param>
        /// <returns></returns>
        public int? RetornarCliIdDoPedidoPorItemPedido(int? ipeId)
        {
            return _dao.RetornarCliIdDoPedidoPorItemPedido(ipeId);
        }

        public RequisicaoFaturamentoDTO GerarRequisicaoFaturamento(int? prtId)
        {
            try
            {
                RequisicaoFaturamentoDTO requisicaoFat = new RequisicaoFaturamentoDTO();
                requisicaoFat.PrtId = prtId;

                var pedidoCRM = FindById(prtId);

                if (pedidoCRM == null)
                    throw new Exception(string.Format("Não é possível encontrar a proposta {0}", prtId));
                var lstItensPedido = _itemPedidoSRV.ListarItemPedidoDoPedido(prtId);

                if (lstItensPedido != null && lstItensPedido.Count() > 0)
                {
                    IList<RequisicaoFaturamentoDetalheDTO> lstDetalheFaturamento = new List<RequisicaoFaturamentoDetalheDTO>();
                    foreach (var itm in lstItensPedido)
                    {
                        string nomeProduto = null;

                        if (itm.PRODUTO_COMPOSICAO != null && itm.PRODUTO_COMPOSICAO != null)
                        {
                            nomeProduto = itm.PRODUTO_COMPOSICAO.CMP_DESCRICAO;
                            // ServiceFactory.RetornarServico<ProdutoComposicaoSRV>().ChecaEMarcaProdutoCurso(itm.PRODUTO_COMPOSICAO);

                        }

                        var detalhe = new RequisicaoFaturamentoDetalheDTO()
                        {
                            IpeId = itm.IPE_ID,
                            NomeProduto = nomeProduto,
                            PrecoUnitario = itm.IPE_PRECO_UNITARIO,
                            ValorPedido = itm.IPE_TOTAL,
                            DataFaturamento = itm.IPE_DATA_FATURAMENTO_SEMANA_FAT,
                            GerarNotaFiscal = (itm.IPE_GERA_NOTA != null) ? itm.IPE_GERA_NOTA.Value : true,
                            EmpId = pedidoCRM.EMP_ID
                        };

                        detalhe.BloqueiaGeracaoNota = !detalhe.GerarNotaFiscal;
                        lstDetalheFaturamento.Add(detalhe);
                    }

                    requisicaoFat.LstRequisicaoFaturamento = lstDetalheFaturamento;
                }

                return requisicaoFat;
            }
            catch(Exception e)
            {
                throw new FaturamentoException("Não é possível obter os parâmetros de faturamento", e);
            }
        }

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorStatus()
        {
            return _dao.ObterGrupoFiltroPorStatus();
        }

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorData()
        {
            return _dao.ObterGrupoFiltroPorData();
        }

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorDataFaturamento()
        {
            return _dao.ObterGrupoFiltroPorDataFaturamento();
        }

        public GruposDeFiltrosDTO ObterGruposDeFiltroDoPedido()
        {
            GruposDeFiltrosDTO grupos = new GruposDeFiltrosDTO();
            var grupoStatus = ObterGrupoFiltroPorStatus();
            var grupoData = ObterGrupoFiltroPorData();
            var grupoDataFaturamento = ObterGrupoFiltroPorDataFaturamento();

            grupos.AdicionarGrupoDeFiltros("status", grupoStatus);
            grupos.AdicionarGrupoDeFiltros("data", grupoData);
            grupos.AdicionarGrupoDeFiltros("dataFaturamento", grupoDataFaturamento);

            return grupos;
        }

        public IList<AutoCompleteDTO> ListarAssinaturaDoPedidoAutoComplete(string assinatura)
        {
            return _dao.ListarAssinaturaDoPedidoAutoComplete(assinatura);
        }


        public void CancelarPedido(CancelamentoDTO cancelamento)
        {
            CancelarOuDevolverNotasDoPedido(cancelamento.PED_CRM_ID);
            using (var scope = new TransactionScope())
            {
                _itemPedidoSRV.CancelarItensDoPedido(cancelamento.PED_CRM_ID, cancelamento);
                scope.Complete();
            }
        }


        public void CancelarOuDevolverNotasDoPedido(int? CodPedido)
        {
            if(CodPedido != null)
            {
                var _notaSRV = ServiceFactory.RetornarServico<NotaFiscalSRV>();
                var _integrNfeSRV = ServiceFactory.RetornarServico<IntegrNfeSRV>();
                var pedido = FindByIdFullLoaded(CodPedido);

                IList<NotaFiscalDTO> lstNotaFiscal = new List<NotaFiscalDTO>();

                if(pedido != null && pedido.ITEM_PEDIDO != null && pedido.ITEM_PEDIDO.Count > 0)
                {
                    foreach(var itm in pedido.ITEM_PEDIDO)
                    {
                        var lstNotas = _notaSRV.ListarNotasDeEntradaEnviada(itm.IPE_ID);
                        lstNotaFiscal = lstNotaFiscal.Concat(lstNotas).ToList();
                    }
                }

                _notaSRV.CancelarOuDevolverNota(lstNotaFiscal);
            }
        }

        public bool PropostaPossuiPedido(int? prtID)
        {
            return _dao.PropostaPossuiPedido(prtID);
        }

        public void PreencherEmpresa(IEnumerable<PedidoCRMDTO> lstPedido)
        {
            if (lstPedido != null)
            {
                foreach (var pro in lstPedido)
                {
                    if (pro.EMP_ID != null)
                    {
                        pro.EMPRESAS = EmpresaSRV.FindById(pro.EMP_ID);
                    }
                }
            }
        }
        public void PreencherEmpresa(PedidoCRMDTO pedido)
        {
            if (pedido != null && pedido.EMP_ID != null)
            {
                pedido.EMPRESAS = EmpresaSRV.FindById(pedido.EMP_ID);
            }
        }
    }
}
