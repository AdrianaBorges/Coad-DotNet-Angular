using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Batch;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service.Boleto;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Util;
using COAD.CRYPT;
using COAD.FISCAL.Model;
using COAD.FISCAL.Model.Integracoes;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Model.Dto.Custons;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.SEGURANCA.Service.Interfaces;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Exceptions;
using GenericCrud.Metadatas;
using GenericCrud.Service;
using GenericCrud.Util;
using GenericCrud.Validations;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("IPE_ID")]
    public class ItemPedidoSRV : ServiceAdapter<ITEM_PEDIDO, ItemPedidoDTO, int>
    {
        private ItemPedidoDAO _dao;

        private ParcelasDAO _daoParcela;

        [ServiceProperty("IPE_ID", Name = "itemPedidoPedidoPagamento", PropertyName = "ITEM_PEDIDO_PEDIDO_PAGAMENTO")]
        public ItemPedidoPedidoPagamentoSRV _itemPedidoPedidoPagamento { get; set; }

        [ServiceProperty("IPE_ID", Name = "pedidoParticipante", PropertyName = "PEDIDO_PARTICIPANTE")]
        public PedidoParticipanteSRV _pedidoParticipanteSRV { get; set; }

        [ServiceProperty("IPE_ID", Name = "nfeXml", PropertyName = "NFE_XML")]
        public NFeXmlSRV _nfeXmlSRV { get; set; }

        public CryptService _cryptSRV = new CryptService();
        public IEmailSRV emailSRV { get; set; }
        public InfoFaturaSRV _infoFaturaSRV { get; set; }
        public ProdutoComposicaoSRV _produtoComposicaoSRV { get; set; }
        public AssinaturaSRV _assinaturaSRV { get; set; }
        public ContratoSRV _contratoSRV { get; set; }
        public ParcelasSRV _parcelasSRV { get; set; }
        public PedidoPagamentoSRV _pedidoPagamentoSRV { get; set; }
        public TipoPagamentoSRV _tipoPagamentoSRV { get; set; }
        public HistoricoNotificacaoSRV _historicoSRV { get; set; }
        public NotaFiscalSRV _notaFiscalSRV { get; set; }
        public ConfigAlocacaoContaSRV _configAlocacaoSRV { get; set; }
        public BoletoSRV _boletoSRV { get; set; }
        public IEmailSRV _emailSRV { get; set; }
        public ClienteSRV _clienteSRV { get; set; }
        public PropostaItemSRV _propostaItemSRV { get; set; }
        public PedidoCRMSRV PedidoCRMSRV { get; set; }
        public NotaFiscalConfigSRV _notaFiscalConfigSRV { get; set; }

        public ItemPedidoSRV(ItemPedidoDAO dao)
        {
            this._dao = dao;
            SetDao(dao);
        }

        public ItemPedidoSRV()
        {
            _dao = new ItemPedidoDAO();
            SetDao(_dao);

            _daoParcela = new ParcelasDAO();

            _itemPedidoPedidoPagamento = new ItemPedidoPedidoPagamentoSRV();
            _pedidoParticipanteSRV = new PedidoParticipanteSRV();
            _nfeXmlSRV = new NFeXmlSRV();
            _cryptSRV = new CryptService();
            _infoFaturaSRV = new InfoFaturaSRV();
            _produtoComposicaoSRV = new ProdutoComposicaoSRV();
            _assinaturaSRV = new AssinaturaSRV();
            _contratoSRV = new ContratoSRV();
            _parcelasSRV = new ParcelasSRV();
            _pedidoPagamentoSRV = new PedidoPagamentoSRV();
            _historicoSRV = new HistoricoNotificacaoSRV();
            _notaFiscalSRV = new NotaFiscalSRV();
            _configAlocacaoSRV = new ConfigAlocacaoContaSRV();
            _boletoSRV = new BoletoSRV();
            _emailSRV = new SEGURANCA.Service.EmailSRV();
            _clienteSRV = new ClienteSRV();
            _tipoPagamentoSRV = new TipoPagamentoSRV();

        }

        public PedidoCRMSRV GetPedidoService()
        {
            return ServiceFactory.RetornarServico<PedidoCRMSRV>();
        }

        public void GerarProspectParaItemPedido(PedidoCRMDTO pedido, ItemPedidoDTO itemPedido)
        {
            if (pedido != null && itemPedido != null)
            {
                var repId = pedido.REP_ID;
                var cliente = pedido.CLIENTES;

                if (cliente == null && pedido.CLI_ID != null)
                {
                    var cliId = pedido.CLI_ID;
                    cliente = _clienteSRV.FindByIdFullLoaded((int)cliId, true, true, true, false);
                }

                var cartCoad = _clienteSRV.SalvarComoProspectado(cliente, repId, null, true);

                if (cartCoad != null)
                {
                    var codigo = cartCoad.CODIGO;

                    int codigoInt;
                    if (int.TryParse(codigo, out codigoInt))
                    {
                        //cliente.PROSP_ID = codigoInt;
                        itemPedido.PROSP_ID = codigoInt;
                    }
                }
            }
        }

        public void SalvarItemPedido(PedidoCRMDTO pedido)
        {
            var lstItemPedido = pedido.ITEM_PEDIDO;
            CheckAndAssignKeyFromParentToChildsList(pedido, lstItemPedido, "PED_CRM_ID");

            if (lstItemPedido != null)
            {
                foreach (var itemPedido in lstItemPedido)
                {
                    _processarSalvamentoItemPedido(pedido, itemPedido);
                }
            }
        }

        private void _processarSalvamentoItemPedido(PedidoCRMDTO pedido, ItemPedidoDTO itemPedido)
        {

            if (itemPedido.REGIAO_TABELA_PRECO != null && (itemPedido.TP_ID == null || itemPedido.RG_ID == null))
            {
                itemPedido.TP_ID = itemPedido.REGIAO_TABELA_PRECO.TP_ID;
                itemPedido.RG_ID = itemPedido.REGIAO_TABELA_PRECO.RG_ID;
            }

            if (itemPedido.CMP_ID == null && itemPedido.PRODUTO_COMPOSICAO != null)
            {
                itemPedido.CMP_ID = itemPedido.PRODUTO_COMPOSICAO.CMP_ID;
            }

            if(itemPedido.TPG_ID == null && itemPedido.TIPO_PAGAMENTO != null)
            {
                itemPedido.TPG_ID = itemPedido.TIPO_PAGAMENTO.TPG_ID;
            }

            var regiaoTabelaPreco = itemPedido.REGIAO_TABELA_PRECO;

            if (regiaoTabelaPreco != null && regiaoTabelaPreco.TABELA_PRECO != null)
            {
                if (itemPedido.IPE_DESCONTO > regiaoTabelaPreco.TABELA_PRECO.TP_MARGEM_NEGOCIACAO)
                {
                    pedido.PST_ID = 6;
                    itemPedido.PST_ID = 6;
                }
                else
                {
                    if (pedido.PST_ID != 6)
                    {
                        pedido.PST_ID = 1;
                    }

                    itemPedido.PST_ID = 1;
                }

                var pedidoCRM =
                GetPedidoService().SaveOrUpdate(pedido);
            }
            else
            {
                itemPedido.PST_ID = 1;
            }

            var lstPagamentoPedido = itemPedido.ITEM_PEDIDO_PEDIDO_PAGAMENTO;
            itemPedido.ITEM_PEDIDO_PEDIDO_PAGAMENTO = null;

            var lstParticipantes = itemPedido.PEDIDO_PARTICIPANTE;
            itemPedido.PEDIDO_PARTICIPANTE = null;

            GerarProspectParaItemPedido(pedido, itemPedido);

            _infoFaturaSRV.SalvarInfoFatura(itemPedido);
            var itemSalvo = Save(itemPedido);
            GerarCodLegado(itemSalvo);

            itemPedido.IPE_COD_LEGADO = itemSalvo.IPE_COD_LEGADO;

            itemPedido.IPE_ID = itemSalvo.IPE_ID;
            itemPedido.ITEM_PEDIDO_PEDIDO_PAGAMENTO = lstPagamentoPedido;
            itemPedido.PEDIDO_PARTICIPANTE = lstParticipantes;

            itemPedido.PEDIDO_CRM = pedido;

            _itemPedidoPedidoPagamento.SalvarItemPedidoPagamento(itemPedido);
            _pedidoParticipanteSRV.SalvarPedidoParticipantes(itemPedido);

            if (itemPedido.IPE_CORTESIA != true)
            {
                if (itemPedido.PPI_ID != null && _pedidoPagamentoSRV.PagamentoDeEntradaEhDoTipo(itemPedido.IPE_ID, TipoPagamentoCoorporativoEnum.Boleto))
                {
                    AssociarParcelasDaPropostasAoItemPedido(itemPedido.PPI_ID, itemPedido.IPE_ID);
                }
                else
                {
                    bool podeAlocar = false;
                    if (_pedidoPagamentoSRV.PagamentoDeEntradaEhDoTipo(itemPedido.IPE_ID, TipoPagamentoCoorporativoEnum.Boleto))
                    {
                        podeAlocar = (itemPedido.PPI_ID != null);
                    }
                    _parcelasSRV.GerarVariasParcelas(itemPedido, null, podeAlocar: podeAlocar);
                }
            }
        }

        /// <summary>
        /// Pega as parcelas válidas de uma proposta e associa ela ao item de pedido da qual a proposta dá origem.
        /// </summary>
        /// <param name="ppiId"></param>
        /// <param name="ipeId"></param>
        public void AssociarParcelasDaPropostasAoItemPedido(int? ppiId, int? ipeId)
        {
            if (ppiId != null && ipeId != null)
            {
                var lstParcelasDaProposta = _parcelasSRV.ListarParcelaPorProposta(ppiId);
                var codPedidoPagamento = _pedidoPagamentoSRV.RetornarCodigoPedidoPagamentoDeEntrada(ipeId);

                foreach (var par in lstParcelasDaProposta)
                {
                    par.IPE_ID = ipeId;
                    par.PAR_PARCELA_DO_PEDIDO = true;
                    par.PGT_ID = codPedidoPagamento;
                }

                _parcelasSRV.MergeAll(lstParcelasDaProposta);
            }
        }

        private void GerarCodLegado(ItemPedidoDTO itemPedido)
        {
            if (itemPedido != null && itemPedido.IPE_ID != null)
            {
                int? idCorrente = itemPedido.IPE_ID;

                string idString = MathUtil.PreencherZeroEsquerda((int)idCorrente, 5);
                idString += "F";
                itemPedido.IPE_COD_LEGADO = idString;

                SaveOrUpdate(itemPedido);
            }
        }

        public void PreencherPedidoPagamentoNoItemPedido(IEnumerable<ItemPedidoDTO> lstItemPedido)
        {
            if (lstItemPedido != null)
            {
                GetAssociations(lstItemPedido, "itemPedidoPedidoPagamento", "pedidoParticipante");
            }
        }

        private void AlterarStatusItemPedido(int? ipeId, int? pstId, string loginQueAlterou, string loginGerenteQueAltorizouDesconto = null, string orderKey = null)
        {
            var itemPedido = FindById(ipeId);
            AlterarStatusItemPedido(itemPedido, pstId, loginQueAlterou, loginGerenteQueAltorizouDesconto, orderKey);
        }

        /// <summary>
        /// Adiciona os dados do pedido referente a transação executada no gateway de pagamento.
        /// </summary>
        /// <param name="pedidoItem"></param>
        /// <param name="requisicao"></param>
        private void AdicionarDadosTransacaoGateway(ItemPedidoDTO pedidoItem, RequisicaoPagamentoDTO requisicao, bool salvarAlteracao = false)
        {
            if (pedidoItem != null && requisicao != null)
                if (!string.IsNullOrWhiteSpace(requisicao.AuthorizationCode))
                    pedidoItem.AUTHORIZATION_CODE = requisicao.AuthorizationCode;

            if (!string.IsNullOrWhiteSpace(requisicao.OrderReference))
                pedidoItem.ORDER_KEY_REF = requisicao.OrderReference;

            if (!string.IsNullOrWhiteSpace(requisicao.OrderKey))
                pedidoItem.ORDER_KEY = requisicao.OrderKey;

            if (salvarAlteracao)
                SaveOrUpdate(pedidoItem);

        }

        /// <summary>
        /// Altera o status do pedido baseado no tipo de status informado.
        /// </summary>
        /// <param name="pedidoItem"></param>
        /// <param name="pstId"></param>
        /// <param name="loginQueAlterou"></param>
        /// <param name="loginGerenteQueAltorizouDesconto"></param>
        /// <param name="orderKey"></param>
        /// <param name="save"></param>
        [MetodoAuxiliar]
        private void AlterarStatusItemPedido(ItemPedidoDTO pedidoItem, int? pstId, string loginQueAlterou, string loginGerenteQueAltorizouDesconto = null, string orderKey = null, bool save = true, string orderKeyref = null, string AuthorizationCode = null)
        {

            pedidoItem.IPE_LOGIN_ALTEROU_STATUS = loginQueAlterou;
            pedidoItem.IPE_LOGIN_APROVOU_DESCONTO = loginGerenteQueAltorizouDesconto;

            if (!string.IsNullOrWhiteSpace(AuthorizationCode))
                pedidoItem.AUTHORIZATION_CODE = AuthorizationCode;

            if (!string.IsNullOrWhiteSpace(orderKeyref))
                pedidoItem.ORDER_KEY_REF = orderKeyref;

            if (!string.IsNullOrWhiteSpace(orderKey))
                pedidoItem.ORDER_KEY = orderKey;


            if (pedidoItem != null)
            {
                pedidoItem.PST_ID = pstId;
            }

            if (save)
            {
                Merge(pedidoItem);
            }

            if (pedidoItem.PEDIDO_CRM != null && pedidoItem.PED_CRM_ID != null)
            {
                var statusPredominante = ChecaStatusItensIguais(pedidoItem.PED_CRM_ID);
                if (statusPredominante != null) // se todos os filhos estão cancelados então altero o pai como cancelado
                {
                    pedidoItem.PEDIDO_CRM.PST_ID = statusPredominante;
                }
                else
                {
                    if (ChecaSeExisteStatusAguardandoAprovacaoDesconto(pedidoItem.PED_CRM_ID))
                    {
                        pedidoItem.PEDIDO_CRM.PST_ID = 6;
                    }
                    else
                    if (ChecaSeExisteStatusNotaFiscalRejeitada(pedidoItem.PED_CRM_ID))
                    {
                        pedidoItem.PEDIDO_CRM.PST_ID = 11;
                    }
                    else
                    if (ChecaSeExisteStatusNotaFiscalEmitida(pedidoItem.PED_CRM_ID))
                    {
                        pedidoItem.PEDIDO_CRM.PST_ID = 12;
                    }
                    else
                    {
                        pedidoItem.PEDIDO_CRM.PST_ID = 1;
                    }
                }

                GetPedidoService().Merge(pedidoItem.PEDIDO_CRM);
            }
        }

        public void CancelarItemPedido(CancelamentoItemDTO cancelamento)
        {
            int? pedCrmId = null;
            try
            {
                if (cancelamento != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        if (cancelamento.DadosCancelamentoPai.PED_CRM_ID != null)
                            pedCrmId = cancelamento.DadosCancelamentoPai.PED_CRM_ID;
                        CancelarItemPedidoSemTransacao(cancelamento);
                        scope.Complete();
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception(string.Format("Não é possível cancelar o pedido {0}.", pedCrmId), e);
            }
        }

        public void CancelarItensDoPedido(int? pedCrmId, CancelamentoDTO cancelamento)
        {
            if(pedCrmId != null && cancelamento != null)
            {
                //var lstItensPedido = ListarItemPedidoDoPedido(pedCrmId);
                var lstDadosItemPedido = cancelamento.Itens;
                foreach(var itm in lstDadosItemPedido)
                {
                    itm.DadosCancelamentoPai = cancelamento;
                    CancelarItemPedidoSemTransacao(itm);
                }
            }
        }


        public void CancelarItemPedidoSemTransacao(CancelamentoItemDTO cancelamento)
        {
            if(cancelamento != null)
            {
                var ipeId = cancelamento.ipeId;
                var motivoCancelamento = cancelamento.DadosCancelamentoPai.MOTIVO_ALTERACAO;
                var login = cancelamento.DadosCancelamentoPai.USU_LOGIN;
                var cliId = cancelamento.DadosCancelamentoPai.CLI_ID;
                var repId = cancelamento.DadosCancelamentoPai.REP_ID;
                var itemPedido = FindById(ipeId);

                if(itemPedido.PST_ID == 3)
                {
                    CancelarAssinaturaDoItemPedido(itemPedido, cancelamento.DadosCancelamentoPai);
                }
                else
                {
                    if (itemPedido.PST_ID == 3 || (!string.IsNullOrWhiteSpace(itemPedido.ASN_NUM_ASSINATURA) && itemPedido.PEDIDO_CRM.TPD_ID == 1))
                    {
                        ServiceFactory
                            .RetornarServico<AssinaturaSenhaSRV>()
                            .DeletarAssinaturaSenha(itemPedido.ASN_NUM_ASSINATURA);
                    }
                    CancelarParcelasPreFaturamentoDoPedido(itemPedido);
                }

                AlterarStatusItemPedido(ipeId, 5, login);
                
                if (itemPedido != null && itemPedido.PED_CRM_ID != null)
                {
                    var pedidoCrm = itemPedido.PED_CRM_ID;
                    _historicoSRV.RegistrarHistoricoPedidoCancelado(login, cliId, repId, pedidoCrm, ipeId, motivoCancelamento);

                    int? idRepresentanteDoPedido = null;
                    if (!ChecarPedidoEhDoRepresentante(repId, ipeId, out idRepresentanteDoPedido))
                    {
                        new NotificacoesSRV().InserirNotificacaoPedidoCancelado(repId, idRepresentanteDoPedido, cliId, pedidoCrm, ipeId, motivoCancelamento);
                    }
                }

                if(cancelamento.ExtornarNumeroNotaFiscal &&
                    cancelamento.ValidadoPraExtornarNumNota)
                {
                    ExtornarNotaFiscal(cancelamento);
                }

                if (cancelamento.DadosCancelamentoPai.EnviaEmail)
                {
                    if (string.IsNullOrWhiteSpace(cancelamento.DadosCancelamentoPai.MengagemEmailAssCanc))
                    {
                        throw new Exception("Não é possível enviar o email para o cliente. Você deve digitar uma mensagem explicando para o cliente o motivo do cancelamento.");
                    }
                    if(cancelamento.DadosCancelamentoPai.MengagemEmailAssCanc.Count() <= 7)
                    {
                        throw new Exception("Não é possível enviar o email para o cliente. O texto  mensagem deve ter no mínimo 8 caracteres.");
                    }

                    EnviarEmailCancAssinatura(cancelamento);
                }

                if(cancelamento.ValidadoPraExtornarNumNota == false &&
                    cancelamento.ExtornarNumeroNotaFiscal)
                {
                    throw new Exception("O cancelamento está marcado para extornar a nota fiscal. Porém, ela não passou na validação. Portanto, não pode ser extornada. Para prosseguir, desmarque a opção de extornar.");
                }

                if(itemPedido.PPI_ID != null)
                {
                    cancelamento.DadosCancelamentoPai.PPI_ID = itemPedido.PPI_ID;
                    _propostaItemSRV.CancelarPropostaItemSemTransacao(cancelamento.DadosCancelamentoPai);
                }
            }
        }

        private void CancelarAssinaturaDoItemPedido(ItemPedidoDTO itemPedido, AlteracaoStatusDTO alteracaoStatus = null)
        {
            if (itemPedido == null)
                throw new ArgumentNullException("Não é possível cancelar o item de pedido. Nenhum Objeto de ItemPedido foi informado no método.");
            _assinaturaSRV.CancelarAssinaturaDoItemPedido(itemPedido, alteracaoStatus);
        }

       
        public bool ChecarPedidoEhDoRepresentante(int? repId, int? ipeId, out int? repIdDoPedido)
        {
            if (repId != null && ipeId != null)
            {
                var itemPedido = FindById(ipeId);

                if (itemPedido != null && itemPedido.PEDIDO_CRM != null && itemPedido.PEDIDO_CRM.REP_ID != null)
                {
                    var repIdDoPedidoR = itemPedido.PEDIDO_CRM.REP_ID;
                    repIdDoPedido = repIdDoPedidoR;
                    return (repIdDoPedido == repId);
                }
            }
            repIdDoPedido = null;
            return false;
        }

        public void CancelarParcelasPreFaturamentoDoPedido(ItemPedidoDTO itemPedido)
        {
            if(itemPedido.PST_ID != 3)
            {
                _parcelasSRV.CancelarParcelasPreFaturamentoDoPedido(itemPedido.IPE_ID);
            }
        }

        /// <summary>
        /// Realiza baixa na próxima parcela em aberto do pedido.
        /// </summary>
        /// <param name="status"></param>
        public void ConfimarPedidoPagoManualmente(AlteracaoStatusDTO status)
        {
            if(status != null)
            {
                using (var scope = new TransactionScope())
                {
                    var ipeId = status.IPE_ID;
                    var login = status.USU_LOGIN;
                    var repId = status.REP_ID;
                    
                    PagarPedido(ipeId, repId, login);
                    scope.Complete();
                    
                    //if (itemPedido.PST_ID != 3 && itemPedido.PST_ID != 7) // Se ainda não foi faturado ou pago dou baixa na entrada
                    //{
                    //    AlterarStatusItemPedido(ipeId, 7, login);
                    //    _pedidoPagamentoSRV.MarcarEntradaComoPaga(ipeId);
                    //    ConcederAcessosDoPedido(ipeId, cliId, login);
                    //    _historicoSRV.RegistrarHistoricoPedidoAlteracaoParaPago(login, cliId, repId, ipeId);
                    //}
                    //else
                    //{
                    //    var msg = "Não é possível pagar a entrada do pedido de código {0}, ela já foi {1}.";
                    //    var acao = (itemPedido.PST_ID == 3) ? "faturado" : "pago";
                    //    msg = string.Format(msg, itemPedido.PST_ID, acao);

                    //    throw new PedidoException(msg);
                    //}
                }
            }
        }


        /// <summary>
        /// Indica que o pedido foi pago. Mais ainda precisa ter seu pagamento conferido.
        /// </summary>
        /// <param name="status"></param>
        public void MarcarItemPedidoComoPagoComPendenciaDeConferencia(AlteracaoStatusDTO status)
        {
            if (status != null)
            {
                 // abrindo a transação...
                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    var ipeId = status.IPE_ID;
                    var motivoMarcacaoPagamento = status.MOTIVO_ALTERACAO;
                    var login = status.USU_LOGIN;
                    var cliId = status.CLI_ID;
                    var repId = status.REP_ID;
                    var itemPedido = FindById(ipeId);

                    GetPedidoService().ChecarEPreencherPedido(itemPedido);

                    //_parcelasSRV.PrepararParcelasGateway(itemPedido, null, false, null);

                    AlterarStatusItemPedido(ipeId, 2, login);

                    if (itemPedido != null && itemPedido.PED_CRM_ID != null)
                    {
                        var pedidoCrm = itemPedido.PED_CRM_ID;
                        _historicoSRV.RegistrarHistoricoPedidoAlteracaoParaPagoComPendenciaDeConferencia(login, cliId, repId, ipeId, pedidoCrm, motivoMarcacaoPagamento);
                    }
                 
                    scope.Complete();
                }
            }
        }


        public bool PagarProximaParcelaAbertaDoPedido(int? ipeId, RequisicaoPagamentoDTO requisicao)
        {
            return _parcelasSRV.PagarProximaParcelaDoPedidoEmAberto(ipeId, requisicao);
        }

        public bool PagarProximaParcelaAbertaDoPedido(ItemPedidoDTO itemPedido, RequisicaoPagamentoDTO requisicao)
        {
            if (itemPedido != null)
            {
                int? ipeId = itemPedido.IPE_ID;
                return PagarProximaParcelaAbertaDoPedido(ipeId, requisicao);
            }
            return false;
        }

        //public void ConfirmarItemPedidoPago(ItemPedidoDTO itemPedido, RequisicaoDTO requisicao, string login = "gateway")
        //{       
        //    if (itemPedido != null)
        //    {
        //        var ipeId = itemPedido.IPE_ID;
        //        AlterarStatusItemPedido(ipeId, 7, login);
        //        _pedidoPagamentoSRV.MarcarEntradaComoPaga(ipeId);

        //    }
            
        //}

        public void AprovarDescontoNoPedido(AlteracaoStatusDTO status)
        {
            if (status != null)
            {
                using (var scope = new TransactionScope())
                {
                    var ipeId = status.IPE_ID;
                    var login = status.USU_LOGIN;
                    var cliId = status.CLI_ID;
                    var repId = status.REP_ID;

                    AlterarStatusItemPedido(ipeId, 1, login, login);
                    var itemPedido = FindById(ipeId);

                    if (itemPedido != null && itemPedido.PED_CRM_ID != null)
                    {
                        var pedidoCrm = itemPedido.PED_CRM_ID;

                        _historicoSRV.RegistrarHistoricoPedidoAlteracaoParaDescontoAprovado(login, cliId, repId, ipeId, pedidoCrm);

                        int? idRepresentanteDoPedido = null;
                        if (!ChecarPedidoEhDoRepresentante(repId, ipeId, out idRepresentanteDoPedido))
                        {
                            new NotificacoesSRV().InserirNotificacaoAprovacaoDesconto(repId, idRepresentanteDoPedido, cliId, pedidoCrm, ipeId);
                        }

                    }
                    scope.Complete();
                }
            }
        }

        public IList<ItemPedidoDTO> ListarItemPedidoDoPedido(int? pedCrmId)
        {
            var lstItemPedido =  _dao.ListarItemPedidoDoPedido(pedCrmId);
            VerificarEMarcarProdutoCurso(lstItemPedido);
            ProcessarLinksDePagamento(lstItemPedido);
            PreencherListaNfeXml(lstItemPedido);
            PreencherTipoPagamento(lstItemPedido);

            return lstItemPedido;
        }

        private void PreencherTipoPagamento(ICollection<ItemPedidoDTO> lstPedido)
        {
            if(lstPedido != null)
            {
                foreach(var ped in lstPedido)
                {
                    if(ped.TPG_ID != null)
                    {
                        if (ped.TIPO_PAGAMENTO == null)
                            ped.TIPO_PAGAMENTO = _tipoPagamentoSRV.FindById(ped.TPG_ID);
                        _tipoPagamentoSRV.PreencherTiposDePagamentosNoTipoPagamentoComposto(ped.TIPO_PAGAMENTO);
                    }
                }
            }
        }

        public void ProcessarLinksDePagamento(IEnumerable<ItemPedidoDTO> lstItemPedido)
        {
            if (lstItemPedido != null)
            {
                foreach (var itemPedido in lstItemPedido)
                {
                    var ipe_id = itemPedido.IPE_ID;
                    ChecaPedidoPodeUsarGateway(itemPedido);

                    if (itemPedido.PST_ID == 1)
                    {
                        var url = GetPedidoService().RetornarURLPagamento((int)ipe_id);
                        itemPedido.UrlPagamento = url;
                    }

                }
            }
        }

        //public void GerarUrlsDePagamento(IEnumerable<ItemPedidoDTO> lstItemPedido)
        //{
        //    if (lstItemPedido != null)
        //    {
        //        foreach (var itemPedido in lstItemPedido)
        //        {
        //            if (itemPedido.PST_ID == 1)
        //            {
        //                var ipe_id = itemPedido.IPE_ID;
        //                var url = GetPedidoService().RetornarURLPagamento((int)ipe_id);
        //                itemPedido.UrlPagamento = url;
        //            }
        //        }
        //    }
            
        //}

        public bool ChecaSeExisteStatus(int? pedidoCRMId, int? statusId)
        {
            return _dao.ChecaSeExisteStatus(pedidoCRMId, statusId);
        }

        public bool ChecaSeExisteStatusAguardandoAprovacaoDesconto(int? pedidoCRMId)
        {
            return ChecaSeExisteStatus(pedidoCRMId, 6);
        }

        public bool ChecaSeExisteStatusNotaFiscalEmitida(int? pedidoCRMId)
        {
            return ChecaSeExisteStatus(pedidoCRMId, 12);
        }

        public bool ChecaSeExisteStatusNotaFiscalRejeitada(int? pedidoCRMId)
        {
            return ChecaSeExisteStatus(pedidoCRMId, 11);
        }

        public int? ChecaStatusItensIguais(int? PED_CRM_ID)
        {
            return _dao.ChecaStatusItensIguais(PED_CRM_ID);
        }

       

        public void VerificarEMarcarProdutoCurso(IEnumerable<ItemPedidoDTO> lstItemPedido)
        {
            if (lstItemPedido != null)
            {
                foreach (var itemPedido in lstItemPedido)
                {
                    if (itemPedido.PRODUTO_COMPOSICAO != null)
                    {
                        _produtoComposicaoSRV.ChecaEMarcaProdutoCurso(itemPedido.PRODUTO_COMPOSICAO);
                    }
                }
            }
        }

        private void _validarConversaoItemPedido(EmissaoPedidoItemDTO emissaoItem)
        {
            if (emissaoItem.QTD == null || emissaoItem.QTD == null)
            {
                throw new NullReferenceException("O produto de venda não foi informado.");
            }

            if (emissaoItem.PRODUTO_COMPOSICAO == null)
            {
                throw new NullReferenceException("O produto de venda não foi informado.");
            }
        }

        public ICollection<ItemPedidoDTO> ConverterParaItemPedido(ICollection<EmissaoPedidoItemDTO> lstEmissaoPedidoItem)
        {
            ICollection<ItemPedidoDTO> lstPedidoItem = new List<ItemPedidoDTO>();
            
            if (lstEmissaoPedidoItem != null)
            {
                var index = 0;
                foreach (var item in lstEmissaoPedidoItem)
                {
                    
                    index++;
                    var validadeResult = ValidatorProxy.RecursiveValidate(item);
                    if (!validadeResult.IsValid)
                    {
                        throw new ValidacaoException("Erro de validação", validadeResult);
                    }

                    var itemPedido = new ItemPedidoDTO()
                    {
                        PRODUTO_COMPOSICAO = item.PRODUTO_COMPOSICAO,
                        IPE_QTD = item.QTD,
                        IPE_PARCELA = item.QTD_PARCELAS,
                        IPE_PRECO_UNITARIO = item.VALOR_UNITARIO,
                        IPE_TOTAL = item.VALOR_TOTAL,
                        IPE_VALOR_PARCELA = item.VALOR_PARCELAS,
                        REGIAO_TABELA_PRECO = item.REGIAO_TABELA_PRECO,
                        IPE_VALOR_ENTRADA = item.VALOR_ENTRADA,
                        IPE_POSSUI_ENTRADA = (item.TIPO_PAGAMENTO != null && item.TIPO_PAGAMENTO.TPG_TIPO == 1),
                        PPI_ID = item.PPI_ID,
                        DataVencimento = item.DataVencimento,
                        DataVencimentoSegParcela = item.DataVencimentoSegparcela,
                        ASN_NUM_ASSINATURA = item.ASN_NUM_ASSINATURA,
                        IPE_ACESSOS_CONCEDIDOS = item.ACESSOS_CONCEDIDOS,
                        IPE_QTD_CONSULTA = item.QuantidadeConsulta,
                        IPE_CORTESIA = item.Cortesia,
                        IPE_DATA_FATURAMENTO_SEMANA_FAT = item.DATA_PARA_FATURAMENTO,
                        IPE_PERIODO_FAT = item.PERIODO_FAT,
                        IPE_SEMANA_FAT = item.SEMANA_FAT,
                        IPE_PERIODO_MES_BONUS = item.PERIODO_MES_BONUS,
                        IPE_ASN_NUM_ASS_CANC = item.CodigoAssinaturaCanc,
                        TPG_ID = (item.TIPO_PAGAMENTO != null) ? item.TIPO_PAGAMENTO.TPG_ID : null,
                        IPE_GERA_NOTA = item.GeraNotaFiscal,
                        PEDIDO_PARTICIPANTE = item.PEDIDO_PARTICIPANTE,
                        IFF_ID = item.IFF_ID,
                        IFF_ID_ENTRADA = item.IFF_ID_ENTRADA,
                        LOC_ID = item.LOC_ID
                    };

                    if(item.REGIAO_TABELA_PRECO == null && (item.RG_ID != null || item.TTP_ID != null)){

                        itemPedido.RG_ID = item.RG_ID;
                        itemPedido.TTP_ID = item.TTP_ID;
                    }

                    var lstItemPedidoPedidoPagamento = _itemPedidoPedidoPagamento.ConverterParaPedidoPagamento(itemPedido, item.TIPO_PAGAMENTO);
                    itemPedido.ITEM_PEDIDO_PEDIDO_PAGAMENTO = lstItemPedidoPedidoPagamento;
                    lstPedidoItem.Add(itemPedido);
                }
            }

            return lstPedidoItem;
        }


        /// <summary>
        /// Verifica se o item pedido já possúi referência a um tipo de período. 
        /// Se não achar, adiciona a partir da tabela de preco do item.
        /// Retorna o tipo de Período Adicionado. Ou null caso falhe
        /// </summary>
        /// <param name="regiaoTabelaPreco"></param>
        /// <returns></returns>
        public TipoPeriodoDTO ChecaEAdicionaTipoPeriodo(ItemPedidoDTO itemPedido)
        {
           TipoPeriodoDTO tipoPeriodo = null;

           if (itemPedido != null
               && itemPedido.TTP_ID == null
               && itemPedido.REGIAO_TABELA_PRECO != null
               && itemPedido.REGIAO_TABELA_PRECO.TABELA_PRECO != null
               && itemPedido.REGIAO_TABELA_PRECO.TABELA_PRECO.TTP_ID != null)
           {
               var ttpId = itemPedido.REGIAO_TABELA_PRECO.TABELA_PRECO.TTP_ID;
               tipoPeriodo = new TipoPeriodoSRV().FindById(ttpId);

               itemPedido.TTP_ID = ttpId;
               itemPedido.TIPO_PERIODO = tipoPeriodo;

               Merge(itemPedido);
               return tipoPeriodo;
           }
           else if(itemPedido != null && itemPedido.TTP_ID != null)
           {
               if (itemPedido.TIPO_PERIODO != null)
                    return itemPedido.TIPO_PERIODO;
               else
                    return new TipoPeriodoSRV().FindById(itemPedido.TTP_ID);
           }

           return null;
            
        }

        public PedidoPagamentoDTO ChecarERetornarPedidoPagamentoPago(ItemPedidoDTO itemPedido)
        {
            var itemPedidoId = itemPedido.IPE_ID;
            var formaPagamento = _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(itemPedidoId);

            if (formaPagamento == null)
            {
                throw new NullReferenceException("Não há informações de pagamento para ser faturado.");
            }

            if (formaPagamento.PGT_PAGO != true)
            {
            }

            return formaPagamento;
        }

        public void ProcessarContextoFaturamento(ItemPedidoDTO itemPedido, ContextoFaturamentoDTO contexto)
        {
            if (contexto != null)
            {
                var ipeId = itemPedido.IPE_ID;
                var produtoComposto = _produtoComposicaoSRV.FindById(itemPedido.CMP_ID);
                var proId = ServiceFactory.RetornarServico<ProdutoComposicaoItemSRV>().ObterProIdParaGerarAssinatura(itemPedido.CMP_ID);
                var produto = ServiceFactory.RetornarServico<ProdutosSRV>().FindById(proId);
                var pedidoPagamentoEntrada = _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(ipeId);
                var pedidoPagamentoRestante = _pedidoPagamentoSRV.RetornarPagamentoTirandoAEntrada(ipeId);
                var pagamentoUnico = (pedidoPagamentoRestante == null);

                contexto.entrada = pedidoPagamentoEntrada;
                contexto.itemPedido = itemPedido;
                contexto.pagamentoRestante = pedidoPagamentoRestante;
                contexto.pagamentoSemEntrada = pagamentoUnico;
                contexto.dataFaturamento = itemPedido.IPE_DATA_FATURAMENTO;
                contexto.produto = produtoComposto;
                if (itemPedido.REGIAO_TABELA_PRECO != null && itemPedido.REGIAO_TABELA_PRECO.TABELA_PRECO != null)
                {
                    contexto.tabelaPreco = itemPedido.REGIAO_TABELA_PRECO.TABELA_PRECO;
                }

                if (itemPedido.IPE_CORTESIA == true)
                    contexto.Cortesia = true;
            }
        }
        
        public void FaturarItemPedido(ClienteDto cliente, IEnumerable<ItemPedidoDTO> lstItemPedido, ContextoFaturamentoDTO contextoFaturamento)
        {
            if (lstItemPedido != null)
            {
                foreach (var itemPed in lstItemPedido)
                {
                    if (itemPed.PST_ID == 5)
                    {
                        continue;
                    }

                    _clienteSRV.ValidacaoTotalCliente(cliente, "Faturar o pedido");
                    ProcessarContextoFaturamento(itemPed, contextoFaturamento);

                    var usuario = contextoFaturamento.USU_LOGIN;
                    var cliId = cliente.CLI_ID;
                    var enderecoCliente = ServiceFactory.RetornarServico<ClienteEnderecoSRV>().BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliente);
                    var pedido = contextoFaturamento.PEDIDO;
                    var repId = pedido.REP_ID;
                    var repIdQueExecutouAAcao = contextoFaturamento.REP_ID_QUE_EXECUTOU_ACAO;
                    var ipeId = itemPed.IPE_ID;

                    var proId = ServiceFactory.RetornarServico<ProdutoComposicaoItemSRV>().ObterProIdParaGerarAssinatura(itemPed.CMP_ID);
                    var produto = ServiceFactory.RetornarServico<ProdutosSRV>().FindById(proId);
                    
                    if(itemPed.IPE_CORTESIA != true && (pedido.TNE_ID == null || pedido.TNE_ID == 1))
                        ChecarERetornarPedidoPagamentoPago(itemPed);

                    if(!string.IsNullOrWhiteSpace(itemPed.ASN_NUM_ASSINATURA)){

                        if (itemPed.ASSINATURA == null)
                            itemPed.ASSINATURA = _assinaturaSRV.FindById(itemPed.ASN_NUM_ASSINATURA);
                        contextoFaturamento.assinatura = itemPed.ASSINATURA;
                    }
                    else {
                        contextoFaturamento.assinatura = _assinaturaSRV.GerarOuAcharAssinaturaFaturamento(cliente, proId);
                        itemPed.ASN_NUM_ASSINATURA = contextoFaturamento.assinatura.ASN_NUM_ASSINATURA;
                    }
                    InserirPeriodoDeFaturamento(itemPed, contextoFaturamento);
                    var contratos = _contratoSRV.GerarContratoFaturamento(contextoFaturamento);

                    if(itemPed.IPE_CORTESIA != true) // Só gera parcelas se não for cortesia
                    {
                        var parcelas = _parcelasSRV.GerarParcelasFaturamento(contratos, contextoFaturamento);
                    }
                    
                    _contratoSRV.SaveOrUpdateAll(contratos);
                    var pedPag = _pedidoPagamentoSRV.RetornarPagamentoTirandoAEntrada(itemPed.IPE_ID);
                    if(pedPag != null && pedPag.TPG_ID == 8 && (pedido.TNE_ID == null || pedido.TNE_ID == 1))
                    {
                        MarcarParcelasDoItemPedidoPago(itemPed, "COADCORP", false);
                    }

                    // Incluindo o registro de Faturamento

                    if(contextoFaturamento.RequisicaoFaturamento != null && 
                        contextoFaturamento.RequisicaoFaturamento.LstRequisicaoFaturamento != null)
                    {
                        var requisicaoDetalhes = contextoFaturamento
                            .RequisicaoFaturamento
                            .LstRequisicaoFaturamento
                            .Where(x => x.IpeId == ipeId).FirstOrDefault();

                        if(requisicaoDetalhes != null)
                        {
                            ServiceFactory.RetornarServico<RegistroFaturamentoSRV>().IncluirRegistroDeFaturamento(ipeId, contextoFaturamento, requisicaoDetalhes, contratos);
                        }
                    }
                    
                        
                   // Se não for cortesia gera a nota fiscal
                    //if(itemPed.IPE_CORTESIA != true)
                    //    GerarNotaFiscal(contratos, cliente, contextoFaturamento.itemPedido, enderecoCliente, produto, contextoFaturamento.PathNotaFiscal);
                  
                    itemPed.PST_ID = 3;
                    itemPed.CONTRATOS = contratos.ToList();
                    _historicoSRV.RegistrarHistoricoPedidoAlteracaoParaFaturado(usuario, cliId, repIdQueExecutouAAcao, ipeId);
                }

                SaveOrUpdateAll(lstItemPedido);
            }
        }

        
        public IEnumerable<NfeXmlDTO> GerarNotaFiscal(IEnumerable<ContratoDTO> contratos, 
            ClienteDto cliente, 
            ItemPedidoDTO itemPed, 
            ClienteEnderecoDto enderecoCliente, 
            ProdutosDTO produto,
            string PathNotaFiscal,
            BatchContext batchContext,
            int? NFX_ID = null,
            bool batch = false)
        {
            IEnumerable<NfeXmlDTO> lstResultadoRetorno = new HashSet<NfeXmlDTO>();
            int? ipeId = itemPed.IPE_ID;
            int? codPedido = itemPed.PED_CRM_ID;
            int? cliId = cliente.CLI_ID;
            int? empId = null;
            try
            {
                var listRequisicao = new List<GeracaoNFeRequestDTO>();

                foreach (var contrato in contratos)
                {
                    var contratoProcessado = contrato.CTR_NUM_CONTRATO;
                    try
                    {
                        if (contrato.CTR_GERA_NOTA_FISCAL == true && 
                            (contrato.CTR_SERVICO == null || contrato.CTR_SERVICO == false || 
                            (contrato.CTR_VLR_PRODUTO != null && contrato.CTR_VLR_PRODUTO > 0)))
                        {
                            if(contrato.CTR_CORTESIA == 1)
                            {
                                throw new NFeException(string.Format("Não é possível gerar/regerar esta nota fiscal. O contrato {0} não está marcado como cortesia.", contrato.CTR_NUM_CONTRATO));

                            }

                            TNFeInfNFeCobr cobranca = null;

                            IList<ParcelasDTO> parcelas = _daoParcela.ListarParcelasContrato(contrato.CTR_NUM_CONTRATO);

                            if (parcelas != null)
                            {

                                cobranca = new TNFeInfNFeCobr();

                                TNFeInfNFeCobrFat fat = new TNFeInfNFeCobrFat();

                                cobranca.dup = new TNFeInfNFeCobrDup[parcelas.Count];

                                decimal valor = (decimal) contrato.CTR_VLR_CONTRATO;

                                fat.nFat = parcelas[0].CTR_NUM_CONTRATO;  //pedido.CodPedido.ToString();
                                fat.vOrig = valor.ToString();
                                fat.vDesc = "0.00";
                                fat.vLiq = valor.ToString();

                                cobranca.fat = fat;

                                for (int contador = 0; contador < parcelas.Count; contador++)
                                {

                                    ParcelasDTO parcela = parcelas[contador];

                                    TNFeInfNFeCobrDup dup = new TNFeInfNFeCobrDup();

                                    dup.nDup = String.Format("{0:000}", contador + 1); //parcela.Parcela;
                                    dup.dVenc = String.Format("{0:yyyy-MM-dd}", parcela.PAR_DATA_VENCTO);
                                    dup.vDup = parcela.PAR_VLR_PARCELA.ToString();

                                    cobranca.dup[contador] = dup;

                                }

                            }

                            int? nfxId = (NFX_ID != null) ? NFX_ID : contrato.NFX_ID; 
                            var empresa = ServiceFactory.RetornarServico<EmpresaSRV>().FindById(contrato.EMP_ID);
                            empId = empresa.EMP_ID;

                            var requisicao = new GeracaoNFeRequestDTO()
                            {
                                cliente = cliente,
                                empresa = empresa,
                                itemPedido = itemPed,
                                produtoComposicao = itemPed.PRODUTO_COMPOSICAO,
                                endereco = enderecoCliente,
                                produto = produto,
                                path = PathNotaFiscal,
                                NFX_ID = nfxId,
                                DataFaturamento = contrato.CTR_DATA_FAT,
                                IpeId = itemPed.IPE_ID,
                                cobranca =  cobranca,
                            };

                            var resultado = _notaFiscalSRV.GerarXMLDaNotaFiscal(requisicao);

                            if (resultado != null)
                            {
                                ++batchContext.TotalExito;
                                var lstResultadoSalvo = _nfeXmlSRV.SaveOrUpdateAll(resultado);

                                if(lstResultadoSalvo != null && lstResultadoSalvo.Count() > 0)
                                {
                                    var result = lstResultadoSalvo.FirstOrDefault();
                                    contrato.CTR_NUMERO_NOTA = result.NFX_NUMERO_NOTA;
                                    contrato.NFX_ID = result.NFX_ID;
                                }
                                lstResultadoRetorno = lstResultadoRetorno.Concat(lstResultadoSalvo);
                            }

                        }
                        else
                        {
                            throw new NFeException(string.Format("Não é possível gerar/regerar esta nota fiscal. O contrato {0} não está marcado para gerar nota fiscal", contrato.CTR_NUM_CONTRATO));
                        }
                    }
                    catch(Exception e){

                        if (batch)
                        {
                            string chaveErro = string.Format("Cod: Ped {0}, Cod Itm {1}, Contrato {2}", codPedido, ipeId, contratoProcessado);

                            ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarErroBatch(new RegistroErroBatchDTO()
                            {
                                batchEx = batchContext,
                                context = chaveErro,
                                e = e,
                                nomeDaExecucao = "Geração de Lote de Nota Fiscal",
                                projeto = "CORPORATIVO",
                                servico = "ItemPedidoSRV",
                                tipoJob = 3,
                                descricaoCodigoReferencia = "Código do Contrato",
                                codTipoJobStr = contratoProcessado,
                                
                            });                            
                        }
                        else
                        {
                            throw e;
                        }
                    }

                }
                _contratoSRV.SaveOrUpdateNonIdentityKeyEntity(contratos);
                
                return lstResultadoRetorno;
            }
            catch(Exception e)
            {
                string mensagem = "Não é possível gerar a nota fiscal. O pedido {0} com o cliente de código {1} e empresa de código {2}";
                mensagem = string.Format(mensagem, ipeId, cliId, empId);
                throw new FaturamentoException(mensagem, e);
            }

        }

        
        public void GerarOuAtualizarNotaFiscal(int? ipeId, string pathRaiz, int? NFX_ID = null)
        {
            BatchContext batchContext = new BatchContext();

            var itemPedido = FindById(ipeId);
            if(itemPedido == null)
            {
                throw new NFeException(string.Format("Não é possível encontrar o pedido {0}.", ipeId));
            }
            if(itemPedido.PST_ID == 3)
            {
                var proId = ServiceFactory.RetornarServico<ProdutoComposicaoItemSRV>().ObterProIdParaGerarAssinatura(itemPedido.CMP_ID);
                var produto = ServiceFactory.RetornarServico<ProdutosSRV>().FindById(proId);
                var contratos = _contratoSRV.ListarContratosDoItemPedido(ipeId);
                var cliId = ServiceFactory.RetornarServico<PedidoCRMSRV>().RetornarCliIdDoPedidoPorItemPedido(ipeId);
                var cliente = _clienteSRV.FindById(cliId);
                var enderecoCliente = ServiceFactory.RetornarServico<ClienteEnderecoSRV>().BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliId);
            
                if(contratos != null && contratos.Count() > 0)
                {
                    using(var scope = new TransactionScope())
                    {
                        var contratosQueGeraNota = contratos.Where(x => x.CTR_GERA_NOTA_FISCAL == null || x.CTR_GERA_NOTA_FISCAL == true);
                        GerarNotaFiscal(contratosQueGeraNota, cliente, itemPedido, enderecoCliente, produto, pathRaiz, batchContext, NFX_ID);
                        scope.Complete();
                    }
                }

            }
        }

        
        public IList<NfeXmlDTO> GerarOuAtualizarNotaFiscal(IList<ContratoDTO> lstContratos, string pathRaiz, BatchContext batchContext)
        {
            IList<NfeXmlDTO> resultado = new List<NfeXmlDTO>();
            if(lstContratos != null)
            {
                int? itemPedidoProcessado = null;
                int? pedidoProcessado = null;
               // string contratoProcessado = null;

                var lstCodPedidos = lstContratos
                                    .OrderBy(x => x.IPE_ID)
                                    .Select(x => x.IPE_ID)
                                    .Distinct();
                
                if(lstCodPedidos != null)
                {
                    foreach(var ipeId in lstCodPedidos)
                    {
                        itemPedidoProcessado = ipeId;
                        try
                        {
                            var itemPedido = FindById(ipeId);
                            pedidoProcessado = itemPedido.PED_CRM_ID;
                            if (itemPedido == null)
                            {
                                throw new NFeException(string.Format("Não é possível encontrar o pedido {0}.", ipeId));
                            }
                            if (itemPedido.PST_ID == 3)
                            {
                                var proId = ServiceFactory.RetornarServico<ProdutoComposicaoItemSRV>().ObterProIdParaGerarAssinatura(itemPedido.CMP_ID);
                                var produto = ServiceFactory.RetornarServico<ProdutosSRV>().FindById(proId);
                                var cliId = ServiceFactory.RetornarServico<PedidoCRMSRV>().RetornarCliIdDoPedidoPorItemPedido(ipeId);
                                var cliente = _clienteSRV.FindById(cliId);
                                var enderecoCliente = ServiceFactory.RetornarServico<ClienteEnderecoSRV>().BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliId);
                                var contratos = lstContratos.Where(x => x.IPE_ID == ipeId);

                                if (contratos != null && contratos.Count() > 0)
                                {

                                    using (var scope = new TransactionScope())
                                    {
                                     
                                        var contratosQueGeraNota = contratos.Where(x => x.CTR_GERA_NOTA_FISCAL == true || x.CTR_GERA_NOTA_FISCAL == null);
                                       
                                        var resp = GerarNotaFiscal(contratosQueGeraNota, cliente, itemPedido, enderecoCliente, produto, pathRaiz, batchContext, null, true);
                                        if(resp != null)
                                        {
                                            resultado = resultado.Concat(resp).ToList();
                                        }

                                        
                                        scope.Complete();
                                    }
                                }

                                
                            }

                        }
                        catch(Exception e)
                        {
                            string chaveErro = string.Format("Cod: Ped {0}, Cod Itm {1}.", pedidoProcessado, ipeId);
                            string mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);

                            ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarErroBatch(new RegistroErroBatchDTO()
                            {
                                batchEx = batchContext,
                                context = chaveErro,
                                e = e,
                                nomeDaExecucao = "Geração de Lote Nota Fiscal",
                                projeto = "CORPORATIVO",
                                servico = "ItemPedidoSRV",
                                tipoJob = 3
                            });

                            //var historicoExecucaoDTO = new RegistroHistoricoExecucaoDTO()
                            //{
                            //    codTipoJob = 3,
                            //    descricao = "Ocorreu um falha na execução!",
                            //    exception = e,
                            //    nomeDaExecucao = "Geração de Lote de Nota Fiscal",
                            //    nomeProjeto = "CORPORATIVO",
                            //    nomeServico = "ItemPedidoSRV",
                            //    descricaoCodigoReferencia = "Código do Contrato",
                            //    codTipoJobStr = contratoProcessado
                            //};           
                            
                            //ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(historicoExecucaoDTO);

                            //batchContext.ListErros.Add(new ErroReportItemDTO()
                            //{
                            //    Contexto = chaveErro,
                            //    Mensagem = mensagem
                            //});
                        }
                    }
                }
            }
            return resultado;
        }
    
        /// <summary>
        /// Retorna os parâmetros e dados necessárioss para realizar um pagamento pelo gateway
        /// </summary>
        /// <param name="ipeId"></param>
        /// <returns></returns>
        public RequisicaoPagamentoDTO RetornarDadosDePagamento(int? ipeId)
        {
            if (ipeId != null)
            {
                var requisicaoPagamento = _dao.RetornarDadosDePagamento(ipeId);
                var itemPedido = requisicaoPagamento.ITEM_PEDIDO;
                var pedidoPagamento = requisicaoPagamento.ENTRADA;
                var pagamentoRestante = requisicaoPagamento.PAGAMENTO_RESTANTE;
                var recorrente = requisicaoPagamento.Recorrente;

                PedidoPagamentoDTO pagamentoUsado = null;
                if (itemPedido.PST_ID == 5)
                {
                    throw new PagamentoException("Não é possível obter dados de pagamento de um item cancelado.");
                }

                var parcela = _parcelasSRV.ObterProximaParcelaDoPedidoEmAberto(ipeId);

                if (parcela == null)
                {
                    throw new PagamentoException("Não é possível obter dados de pagamento. Não há mais parcelas pendentes de pagamento.");
                }

                pagamentoUsado = PedidoPagamentoSRV.CompararEntradaComPagamentoRestante(pedidoPagamento, pagamentoRestante);               

                var tipoPeriodo = ChecaEAdicionaTipoPeriodo(itemPedido);

                if (tipoPeriodo != null)
                {
                    recorrente = tipoPeriodo.TTP_RECORRENTE;
                }

                if (itemPedido.PST_ID != 3)
                {
                    requisicaoPagamento.tipoPagamentoGateway = TipoPagamentoGateway.PEDIDO;
                    requisicaoPagamento.TPG_ID = pagamentoUsado.TPG_ID;
                    requisicaoPagamento.CodigoItemPedido = itemPedido.IPE_ID;


                    if (pagamentoUsado.TPG_ID == 9)
                    {
                        requisicaoPagamento.ValorPagamento = pagamentoUsado.PGT_VLR_TOTAL;
                        requisicaoPagamento.qtdParcelas = pagamentoUsado.PGT_QTDE_PARCELAS;
                    }
                    else
                    {
                        requisicaoPagamento.ValorPagamento = pagamentoUsado.PGT_VLR_PARCELA;
                        requisicaoPagamento.qtdParcelas = 1;
                    }

                }
                else
                {
                    if (parcela == null)
                    {
                        throw new PagamentoException("Não é possível obter a última parcela em aberto do pedido. Provavel causa: O pedido não possui mais parcelas em aberto.");
                    }

                    requisicaoPagamento.tipoPagamentoGateway = TipoPagamentoGateway.PARCELA;
                    requisicaoPagamento.ValorPagamento = parcela.PAR_VLR_PARCELA;
                    requisicaoPagamento.TPG_ID = parcela.TPG_ID;
                    requisicaoPagamento.CodigoDaParcela = parcela.PAR_NUM_PARCELA;

                }

                return requisicaoPagamento;
            }

            return null;
        }

        /// <summary>
        /// Retorna os parâmetros básicos (mais leve) para o pagamento
        /// Inclui (Item Pedido, Cliente, Venda Recorrente, 
        /// </summary>
        /// <param name="ipeId"></param>
        /// <returns></returns>
        public RequisicaoPagamentoDTO RetornarDadosBasicosDePagamento(int? ipeId, string usu_login = null, int? repIdQueExecutou = null)
        {
            if (ipeId != null)
            {
                var itemPedido = FindById(ipeId);

                if (itemPedido.PST_ID == 5)
                {
                    throw new PagamentoException("Não é possível obter dados de pagamento de um item cancelado.");
                }

                int? cliId = null;
                int? repId = null;
                if (itemPedido.PEDIDO_CRM != null)
                {
                    cliId = itemPedido.PEDIDO_CRM.CLI_ID;
                    repId = itemPedido.PEDIDO_CRM.REP_ID;
                }

                var cliente = new ClienteSRV().FindById((int) cliId);

                bool recorrente = false;

                var tipoPeriodo = ChecaEAdicionaTipoPeriodo(itemPedido);

                if (tipoPeriodo != null)
                {
                    recorrente = tipoPeriodo.TTP_RECORRENTE;
                }

                var requisicaoPagamento = new RequisicaoPagamentoDTO()
                {
                    CodigoItemPedido = ipeId,
                    CLI_ID = cliId,
                    REP_ID = repId,
                    ITEM_PEDIDO = itemPedido,
                    CLIENTE = cliente,
                    qtdParcelas = itemPedido.IPE_PARCELA,
                    Recorrente = recorrente, 
                    USU_LOGIN = usu_login,
                    REP_ID_EXECUTOU_A_ACAO = repIdQueExecutou,
                    darBaixar = true
                };          

                return requisicaoPagamento;
            }

            return null;
        }

        [MetodoTopLevelReferenciavel]
        public void PagarPedido(int? ipeId, int? repId, string usuLogin)
        {
            var dados = RetornarDadosBasicosDePagamento(ipeId, usuLogin, repId);
            PagarPedido(dados);
        }

        /// <summary>
        /// Executa todos os passos necessários para mudar o status do pedido como pago, <para></para>
        /// marca o dado de pagamento como paga, dá baixa e liquida a parcela e registra no histórico do cliente.
        /// </summary>
        /// <param name="requisicao"></param>
        [MetodoTopLevelReferenciavel]
        public void PagarPedido(RequisicaoPagamentoDTO requisicao)
        {
            if (requisicao != null)
            {       
                var ipeId = requisicao.CodigoItemPedido;
                var login = requisicao.USU_LOGIN;
                var cliId = requisicao.CLI_ID;
                var repId = requisicao.REP_ID;
                var orderKey = requisicao.OrderKey;
                var itemPedido = requisicao.ITEM_PEDIDO;
                var pagamentoEntrada = requisicao.ENTRADA;
                var pagamentoRestante = requisicao.PAGAMENTO_RESTANTE;
                var cliente = requisicao.CLIENTE;
                var lstParcelasAPagar = requisicao.lstParcelasAPagar;
                var darBaixa = requisicao.darBaixar;
                var representanteQueExecutou = (requisicao.REP_ID_EXECUTOU_A_ACAO != null) ? requisicao.REP_ID_EXECUTOU_A_ACAO : repId;
                var marcarPedidoPagamento = requisicao.marcarPedidoPagamentoPago;

                if (itemPedido.PST_ID == 3 || itemPedido.PST_ID == 7) // Se ainda não foi faturado ou pago dou baixa na entrada
                {
                    var msg = "Não é possível pagar o pedido de código {0}, ela já foi {1}.";
                    var acao = (itemPedido.PST_ID == 3) ? "faturado" : "pago";
                    msg = string.Format(msg, itemPedido.PST_ID, acao);

                    throw new PedidoException(msg);
                }

                if (marcarPedidoPagamento == null || marcarPedidoPagamento == true)
                {
                    if (itemPedido.PST_ID == 1 || itemPedido.PST_ID == 2)
                    {
                        if(marcarPedidoPagamento == null)
                            marcarPedidoPagamento = true;
                        
                        AlterarStatusItemPedido(itemPedido, 7, login, orderKey: orderKey, orderKeyref: requisicao.OrderReference, AuthorizationCode: requisicao.AuthorizationCode);
                        _assinaturaSRV.ConcederAcessosDoPedido(itemPedido, cliente, login);
                    }
                }
                else
                {
                    AdicionarDadosTransacaoGateway(itemPedido, requisicao, true);
                }
            
                if(lstParcelasAPagar == null || lstParcelasAPagar.Count() <= 0)
                {
                    var lstParcelas = _parcelasSRV.ObterProximasParcelasAPagar(ipeId);

                    if (lstParcelas != null)
                    {
                        lstParcelasAPagar = lstParcelas;
                    }
                }

                bool gateway = (!string.IsNullOrWhiteSpace(orderKey)) ? true : false;
                
                _parcelasSRV.PrepararParcelasGateway(lstParcelasAPagar, gateway);

                if(darBaixa)
                    _parcelasSRV.DarBaixaNaParcela(lstParcelasAPagar, requisicao, darBaixa, (bool) marcarPedidoPagamento);

                if(marcarPedidoPagamento == true)
                    _historicoSRV.RegistrarHistoricoPedidoAlteracaoParaPago(login, cliId, representanteQueExecutou, ipeId);                              
              
            }
        }



        /// <summary>
        /// Checa qual é o proximo pagamento do pedido e retorna qual é a forma de pagamento
        /// </summary>
        /// <param name="ipeId"></param>
        /// <returns></returns>
        public int? RetornaFormaDePagamentoParaProximoPagamentoDoPedido(ItemPedidoDTO itemPedido)
        {
            if (itemPedido != null)
            {
                var ipeId = itemPedido.IPE_ID;
                var pedidoPagamento = _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(ipeId);
                var pedidoPagamentoRestante = _pedidoPagamentoSRV.RetornarPagamentoTirandoAEntrada(ipeId);
                PedidoPagamentoDTO pagamentoValido = null;

                if (itemPedido.PST_ID == 1)
                {
                    pagamentoValido = pedidoPagamento;
                }

                if (itemPedido.PST_ID == 3)
                {
                    pagamentoValido = (pedidoPagamentoRestante != null) ? pedidoPagamentoRestante : pedidoPagamento;
                }

                var tpgId = (pedidoPagamento != null) ? pedidoPagamento.TPG_ID : null;
                return tpgId;
            }

            return null;
        }

        public void ChecaPedidoPodeUsarGateway(ItemPedidoDTO itemPedido)
        {
            if (itemPedido != null)
            {
                var tpgId = RetornaFormaDePagamentoParaProximoPagamentoDoPedido(itemPedido);
                if (tpgId == 7 || tpgId == 9)
                {
                    itemPedido.PodeUsarGateway = true;
                }
            }
        }


        public RequisicaoPagamentoDTO TestarPagamentoDoPedido(int? ipeId, int? REP_ID, string USU_LOGIN, bool pagar = false)
        {
            RequisicaoPagamentoDTO requisicao = null;
            
            using (var scope = new TransactionScope())
            {
                RequisicaoPagamentoDTO requisicaoPagamento = RetornarDadosDePagamento(ipeId);
                requisicaoPagamento.REP_ID = REP_ID;
                requisicaoPagamento.USU_LOGIN = USU_LOGIN;
                requisicao = requisicaoPagamento;
                requisicao.StatusTransacao = "Paid";
                requisicao.OrderKey = "%E$F$EREFDE$R%EFAREAFD%$efre";
                requisicao.OrderReference = "334343453D";
                requisicao.DataLiquidacao = DateUtil.AdicionaDia(DateTime.Now, 3);

                if (pagar == false)
                {   
                    requisicao.darBaixar = false;
                    requisicao.marcarPedidoPagamentoPago = false;
                }

                PagarPedido(requisicaoPagamento);

                scope.Complete();
            }

            return requisicao;
        }

        ///// <summary>
        ///// Procura a assinatura do produto no qual o pedido se refere. Se não achar, cria outra assinatura
        ///// </summary>
        ///// <param name="ipeId"></param>
        ///// <param name="cliId"></param>
        ///// <returns></returns>
        //public AssinaturaDTO GerarOuRetornarAssinaturaParaItemPedido(int? ipeId, int? cliId)
        //{
        //    if (ipeId != null && cliId != null)
        //    {
        //        var itemPedido = FindById(ipeId);
        //        var cliente = new ClienteSRV().FindById(cliId);
        //        return GerarOuRetornarAssinaturaParaItemPedido(itemPedido, cliente);
        //    }

        //    return null;
        //}

        ///// <summary>
        ///// Procura a assinatura do produto no qual o pedido se refere. Se não achar, cria outra assinatura
        ///// </summary>
        ///// <param name="itemPed"></param>
        ///// <param name="cliente"></param>
        ///// <returns></returns>
        //public AssinaturaDTO GerarOuRetornarAssinaturaParaItemPedido(ItemPedidoDTO itemPed, ClienteDto cliente)
        //{

        //    var proId = ServiceFactory
        //        .RetornarServico<ProdutoComposicaoItemSRV>()
        //        .ObterProIdParaGerarAssinatura(itemPed.CMP_ID);

        //    var assinatura = _assinaturaSRV.GerarOuAcharAssinaturaFaturamento(cliente, proId);
        //    return assinatura;
        //}

        ///// <summary>
        ///// Concede os acessos necessário para o cliente baseado nos produtos comprados
        ///// </summary>
        ///// <param name="ipeId"></param>
        ///// <param name="cliId"></param>
        //public void ConcederAcessosDoPedido(int? ipeId, int? cliId, string usuLogin)
        //{
        //    if (ipeId != null && cliId != null)
        //    {
        //        var itemPedido = FindById(ipeId);
        //        var cliente = ServiceFactory
        //        .RetornarServico<ClienteSRV>().FindById(cliId);

        //        ConcederAcessosDoPedido(itemPedido, cliente, usuLogin);
        //    }
        //}

        ///// <summary>
        ///// Concede os acessos necessário para o cliente baseado nos produtos comprados
        ///// </summary>
        ///// <param name="itemPed"></param>
        ///// <param name="cliente"></param>
        //public void ConcederAcessosDoPedido(ItemPedidoDTO itemPed, ClienteDto cliente, string usuLogin)
        //{
        //    ProdutosSRV proSRV = new ProdutosSRV();
        //    var assinatura = GerarOuRetornarAssinaturaParaItemPedido(itemPed, cliente);

        //    if (!proSRV.ChecaProdutoEhCurso(assinatura.PRO_ID))
        //    {
        //        AdicionarConsultasAPartirDoProduto(assinatura, itemPed, usuLogin);
        //    }
            
        //}

        //public void AdicionarConsultasAPartirDoProduto(AssinaturaDTO assinatura, ItemPedidoDTO itemPedido, string usuLogin)
        //{
        //    var cmpId = itemPedido.CMP_ID;
        //    var qtdConsultas = _produtoComposicaoSRV.RetornaQuantidadeDeConsultasDoProdutoComposto(cmpId);

        //    _assinaturaSRV.AdicionarConsultaNaAssinatura(assinatura, qtdConsultas, usuLogin);

        //}

        /// <summary>
        /// Retorna os parâmetros e dados necessárioss para realizar um pagamento pelo gateway
        /// </summary>
        /// <param name="ipeId"></param>
        /// <returns></returns>
        public GeracaoNFeRequestDTO RetornarDadosDePagamento(int? ipeId, int cfop)
        {
            if (ipeId != null)
            {
                var itemPedido = FindById(ipeId);

                if (itemPedido.PST_ID == 5)
                {
                    throw new PagamentoException("Não é possível obter dados de pagamento de um item cancelado.");
                }

                if (itemPedido.PST_ID == 7)
                {
                    throw new PagamentoException("Não é possível obter dados de pagamento. O pedido teve sua entrada paga. Mas ainda não foi faturado.");
                }

                int? cliId = null;
                int? repId = null;

                if (itemPedido.PEDIDO_CRM != null)
                {
                    cliId = itemPedido.PEDIDO_CRM.CLI_ID;
                    repId = itemPedido.PEDIDO_CRM.REP_ID;
                }

                var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindByIdFullLoaded((int)cliId, true, true, true);
                var endereco = ServiceFactory.RetornarServico<ClienteEnderecoSRV>().BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliente);
                var produtoComposicao = itemPedido.PRODUTO_COMPOSICAO;
                var proId = ServiceFactory.RetornarServico<ProdutoComposicaoItemSRV>().ObterProIdParaGerarAssinatura(itemPedido.CMP_ID);
                var produto = ServiceFactory.RetornarServico<ProdutosSRV>().FindById(proId);
                
                var geracaoDaNotaFiscal = new GeracaoNFeRequestDTO()
                {
                    itemPedido = itemPedido,
                    cliente = cliente,
                    endereco = endereco,
                    produtoComposicao = produtoComposicao,
                    produto = produto,
                    cfop = cfop                    
                    
                };

                return geracaoDaNotaFiscal;
            }

            return null;
        }

        public string RetormaPathDaNotaFiscalDoItemPedido(int? ipeId)
        {
            return _dao.RetormaPathDaNotaFiscalDoItemPedido(ipeId); 
        }

        public void PreencherListaNfeXml(IEnumerable<ItemPedidoDTO> lstItemPedido)
        {
            if (lstItemPedido != null)
            {
                GetAssociations(lstItemPedido, "nfeXml");
            }
        }


        public ItemPedidoDTO BuscarPedidoPorTransacao(string _orderkey)
        {
            return _dao.BuscarPedidoPorTransacao(_orderkey);
        }

        public int? RetornaEmpIdDoItemPedido(ItemPedidoDTO itemPedido)
        {
            int? empId = null;
            if (itemPedido != null)
            {
                var regFat = ServiceFactory.RetornarServico<RegistroFaturamentoSRV>()
                    .RetornarPrimerioRegistroDeFaturamentoPorItemPedido(itemPedido.IPE_ID);

                if (regFat != null)
                {
                    empId = regFat.EMP_ID;
                }
                else
                {
                    var pedido = itemPedido.PEDIDO_CRM;
                    empId = GetPedidoService().RetornarEmpIdDoPedido(pedido);
                }

            }

            return empId;
        }

        /// <summary>
        /// Devolve os dados de pagamento atual¹ para tratamento do que exibir na tela. <para></para>
        /// ¹ - Verificar regras para elencar o dado de pagamento atual no método 
        /// 
        ///     '<see cref="PedidoPagamentoSRV.CompararEntradaComPagamentoRestante(PedidoPagamentoDTO pedidoPagamentoEntrada, PedidoPagamentoDTO pagamentoRestante)" />'
        /// 
        /// </summary>
        /// <param name="IPE_ID"></param>
        /// <returns></returns>
        [MetodoTopLevelReferenciavel]
        public PedidoPagamentoDTO ObterPedidoPagamentoAtual(int? IPE_ID)
        {
            var pedidoPagamento = _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(IPE_ID);
            var pagamentoRestante = _pedidoPagamentoSRV.RetornarPagamentoTirandoAEntrada(IPE_ID);
            PedidoPagamentoDTO pagamentoUsado = PedidoPagamentoSRV.CompararEntradaComPagamentoRestante(pedidoPagamento, pagamentoRestante);

            return pagamentoUsado;                
        }

        public void EnviarLinkPorEmail(int? IPE_ID, int? REP_ID, List<string> lstEmail, string USU_LOGIN)
        {
            var txOpt = new TransactionOptions();
            txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;

            using (TransactionScope scope = new TransactionScope())
            {
                var itemPedido = FindById(IPE_ID);
                var pedido = itemPedido.PEDIDO_CRM;
                var regiao = new RegiaoSRV().FindById(itemPedido.RG_ID);
                var statusDoPedido = itemPedido.PST_ID;

                ContaDTO conta = _configAlocacaoSRV.RetornaPorConfiguracao(regiao, null, 1);
                ParcelasDTO parcela = _parcelasSRV.PrepararParcelaGateway(itemPedido, regiao, false, conta);

                if (pedido != null)
                {
                    var cliId = pedido.CLI_ID;
                    //var email = new AssinaturaEmailSRV().RetornarEmailDeContato(pedido.CLI_ID);

                    //if (email != null)
                    //    endEmail = email.AEM_EMAIL;

                    var urlBoleto = GerarLinkBoleto(IPE_ID);
                    urlBoleto = HttpContext.Current.Server.UrlEncode(urlBoleto);
                    EnviarLinkDoBoletoPorEmail(itemPedido, cliId, urlBoleto, lstEmail);
                    
                    var count = lstEmail.Count;
                    var index = 0;

                    StringBuilder sb = new StringBuilder();
                    foreach(var email in lstEmail)
                    {
                        sb.Append(email);
                        if (index < (count - 1))
                            sb.Append(", ");
                        index++;
                    }
                    _historicoSRV.histPedido.RegistrarHistoricoEnvioLinkEmail(USU_LOGIN, REP_ID, sb.ToString(), IPE_ID, statusDoPedido);
                }

                scope.Complete();
            }
            
        }


        //public void EnviarLinksPorEmailPedidoOnline(ItemPedidoDTO itemPedido, string email)
        //{
        //    var pedido = itemPedido.PEDIDO_CRM;
        //    var regiao = new RegiaoSRV().FindById(itemPedido.RG_ID);
        //    var statusDoPedido = itemPedido.PST_ID;

        //    ContaDTO conta = _configAlocacaoSRV.RetornaPorConfiguracao(regiao, null, 1);
        //    ParcelasDTO parcela = _parcelasSRV.PrepararParcelaGateway(itemPedido, regiao, false, conta);

        //    if (pedido != null)
        //    {
        //        var cliId = pedido.CLI_ID;
                    
        //        var urlBoleto = GerarLinkBoleto(itemPedido.IPE_ID);
        //        urlBoleto = HttpContext.Current.Server.UrlEncode(urlBoleto);
        //        EnviarLinkDoBoletoPorEmail(itemPedido, cliId, urlBoleto, email);
        //    }
        //}

        public string GerarLinkBoleto(int? IPE_ID)
        {
            if (IPE_ID == null)
            {
                throw new ArgumentNullException("Informe o código do itemPedido");
            }
            
            var codPedido = StringUtil.PreencherZeroEsquerda((int) IPE_ID, 32);
            var verificador = MathUtil.CalcularDigitoVerificador32Digitos(codPedido);

            var codigoFinal = codPedido + verificador;

            string codBoleto = _cryptSRV.CriptografarTripleDES(codigoFinal);
            return codBoleto;
        }

        public ResumoPedidoDTO GerarResumoDoPedido(ItemPedidoDTO itemPedido)
        {
            GetPedidoService().ChecarEPreencherPedido(itemPedido);
            
            var pedido = itemPedido.PEDIDO_CRM;
            var codPedido = itemPedido.IPE_ID;
            var cliente = new ClienteSRV().FindById(pedido.CLI_ID);
            var nomeCliente = cliente.CLI_NOME;
            var descricaoProduto = itemPedido.PRODUTO_COMPOSICAO.CMP_DESCRICAO;

            var valorUnitario = itemPedido.IPE_PRECO_UNITARIO;
            var quantidade = itemPedido.IPE_QTD;
            var valorParcela = itemPedido.IPE_VALOR_PARCELA;
            var porcentagemDesconto = itemPedido.IPE_DESCONTO;
            var valorTotal = itemPedido.IPE_TOTAL;
            
            var infoFatura = itemPedido.INFO_FATURA;

            var totalBruto = (valorUnitario * quantidade);
            var valorDesconto =(decimal) (porcentagemDesconto / 100) * totalBruto;

            if (infoFatura != null)
            {
                var descontoDaFatura = infoFatura.IFF_TOTAL_DESCONTADO;
                valorDesconto += descontoDaFatura;
            }

            var resumoPedido = new ResumoPedidoDTO()
            {
                CodigoPedido = codPedido,
                InformacoesImposto = infoFatura,
                NomeCliente = nomeCliente,
                NomeDoProduto = descricaoProduto,
                PorcentagemDesconto = porcentagemDesconto,
                Quantidade = quantidade,
                Total = valorTotal,
                ValorDeDesconto = valorDesconto,
                ValorUnitario = valorUnitario,
                TotalBruto = totalBruto
            };

            return resumoPedido;
        }

        public void EnviarLinkDoBoletoPorEmail(ItemPedidoDTO itemPedido, int? cliId, string linkBoleto, List<string> lstEmail = null)
        {

            var resumoPedido = GerarResumoDoPedido(itemPedido);
            linkBoleto = SysUtils.RetornarHostName() + "/cliente/GerarBoleto?hashkey=" + linkBoleto;
            var nomeCliente = resumoPedido.NomeCliente;
            var nomeProduto = resumoPedido.NomeDoProduto;
            var qtd = resumoPedido.Quantidade;
            var codigoPedido = resumoPedido.CodigoPedido;
            var valorUnitario = resumoPedido.ValorUnitario;
            var valorDesconto = resumoPedido.ValorDeDesconto;
            var totalBruto = resumoPedido.TotalBruto;
            var total = resumoPedido.Total;

            var tabelaPedido = @"
            <table>
                <thead>
                    <tr>
                        <th>Nome do Produto</th>
                    </tr>
                    <tr>
                        <th>Quantidade</th>
                    </tr>
                    <tr>
                        <th>Valor Unitário</th>
                    </tr>
                </thead>
                    <tr>
                        <td>{{nomeProduto}}</td>
                    </tr>
                    <tr>
                        <td>{{qtd}}</td>
                    </tr>
                    <tr>
                        <td>{{valorUnitario}}</td>
                    </tr>
            </table>
            <div>
                <div style='float:right; max-width: 200px'>
                    <label>Total Bruto:</label><strong>{{totalBruto}}</strong>
                    <label>Desconto:</label><strong>{{desconto}}</strong>
                    <label>Total Liguido:</label><strong>{{totalLiquido}}</strong>
                </div>
            </div>";

            
            tabelaPedido = tabelaPedido.Replace("{{nomeProduto}}", nomeProduto);
            tabelaPedido = tabelaPedido.Replace("{{qtd}}", "" + qtd);
            tabelaPedido = tabelaPedido.Replace("{{valorUnitario}}", "" + valorUnitario);
            tabelaPedido = tabelaPedido.Replace("{{totalBruto}}", "" + totalBruto);
            tabelaPedido = tabelaPedido.Replace("{{desconto}}", "" + valorDesconto);
            tabelaPedido = tabelaPedido.Replace("{{totalLiquido}}", "" + total);

            if(lstEmail != null)
            {
                var index = 0;
                foreach(var email in new List<string>(lstEmail))
                {
                    lstEmail[index] = SysUtils.DecidirEnderecoDeEmail(email);
                    index++;
                }
            }
                if (lstEmail != null && lstEmail.Count > 0)
                {
                    var url = "https://ci4.googleusercontent.com/proxy/GgWnRPBud6_dbgT5a4AZGD1cXJaq7heSiSI6uRSLpqrbeRczzyf8rGzRft8ARSffAAjCKNryW9c1grWR6aZ4DfbBnsH6SAPgdbI5SsEUK5ISOjmLsiZKwAW0iJfwmKPQF_ufrNjh0VNiRRastLGv7F1SB7KA=s0-d-e1-ft#http://emkt.coad.com.br/emkt/dados/10268/10767/Image/Cursos_Novo/Header_Contabilidade_Geral.png";

                    var templateEmail =
                        @"<div style='padding:15px;'>
                        <fieldset style='border:none;'>
                            <legend style='font-size:16px; color: #0970a3;'><strong>Pagamento do Boleto!!!</strong></legend>
                            <form>
                                <br />
                                <div style='font-size:14px'>
                                    Prezado(a) {0},
                                    Segue abaixo o link do boleto para o pedido {1}.
                                    Verifique logo abaixo seus dados de acesso e comece agora mesmo!!                                    
                                </div>

                                <br />
                                <br />
                                {2}
                                <br /> 
                                <br />
                                <div>Link para o boleto: <a href='{3}' target='_new'>{4}</a></div>
                            </form>
                        </fieldset>                    
                    </div>";

                    templateEmail = string.Format(templateEmail, nomeCliente, codigoPedido, tabelaPedido, linkBoleto, linkBoleto);

                foreach(var email in lstEmail)
                {
                    _emailSRV.EnviarEmailParaCliente(email, "Link para geração do boleto do pedido", templateEmail, url);
                }
            }
            
        }

        public string ValidarCodigoERetornarHashBoleto(string codigo)
        {
            if (!string.IsNullOrWhiteSpace(codigo))
            {
                var codigoSemVerificador = codigo.Substring(0, codigo.Length - 1);
                var verificadorPassado = int.Parse(codigo.Substring(codigo.Length - 1, 1));
                var verificadorCalculado = MathUtil.CalcularDigitoVerificador32Digitos(codigoSemVerificador);

                if (verificadorPassado == verificadorCalculado)
                {
                    return codigoSemVerificador;                    
                }
                throw new Exception("O código informado não é consistente.");
            }
            else
            {
                throw new ArgumentNullException("Informe o código");
            }
        }

        public byte[] GerarPDFDoBoleto(string hash)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var codPedido = _cryptSRV.DescriptografarTripleDES(hash);
                    codPedido = ValidarCodigoERetornarHashBoleto(codPedido);

                    var codPedidoInt = int.Parse(codPedido);

                    var itemPedido = FindById(codPedidoInt);
                    var _pedidoSRV = GetPedidoService();
                    var pedido = _pedidoSRV.ChecarEPreencherPedido(itemPedido);
                    var cliId = pedido.CLI_ID;
                    var regiao = ServiceFactory.RetornarServico<RegiaoSRV>().FindById(pedido.RG_ID);
                    var conta = _configAlocacaoSRV.RetornaPorTipo(TipoAmbientePagamento.INTERNO); //.RetornaPorConfiguracao(regiao, null, 1);

                    var parcela = _parcelasSRV.ObterProximaParcelaDoPedidoEmAberto(codPedidoInt);

                    if (parcela == null)
                    {
                        throw new Exception("Não há parcelas pendentes para gerar o boleto.");
                    }

                    _parcelasSRV.PrepararParcelaGateway(itemPedido, regiao, false, conta, parcela);                   

                    if (conta == null)
                    {
                        throw new Exception("Dados esseciais não foram encontrados. Cod.01.");
                    }

                    ParametroDTO paramBoleto = new ParametroDTO()
                    {
                        idCliente = (int)cliId,
                        idConta = (int)conta.CTA_ID,
                        idEmpresa = (int)regiao.RG_ID,
                        idTitulo = parcela.PAR_NUM_PARCELA,
                        preAlocado = true,
                        idRemessa = "01",
                    };

                    var lstBytes = _boletoSRV.GerarVariosBoletosPDF(new List<ParametroDTO> { paramBoleto });
                    scope.Complete();


                    return lstBytes;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível gerar o boleto.", e);
            }
        }


        /// <summary>
        /// Recusa a indicação de pagamento manual.
        /// </summary>
        /// <param name="status"></param>
        public void RecusarPagamentoDoPedido(AlteracaoStatusDTO status)
        {
            if (status != null)
            {
                using (var scope = new TransactionScope())
                {
                    var ipeId = status.IPE_ID;
                    var login = status.USU_LOGIN;
                    var cliId = status.CLI_ID;
                    var repId = status.REP_ID;
                    var itemPedido = FindById(ipeId);
                    var repIdRecebimento = itemPedido.PEDIDO_CRM.REP_ID;

                    AlterarStatusItemPedido(ipeId, 1, login);
                    _historicoSRV.RegistrarHistoricoPedidoRecusaIndicacaoManualDePagamento(login, cliId, repId, repIdRecebimento, ipeId, status.OBSERVACOES);
                    
                    scope.Complete();
                }
            }
        }

        /// <summary>
        /// Realiza as ações necessárias para que o Item de Pedido, 
        /// originado de uma proposta, 
        /// tenha um estado válido de pago.
        /// 
        /// </summary>
        public void MudarItemPedidoDaPropostaParaPago(ItemPedidoDTO itemPedido, string login, bool entrada = true)
        {
            if (itemPedido != null)
            {
                AlterarStatusItemPedido(itemPedido, 7, login);
                var pedidoPagamento = (entrada == true) ? 
                        _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(itemPedido.IPE_ID) :
                        _pedidoPagamentoSRV.RetornarPagamentoTirandoAEntrada(itemPedido.IPE_ID);

                _pedidoPagamentoSRV.MarcarPedidoPagamentoPago(pedidoPagamento, null);

                if (!_parcelasSRV.ExisteParcelasPagas(itemPedido.IPE_ID))
                {
                    var lstParcelas = _parcelasSRV.ObterProximasParcelasAPagar(itemPedido.IPE_ID);

                    if(lstParcelas != null)
                    {
                        foreach(var parcela in lstParcelas)
                        {
                            _parcelasSRV.DarBaixaNaParcela(parcela, null, true, false);
                        }
                    }
                }
                var cliente = itemPedido.PEDIDO_CRM.CLIENTES;
                _assinaturaSRV.ConcederAcessosDoPedido(itemPedido, cliente, login);
            }
        }


        public IList<ItemPedidoDTO> ListarItemPedidoDosPedidosFaturado(DateTime? dataFaturamentoAgendado)
        {
            return _dao.ListarItemPedidoDosPedidosFaturado(dataFaturamentoAgendado);
        }

        /// <summary>
        /// Método exclusívo para ADM do sistema para regerar em lote as notas fiscais que não existem.
        /// </summary>
        /// <param name="data"></param>
        //public void RegerarVariasNotasFiscais(DateTime? data, string path)
        //{
        //    var lstPedidos = ListarItemPedidoDosPedidosFaturado(data);

        //    if(lstPedidos != null && lstPedidos.Count() > 0)
        //    {

        //        TransactionOptions txOpt = new TransactionOptions();
        //        txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        //        txOpt.Timeout = TransactionManager.MaximumTimeout;

        //        using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
        //        {
        //            foreach (var ped in lstPedidos)
        //            {
        //                RegerarNotaFiscal(ped.IPE_ID, path);
        //            }
        //            scope.Complete();
        //        }
        //    }
        //}

        public void RegerarTodasAsNotasFiscais(string path)
        {
            var lstNfeXml = _nfeXmlSRV.ListarTodasAsNotasDoTipoProduto();

            if (lstNfeXml != null && lstNfeXml.Count() > 0)
            {

                TransactionOptions txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    foreach (var nfx in lstNfeXml)
                    {
                        GerarOuAtualizarNotaFiscal(nfx.IPE_ID, path, nfx.NFX_ID);
                    }
                    scope.Complete();
                }
            }
        }

        public COAD.FISCAL.Model.Enumerados.TipoPagamentoEnum RetornarTipoPagamentoEntrada(int? ipeId)
        {

            // Obtem a informação de pagamento que não seja a primeira.
            PedidoPagamentoDTO pagamentoRestante = _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(ipeId);
            if (pagamentoRestante == null)
                return FISCAL.Model.Enumerados.TipoPagamentoEnum.OUTROS;

            switch (pagamentoRestante.TPG_ID)
            {
                case 7: return FISCAL.Model.Enumerados.TipoPagamentoEnum.BOLETO_BANCARIO;
                case 8: return FISCAL.Model.Enumerados.TipoPagamentoEnum.CHEQUE;
                case 9: return FISCAL.Model.Enumerados.TipoPagamentoEnum.CARTAO_CREDITO;
                case 10: return FISCAL.Model.Enumerados.TipoPagamentoEnum.OUTROS;
            }

            return FISCAL.Model.Enumerados.TipoPagamentoEnum.OUTROS;

        }
        /// <summary>
        /// Verifica à partir das informações de pagamento se o pedido é à vista ou a prazo.
        /// </summary>
        /// <param name="ipeId"></param>
        /// <returns></returns>
        public bool EhVendaAVista(int? ipeId)
        {
            // Obtem a informação de pagamento que não seja a primeira.
            PedidoPagamentoDTO pagamentoRestante = _pedidoPagamentoSRV.RetornarPagamentoTirandoAEntrada(ipeId);

            // se exista pagamento restante significa que é uma venda a prazo.
            if (pagamentoRestante != null)
                return false;

            PedidoPagamentoDTO pagamentoDeEntrada = _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(ipeId);
            if(pagamentoDeEntrada == null)
            {
                throw new Exception("Não é possível determinar se o pagamento é a vista. As informações de pagamento não foram encontradas.");
            }

            int? parcelas = pagamentoDeEntrada.PGT_QTDE_PARCELAS;
            int? codigoTipoPagamento = pagamentoDeEntrada.TPG_ID;

            // Se o pagamento de entrada possui 1 parcela ou é cartão ou depósito trata-se de uma venda a vista
            if (parcelas == 1 || codigoTipoPagamento == 9 || codigoTipoPagamento == 8)
                return true;
            
            return false;
        }

        /// <summary>
        /// Método exclusívo para ADM do sistema para regerar em lote as notas fiscais que não existem.
        /// </summary>
        /// <param name="data"></param>
        //public void RegerarVariasNotasFiscais(string data, string path)
        //{
        //    if (!string.IsNullOrWhiteSpace(data))
        //    {

        //        string[] dataArray = data.Split('/');

        //        var dia = int.Parse(dataArray[0]);
        //        var mes = int.Parse(dataArray[1]);
        //        var ano = int.Parse(dataArray[2]);

        //        DateTime? dateObj = new DateTime(ano, mes, dia);

        //        RegerarVariasNotasFiscais(dateObj, path);
        //    }
        //}

        /// <summary>
        /// Retorna a quantidade de pedidos que foram faturadas, deveriam ter notas geradas, 
        /// mas inda não possuem nota até no máximo a data informada.
        /// Esse método é útil para checar se a numeração da nota não irá ser gerada ignorando contratos anteriores sem nota gerada.
        /// </summary>
        /// <param name="ateDataFaturamento">Pesquisa pedidos faturadas até essa data.</param>
        /// <returns></returns>
        public int? RetornarQtdPedidosNotaNaoGeradaPorData(DateTime? ateDataFaturamento, int? empId, int? ipeIdParaExcluir = null)
        {
            return _dao.RetornarQtdPedidosNotaNaoGeradaPorData(ateDataFaturamento, empId, ipeIdParaExcluir);
        }

        /// <summary>
        /// Retorna os pedidos que foram faturadas, deveriam ter notas geradas, 
        /// mas inda não possuem nota até no máximo a data informada.
        /// Esse método é útil para checar quais pedidos deveria ter suas notas geradas antes de gerar com a data informada.
        /// </summary>
        /// <param name="ateDataFaturamento">Pesquisa pedidos faturadas até essa data.</param>
        /// <returns></returns>
        public Pagina<PedidosRetroativosSemNotaDTO> RetornarPedidosNotaNaoGeradaPorData(DateTime? ateDataFaturamento, int? empId, int? ipeIdParaExcluir = null, int pagina = 1, int registrosPorPagina = 15)
        {
            var paginaPedidosRetro = _dao.RetornarPedidosNotaNaoGeradaPorData(ateDataFaturamento, empId, ipeIdParaExcluir, pagina, registrosPorPagina);
            //ObterDataDeFaturamentoDoContrato(paginaPedidosRetro);

            return paginaPedidosRetro;
        }

        
        //private void ObterDataDeFaturamentoDoContrato(Pagina<PedidosRetroativosSemNotaDTO> paginaPedidosRetroativos)
        //{
        //    if (paginaPedidosRetroativos != null && paginaPedidosRetroativos.lista != null)
        //    {
        //        var lstPedidos = paginaPedidosRetroativos.lista.ToList();
        //        foreach (var ped in lstPedidos)
        //        {
        //            var contratos = _contratoSRV.ListarContratosDoItemPedido(ped.CodigoItemPedido);
        //            if(contratos != null && contratos.Count() > 0)
        //            {
        //                var contrato = contratos.FirstOrDefault();
        //                ped.DataFaturamento = contrato.CTR_DATA_FAT;
        //            }
        //        }

        //        paginaPedidosRetroativos.lista = lstPedidos.OrderBy(x => x.DataFaturamento);

        //    }
        //}
        public ICollection<PreviewGeracaoNotaFiscalDTO> GerarPreviewNotaFiscal(int? ipeId)
        {
            var contratos = _contratoSRV.ListarContratosDoItemPedido(ipeId);
            ICollection<PreviewGeracaoNotaFiscalDTO> listaPreviewNFe = new HashSet<PreviewGeracaoNotaFiscalDTO>();

            if(contratos != null && contratos.Count() > 0)
            {
                foreach(var con in contratos)
                {
                    if(con.CTR_GERA_NOTA_FISCAL == null || con.CTR_GERA_NOTA_FISCAL == true)
                    {
                        var empresa = ServiceFactory.RetornarServico<EmpresaSRV>().FindById(con.EMP_ID);
                        var numeroNota = ServiceFactory.RetornarServico<CustomEmpresaSRV>().RetornarProvavelSequencialNFEEmpresa(con.EMP_ID);
                        var dataUltimoFaturamento = _contratoSRV.RetornarDataDoUltimoFaturamentoPorEmpresa(con.EMP_ID);
                        var dataFaturamento = con.CTR_DATA_FAT;
                        var qtdContratosRetroativosPendentes = RetornarQtdPedidosNotaNaoGeradaPorData(dataFaturamento, con.EMP_ID, ipeId);
                        
                        PreviewGeracaoNotaFiscalDTO previewGeracaoNfe = new PreviewGeracaoNotaFiscalDTO()
                        {
                            Valido = true,
                            IpeId = ipeId,
                            EmpId = con.EMP_ID,
                            Empresa = empresa,
                            NumeroNota = numeroNota,
                            DataFaturamento = dataFaturamento,
                            DataUltimoFaturamento = dataUltimoFaturamento,
                            QtdContratosRetroativosPendentes = qtdContratosRetroativosPendentes,                            
                        };

                        try
                        {
                            _notaFiscalSRV.ValidarDataFaturamento(con.EMP_ID, dataFaturamento, con.CTR_NUM_CONTRATO, ipeId);
                        }
                        catch(NFeException e)
                        {
                            previewGeracaoNfe.Valido = false;
                            previewGeracaoNfe.Mensagem = e.Message;
                        }

                        listaPreviewNFe.Add(previewGeracaoNfe);
                    }
                }
            }

            return listaPreviewNFe;
        }

        public ItensNfeXmlPedidoResponseDTO ChecarEListarItensNfeDoItemPedido(int? ipeId)
        {
            ItensNfeXmlPedidoResponseDTO response = new ItensNfeXmlPedidoResponseDTO();
            var lstNfeXml = _nfeXmlSRV.ListarNFeXmlPorItemPedido(ipeId);
            response.LstNfeXml = lstNfeXml;

            if(lstNfeXml != null && lstNfeXml.Count() <= 0)
            {
                var lstPreviewGeracao = GerarPreviewNotaFiscal(ipeId);
                response.PreviewGeracao = lstPreviewGeracao;
            }

            return response;
        }

        public void InserirPeriodoDeFaturamento(ItemPedidoDTO itemPedido, ContextoFaturamentoDTO faturamentoDTO)
        {
            var codigoDoItem = itemPedido.IPE_ID;
            var dataFaturamentoSRV = ServiceFactory.RetornarServico<DatasFaturamentoSRV>();
          
            DateTime? dataFaturamento = null;

            if (itemPedido != null)
            {
                if (faturamentoDTO.RequisicaoFaturamento != null &&
                   faturamentoDTO.RequisicaoFaturamento.LstRequisicaoFaturamento != null)
                {
                    var requiFat = faturamentoDTO.RequisicaoFaturamento;
                    var requiFatDetalhe = faturamentoDTO
                        .RequisicaoFaturamento
                        .LstRequisicaoFaturamento
                        .Where(x => x.IpeId == codigoDoItem)
                        .FirstOrDefault();

                    if (requiFatDetalhe != null)
                    {
                        if (requiFatDetalhe.DataFaturamento != null)
                        {
                            dataFaturamento = requiFatDetalhe.DataFaturamento;
                        }
                    }
                }

                if (dataFaturamento == null)
                    dataFaturamento = itemPedido.IPE_DATA_FATURAMENTO_SEMANA_FAT;              

                itemPedido.IPE_DATA_PRODUCAO = DateTime.Now;
                itemPedido.IPE_DATA_FATURAMENTO = dataFaturamento;
            }
        }

        public ItemPedidoDTO ListarItemPedidoDaAssinatura(string assinatura)
        {
            return _dao.ListarItemPedidoDaAssinatura(assinatura);
        }


        
        public BatchContext GerarOuAtualizarVariasNotasFiscais(NotaFiscalBatchDTO notaFiscalBatch)
        {
            BatchContext batchContext = new BatchContext();
            if (notaFiscalBatch != null && notaFiscalBatch.ListCodPedidos != null)
            {
                
                string path = notaFiscalBatch.Path;
                var lstCodPedidos = notaFiscalBatch.ListCodPedidos.Select(x => x.CodPedido).ToList();
                var lstContratos = _contratoSRV.ListarContratosQGeraNota(lstCodPedidos, 1);

                if(lstContratos == null && lstContratos.Count() < 0)
                {
                    string chaveErro = "Contratos";
                    string mensagem = "Nenhum dos contratos dos pedidos selecionados estão marcados para gerar nota fiscal.";
                    batchContext.ListErros.Add(new ErroReportItemDTO()
                    {
                        Contexto = chaveErro,
                        Mensagem = mensagem
                    });
                }
                var resultado = GerarOuAtualizarNotaFiscal(lstContratos, path, batchContext);
                batchContext.Path = GerarZipNFe(resultado, path, batchContext);
            }
            return batchContext;
        }

        public BatchContext DownloadVariasNotas(NotaFiscalBatchDTO notaFiscalBatch)
        {
            BatchContext batchContext = new BatchContext();
            string contexto = null;
            //string mensagem = null;
                if (notaFiscalBatch != null && notaFiscalBatch.ListCodPedidos != null)
                {
                    string path = notaFiscalBatch.Path;
                    var lstCodPedidos = notaFiscalBatch.ListCodPedidos.Select(x => x.CodPedido).ToList();

                    IList<NfeXmlDTO> lstNfeXml = new List<NfeXmlDTO>();
                    foreach (var ped in lstCodPedidos)
                    {
                        try
                        {
                            var lstNfeXmlPorPed = _nfeXmlSRV.ListarNFeXmlPorPedido(ped);
                            if (lstNfeXmlPorPed == null || lstNfeXmlPorPed.Count() <= 0)
                            {

                                contexto = string.Format("Pedido: {0}", ped);
                                throw new NFeException(string.Format("O Pedido {0} não possui nenhuma nota para baixar.", ped));
                            }
                            else
                            {
                                ++batchContext.TotalExito;
                            }
                            lstNfeXml = lstNfeXml.Concat(lstNfeXmlPorPed).ToList();
                        }
                        catch(Exception e)
                        {
                            ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarErroBatch(new RegistroErroBatchDTO()
                            {
                                batchEx = batchContext,
                                context = contexto,
                                e = e,
                                nomeDaExecucao = "Geração de Lote Nota Fiscal",
                                projeto = "CORPORATIVO",
                                servico = "ItemPedidoSRV",
                                tipoJob = 4
                            });
                
                        }
                    }
                    batchContext.Path = GerarZipNFe(lstNfeXml, path, batchContext);
                }
            return batchContext;

        }

        private string GerarZipNFe(IList<NfeXmlDTO> lstNfe, string path, BatchContext batchContext = null)
        {
            ZipFile zipFile = new ZipFile();
            var lstArqNfe = _nfeXmlSRV.ChecarERetornarFileName(lstNfe, path, true, batchContext);
            zipFile.AddFiles(lstArqNfe,"");

            string serverFolder = SysUtils.RetornarPathNFeXML();
            string zipName = string.Format("notas_fiscais({0:yyyy-MM-ddThh-mm}).zip", DateTime.Now);
            string fullPath = Path.Combine(path, serverFolder, zipName);

            zipFile.Save(fullPath);
            return zipName;
        }

        /// <summary>
        /// Retorna as informações necessárias para informar o usuário as condições de cancelamento de um pedido.
        /// </summary>
        /// <param name="pedCrmId"></param>
        /// <returns></returns>
        public CancelamentoDTO GerarDadosIniCancelamento(int? pedCrmId)
        {
            CancelamentoDTO cancelamentoDTO = new CancelamentoDTO();
            var pedido = ServiceFactory
                .RetornarServico<PedidoCRMSRV>()
                .FindById(pedCrmId);

            var lstItemPedido = ListarItemPedidoDoPedido(pedCrmId);

            if(lstItemPedido != null)
            {
                cancelamentoDTO.CLI_ID = pedido.CLI_ID;
                cancelamentoDTO.PED_CRM_ID = pedCrmId;
                    
                foreach(var itm in lstItemPedido)
                {
                    var itemCancelamento = new CancelamentoItemDTO();
                    itemCancelamento.ipeId = itm.IPE_ID;
                    itemCancelamento.EmpId = pedido.EMP_ID;
                    itemCancelamento.NomeProduto = itm.PRODUTO_COMPOSICAO.CMP_DESCRICAO;
                    itemCancelamento.ValorPedido = itm.IPE_TOTAL;
                    itemCancelamento.PstId = itm.PST_ID;
                    itemCancelamento.Assinatura = itm.ASN_NUM_ASSINATURA;
                    itemCancelamento.NomeCliente = pedido.CLIENTES.CLI_NOME;
                    itemCancelamento.DadosCancelamentoPai = cancelamentoDTO.Clone();
                    itemCancelamento.DadosCancelamentoPai.Itens = null;
                    
                    var emailDTO = ServiceFactory.RetornarServico<AssinaturaEmailSRV>().RetornarEmailDeContato(pedido.CLI_ID);
                    if (emailDTO != null)
                        itemCancelamento.Email = emailDTO.AEM_EMAIL;

                    ChecarSeNotaPodeExtornar(itm.IPE_ID, itemCancelamento);

                    if (new int?[]{2, 3, 6, 7 }.Contains(itm.PST_ID) && !string.IsNullOrWhiteSpace(itm.ASN_NUM_ASSINATURA))
                    {
                        itemCancelamento.EnviarEmailCancAssi = true;
                    }

                    cancelamentoDTO.Itens.Add(itemCancelamento);
                }

            }

            return cancelamentoDTO;
            
        }

        /// <summary>
        /// Realiza uma série de validações para determinar se no processo de cancelamento o número da nota pode ser extornada.
        /// </summary>
        /// <returns></returns>
        public void ChecarSeNotaPodeExtornar(int? ipeId, CancelamentoItemDTO cancelamentoDTO)
        {
            cancelamentoDTO.ValidadoPraExtornarNumNota = false;
            cancelamentoDTO.ExtornarNumeroNotaFiscal = false;

            //var lstNotasFiscais = _nfeXmlSRV.ListarNFeXmlProdutoPorItemPedido(ipeId);
            //if (lstNotasFiscais != null && lstNotasFiscais.Count() > 0)
            //{
            //    var lstOrdenada = lstNotasFiscais.OrderBy(x => x.NFX_NUMERO_NOTA);
            //    var lstNumero = lstOrdenada.Select(x => x.NFX_NUMERO_NOTA);
            //    int? numeroInicial = lstNumero.FirstOrDefault();
            //    int? numeroFinal = lstNumero.LastOrDefault();
            //    cancelamentoDTO.ValidadoPraExtornarNumNota = true;
            //    cancelamentoDTO.ExtornarNumeroNotaFiscal = true;
            //    if (lstNotasFiscais.Count() > 1)
            //    {
            //        int index = 0;
            //        foreach(var numero in lstOrdenada)
            //        {
            //            if(numeroInicial != numeroInicial + index)
            //            {
            //                string mensage = "O Item de pedido {0} possuí mais de 1 nota fiscal. Mas seus números não não seguencias. Não é possível extornar essa nota. Numero de Notas [{1}]";

            //                StringBuilder sb = new StringBuilder();

            //                int subIndex = 0;
            //                foreach(var subNumero in lstNumero)
            //                {
            //                    sb.Append(subNumero);
            //                    if (subIndex < lstOrdenada.Count() - 1)
            //                        sb.Append(",");
            //                }

            //                mensage = string.Format(mensage, ipeId, sb.ToString());
            //                cancelamentoDTO.MotivoNaoPodeExtornar = mensage;
            //                cancelamentoDTO.ValidadoPraExtornarNumNota = false;
            //                cancelamentoDTO.ExtornarNumeroNotaFiscal = false;
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        var empresa = ServiceFactory.RetornarServico<EmpresaSRV>().FindById(cancelamentoDTO.EmpId);
            //        if(empresa == null)
            //        {
            //            cancelamentoDTO.MotivoNaoPodeExtornar = string.Format("Não é possível extornar a nota. Empresa de código {0} não foi localizada", cancelamentoDTO.EmpId);
            //            cancelamentoDTO.ValidadoPraExtornarNumNota = false;
            //            cancelamentoDTO.ExtornarNumeroNotaFiscal = false;
            //        }
            //        var ultimoNumeroGerado = ServiceFactory.RetornarServico<CustomEmpresaSRV>().RetornarUltimoNumNotaNFEEmpresa(cancelamentoDTO.EmpId);
            //        if(numeroFinal != ultimoNumeroGerado)
            //        {
            //            string mensagem = string.Format("O item de pedido '{0}' possúi o número de nota '{1}' mas o último numero gerado para a empresa '{2}' foi o '{3}'. Por tanto essa nota não pode ser extornada.",
            //                ipeId, numeroFinal, empresa.EMP_NOME_FANTASIA, ultimoNumeroGerado);
            //            cancelamentoDTO.MotivoNaoPodeExtornar = mensagem;
            //            cancelamentoDTO.ValidadoPraExtornarNumNota = false;
            //            cancelamentoDTO.ExtornarNumeroNotaFiscal = false;
            //        }
            //    }

            //}
        }

        public void ExtornarNotaFiscal(CancelamentoItemDTO cancelamentoDTO)
        {
            //var validado = cancelamentoDTO.ValidadoPraExtornarNumNota;
            //if (validado)
            //{
            //    ChecarSeNotaPodeExtornar(cancelamentoDTO.ipeId, cancelamentoDTO);
            //}

            //if (!cancelamentoDTO.ValidadoPraExtornarNumNota)
            //{
            //    throw new Exception("Não é possível extornar a nota. A validação falhou. " + cancelamentoDTO.MotivoNaoPodeExtornar);
            //}

            var usuario = cancelamentoDTO.DadosCancelamentoPai.USU_LOGIN;
            var repId = cancelamentoDTO.DadosCancelamentoPai.REP_ID;
            var pstId = cancelamentoDTO.PstId;
            var ipeId = cancelamentoDTO.ipeId;
            var lstXmlNfe = _nfeXmlSRV.ListarNFeXmlProdutoPorItemPedido(cancelamentoDTO.ipeId);
            if(lstXmlNfe != null)
            {
                var lstOrdenada = lstXmlNfe.OrderByDescending(x => x.NFX_NUMERO_NOTA);
                foreach(var nfeXml in lstOrdenada)
                {
                    var numero = nfeXml.NFX_NUMERO_NOTA;
                    var empId = cancelamentoDTO.EmpId;

                    numero--;
                    ServiceFactory.RetornarServico<CustomEmpresaSRV>().AlterarNumeroNota(empId, numero);

                    _historicoSRV.RegistrarHistoricoNfeXmlExtornada(usuario, repId, pstId, ipeId, nfeXml.NFX_NUMERO_NOTA);
                }
                _nfeXmlSRV.MarcarComoExtornado(lstXmlNfe);
            }
        }

        public string GerarHTMLEmailCanc(CancelamentoItemDTO cancelamento)
        {
            var nomeCliente = cancelamento.NomeCliente;
            var assinatura = cancelamento.Assinatura;
            var mensagem = cancelamento.DadosCancelamentoPai.MengagemEmailAssCanc;

            var templateEmail =
                    @"<div style='padding:15px;'>
                        <fieldset style='border:none;'>
                            <legend style='font-size:16px; color: #0970a3;'><strong>Seu pedido foi cancelado.</strong></legend>
                            <form>
                                <br />
                                <p>
                                    Prezado <strong>{0}</strong> 
                                    <br />
                                    O seu pedido da Assinatura {1} foi cancelado.                                    
                                </p>
                                <label>Mensagem da Operadora.</label> <br />
                                <div style='font-size:14px; background-color: #c3c3c3;'>
                                    {2}                            
                                </div>
                                <br />
                                <br />
                            </form>
                        </fieldset>                    
                    </div>";

            templateEmail = string.Format(templateEmail, nomeCliente, assinatura, cancelamento.DadosCancelamentoPai.MengagemEmailAssCanc);
            return templateEmail;
        }

        public string GerarPreviewHTMLEmailCanc(CancelamentoDTO cancelamento)
        {
            if(cancelamento != null && 
                cancelamento.Itens != null && 
                cancelamento.Itens.Count() > 0)
            {
                var itemCan = cancelamento.Itens.FirstOrDefault();
                itemCan.DadosCancelamentoPai = cancelamento;
                var template = GerarHTMLEmailCanc(itemCan);
                template = emailSRV.GerarTemplateCliente(template);
                return template;
            }

            return null;
        }

        /// <summary>
        /// Enviar um email cancelamento da Assinatura.
        /// </summary>
        /// <param name="itemCancelamento"></param>
        public void EnviarEmailCancAssinatura(CancelamentoItemDTO itemCancelamento)
        {
            var template = GerarHTMLEmailCanc(itemCancelamento);
            var endEmail = SysUtils.DecidirEnderecoDeEmail(itemCancelamento.Email);
            if (endEmail != null)
            {
                //var url = "https://ci4.googleusercontent.com/proxy/GgWnRPBud6_dbgT5a4AZGD1cXJaq7heSiSI6uRSLpqrbeRczzyf8rGzRft8ARSffAAjCKNryW9c1grWR6aZ4DfbBnsH6SAPgdbI5SsEUK5ISOjmLsiZKwAW0iJfwmKPQF_ufrNjh0VNiRRastLGv7F1SB7KA=s0-d-e1-ft#http://emkt.coad.com.br/emkt/dados/10268/10767/Image/Cursos_Novo/Header_Contabilidade_Geral.png";
                emailSRV.EnviarEmailParaCliente(endEmail, "O seu pedido foi cancelado.", template);
            }

        }

        /// <summary>
        /// Envia a nota fiscal de produto e serviço
        /// </summary>
        /// <param name="notaFiscalBatch"></param>
        /// <returns></returns>
        public IList<INFeLote> AdicionarVariasNotasAoLote(NotaFiscalBatchDTO notaFiscalBatch, BatchContext batchContext)
        {
           var lstLote = AdicionarVariasNotasAoLoteNfe(notaFiscalBatch, batchContext);

            var lstLoteNfse = AdicionarVariasNotasAoLoteNFse(notaFiscalBatch, batchContext);

            var lstLoteRetorno = lstLote.Concat(lstLoteNfse).ToList();
            //return lstLoteNfse;
            return lstLote;
        }

        public IList<INFeLote> AdicionarVariasNotasAoLoteNfe(NotaFiscalBatchDTO notaFiscalBatch, BatchContext batchContext)
        {
            IList<INFeLote> retorno = new List<INFeLote>();
            if (batchContext == null)
                batchContext = new BatchContext();

            if (notaFiscalBatch != null && notaFiscalBatch.ListCodPedidos != null)
            {
                var lstCodPedidos = notaFiscalBatch.ListCodPedidos.Select(x => x.CodPedido).ToList();
                var lstContratos = _contratoSRV.ListarContratosQGeraNota(lstCodPedidos, 1);

                if (lstContratos == null || lstContratos.Count() <= 0)
                {
                    string chaveErro = "Contratos";
                    string mensagem = "Nenhum dos contratos dos pedidos selecionados estão marcados para gerar nota fiscal. Ou os pedidos não estão faturados.";
                    batchContext.ListErros.Add(new ErroReportItemDTO()
                    {
                        Contexto = chaveErro,
                        Mensagem = mensagem
                    });
                    batchContext.TotalFalha++;
                    return retorno;
                }

                var lstEmpresas = lstContratos.Select(x => x.EMP_ID).Distinct();


                foreach (var emp in lstEmpresas)
                {
                    var requisicao = new RequisicaoNovoLote();
                    requisicao.EmpresaID = emp;
                    var contratosDaEmpresa = lstContratos.Where(x => x.EMP_ID == emp);
                    foreach (var con in contratosDaEmpresa)
                    {                        
                        if (ValidarContrato(con, batchContext))
                        {
                            ProcessarAdicaoLoteNf(requisicao, con, 1, batchContext);
                        }

                    }
                    if (requisicao != null && requisicao.LstRequisicoes.Count > 0)
                    {
                        var lote = ServiceFactory.RetornarServico<NotaFiscalSRV>().CriarNovoLote(requisicao);
                        if (lote != null)
                        {
                            retorno.Add(lote);
                        }
                    }
                    else
                    {
                        batchContext.TotalFalha++;
                        batchContext.ListErros.Add(new ErroReportItemDTO()
                        {
                            Contexto = string.Format("Cod. Empresa. {0}", requisicao.EmpresaID),
                            Mensagem = string.Format("Não é possível enviar um lote de NFe para a empresa {0}. Não á nenhum pedido válido para essa empresa.", requisicao.EmpresaID)
                        });

                    }
                }
                //ServiceFactory.RetornarServico<NotaFiscalSRV>().EnviarLoteVigente(notaFiscalBatch.Path, numeroTentativas);

            }
            return retorno;
        }



        /// <summary>
        /// Envia a nota fiscal de Serviço Nota Carioca
        /// </summary>
        /// <param name="notaFiscalBatch"></param>
        /// <returns></returns>
        public IList<INFeLote> _old_AdicionarVariasNotasAoLoteNFse(NotaFiscalBatchDTO notaFiscalBatch, BatchContext batchContext, bool levantarErroLoteVazio = false)
        {
            IList<INFeLote> retorno = new List<INFeLote>();
            if (batchContext == null)
                batchContext = new BatchContext();

            if (notaFiscalBatch != null && 
                notaFiscalBatch.ListCodPedidos != null && 
                levantarErroLoteVazio == false)
            {
                var lstCodPedidos = notaFiscalBatch.ListCodPedidos.Select(x => x.CodPedido).ToList();
                var lstContratos = _contratoSRV.ListarContratosQGeraNota(lstCodPedidos, 2);

                if (lstContratos == null || lstContratos.Count() <= 0)
                {
                    string chaveErro = "Contratos";
                    string mensagem = "Não foi encontrado nenhum contrato que gere nota de serviço. AS causas prováveis são. Os contratos não estão marcados para gerar nota fiscal. O pedidos não estão faturados. O contrato não é um contrato de serviço.";
                    batchContext.ListErros.Add(new ErroReportItemDTO()
                    {
                        Contexto = chaveErro,
                        Mensagem = mensagem
                    });
                    batchContext.TotalFalha++;
                    return retorno;
                }

                var lstEmpresas = lstContratos.Select(x => x.EMP_ID).Distinct();


                foreach (var emp in lstEmpresas)
                {
                    var requisicao = new RequisicaoNovoLote();
                    requisicao.EmpresaID = emp;
                    requisicao.TipoLote = TipoLoteEnum.ENVIO_LOTE_RPS_NFSE;
                    var contratosDaEmpresa = lstContratos.Where(x => x.EMP_ID == emp);
                    foreach (var con in contratosDaEmpresa)
                    {
                        var lotesItens = ServiceFactory.RetornarServico<NotaFiscalLoteItemSRV>().ListarItensDoLoteNFsePendentePorItemPedido(con.IPE_ID, null);
                        var notas = ServiceFactory.RetornarServico<NotaFiscalSRV>().ListarNotasDeServicoDeEntradaEnviada(con.IPE_ID, null);

                        if (lotesItens.Count > 0)
                        {
                            batchContext.TotalFalha++;
                            batchContext.ListErros.Add(new ErroReportItemDTO()
                            {
                                Contexto = string.Format("Cod. Ped. {0}", con.IPE_ID),
                                Mensagem = string.Format("Esse pedido já possuí nota de serviço pendente de envio. Aguarde o final do processamento.", requisicao.EmpresaID)
                            });
                            continue;
                        }

                        if (notas.Count > 0)
                        {
                            batchContext.TotalFalha++;
                            batchContext.ListErros.Add(new ErroReportItemDTO()
                            {
                                Contexto = string.Format("Cod. Ped. {0}", con.IPE_ID),
                                Mensagem = string.Format("Esse pedido já possuí nota de serviço gerada.", requisicao.EmpresaID)
                            });
                            continue;
                        }

                        if (ValidarContrato(con, batchContext))
                        {
                            requisicao.LstRequisicoes.Add(new RequisicaoNovoLoteItem()
                            {
                                CodContrato = con.CTR_NUM_CONTRATO,
                                CodPedido = con.IPE_ID
                            });
                        }

                    }
                    if (requisicao != null && requisicao.LstRequisicoes.Count > 0)
                    {
                        var lote = ServiceFactory.RetornarServico<NotaFiscalSRV>().CriarNovoLote(requisicao);
                        if (lote != null)
                        {
                            retorno.Add(lote);
                        }
                    }
                    else
                    {
                        batchContext.TotalFalha++;
                        batchContext.ListErros.Add(new ErroReportItemDTO()
                        {
                            Contexto = string.Format("Cod. Empresa. {0}", requisicao.EmpresaID),
                            Mensagem = string.Format("Não é possível enviar um lote de NFe para a empresa {0}. Não á nenhum pedido válido para essa empresa.", requisicao.EmpresaID)
                        });

                    }
                }
                //ServiceFactory.RetornarServico<NotaFiscalSRV>().EnviarLoteVigente(notaFiscalBatch.Path, numeroTentativas);

            }
            return retorno;
        }

        /// <summary>
        /// Adiciona o pedido a ao lote de notas fiscais a serem enviados.
        /// </summary>
        /// <param name="ipeId"></param>
        /// <param name="batchContext"></param>
        public bool ValidarContrato(ContratoDTO contrato, BatchContext batchContext, bool servico = false)
        {
            if(contrato == null)
            {
                batchContext.TotalFalha++;
                batchContext.ListErros.Add(new ErroReportItemDTO()
                {
                    Contexto = "Contrato",
                    Mensagem = "Não é possível encontrar o contrato"
                });

                return false;
            }    

            if(contrato.CTR_DATA_CANC != null)
            {
                batchContext.TotalFalha++;
                batchContext.ListErros.Add(new ErroReportItemDTO()
                {
                    Contexto = string.Format("Contrato Cod. {0}", contrato.CTR_NUM_CONTRATO),
                    Mensagem = "Não é possível gerar nota de um contrato cancelado."
                });

                return false;
            }

            if (contrato.CTR_CORTESIA == 1)
            {
                batchContext.TotalFalha++;
                batchContext.ListErros.Add(new ErroReportItemDTO()
                {
                    Contexto = string.Format("Contrato Cod. {0}", contrato.CTR_NUM_CONTRATO),
                    Mensagem = "Não é possível gerar nota de um contrato de cortesia."
                });

                return false;
            }

            
            var ipeId = contrato.IPE_ID;
            var itemPedido = FindById(ipeId);
            if (itemPedido == null)
            {
                batchContext.TotalFalha++;
                batchContext.ListErros.Add(new ErroReportItemDTO()
                {
                    Contexto = ipeId + "",
                    Mensagem = string.Format("Não é possível achar o pedido de código {0}.", ipeId)
                });

                return false;
            }

            //if((servico && contrato.CTR_SERVICO == true && 
            //                (contrato.CTR_VLR_PRODUTO == null || contrato.CTR_VLR_PRODUTO > 0)))
            //{
            //    batchContext.TotalFalha++;
            //    batchContext.ListErros.Add(new ErroReportItemDTO()
            //    {
            //        Contexto = string.Format("Cod. Itm. Pedido: {0}", ipeId),
            //        Mensagem = string.Format("Não é possível gerar a nota para o Pedido {0}. O contrato não possui valor de produto.", ipeId)
            //    });
            //    return false;
            //}

            if (itemPedido.PST_ID == 3 || 
                itemPedido.PST_ID == 11 || 
                itemPedido.PST_ID == 12 || 
                itemPedido.PST_ID == 13 || 
                itemPedido.PST_ID == 14)
            {
                var cliId = ServiceFactory.RetornarServico<PedidoCRMSRV>().RetornarCliIdDoPedidoPorItemPedido(ipeId);
                var cliente = _clienteSRV.FindById(cliId);
                var endereco = ServiceFactory.RetornarServico<ClienteEnderecoSRV>().BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliente);
                endereco.validacaoTotal = true;

                var validacaoCliente = ValidatorProxy.RecursiveValidate(cliente);
                var validacaoEndereco = ValidatorProxy.RecursiveValidate(endereco);

                ICollection<NotaFiscalDTO> lstNotas = null;

                if (!validacaoCliente.IsValid || !validacaoEndereco.IsValid)
                {
                    batchContext.TotalFalha++;
                    batchContext.ListErros.Add(new ErroReportItemDTO(validacaoCliente, validacaoEndereco)
                    {
                        Contexto = ipeId + "",
                        Mensagem = string.Format("Não é possível gerar a nota para o Pedido {0}. Ocorreu erros de validação no cliente ou endereço", ipeId)
                    });
                    return false;
                }
                
                batchContext.TotalExito++;
                return true;
                
            }
            else
            {
                string msg = null;
                if (itemPedido.PST_ID == 12)
                {
                    msg = string.Format("O pedido já possui status de nota fiscal emitida.", ipeId);
                }

                batchContext.TotalFalha++;
                batchContext.ListErros.Add(new ErroReportItemDTO() {
                    Contexto = string.Format("Cod. Itm. Pedido: {0}", ipeId),
                    Mensagem = string.Format("O pedido de Código {0} não pode gerar nota. {1}.", ipeId, msg)
                });
            }

            return false;
        }

        public void AlterarStatusPedidoNotaFiscalRejeitada(ItemPedidoDTO itemPedido, int? numeroNota, int? codRetorno, string motivo)
        {
            AlterarStatusItemPedido(itemPedido, 11, "COADCORP");
            _historicoSRV.RegistrarHistoricoNotaFiscalRejeitada("COADCORP", 1, itemPedido.IPE_ID, numeroNota, codRetorno, motivo);
        }

        public void AlterarStatusPedidoNotaFiscalAutorizada(ItemPedidoDTO itemPedido, int? numeroNota, int? codRetorno, string motivo, string codContrato = null, bool servico = false)
        {
            if(itemPedido.PST_ID != 5 && !string.IsNullOrWhiteSpace(codContrato))
            {
                var contrato = _contratoSRV.FindById(codContrato);
                var status = RetornarStatusNotasEnviadas(itemPedido, contrato, servico);
                AlterarStatusItemPedido(itemPedido, status, "COADCORP");

                if(contrato.CTR_SERVICO == true)
                {
                    _historicoSRV.RegistrarHistoricoNotaFiscalServicoAutorizada("COADCORP", 1, itemPedido.IPE_ID, numeroNota);
                }
                else
                {
                    _historicoSRV.RegistrarHistoricoNotaFiscalAutorizada("COADCORP", 1, itemPedido.IPE_ID, numeroNota, codRetorno, motivo);
                }
            }
        }

        public int? RetornarStatusNotasEnviadas(ItemPedidoDTO itemPedido, ContratoDTO contrato, bool servico = false)
        {
            //var notaSRV = ServiceFactory.RetornarServico<NotaFiscalSRV>();

            //if (contrato.CTR_SERVICO == null || contrato.CTR_SERVICO == false)
            //    return 12;

            //if(contrato.CTR_SERVICO == true)
            //{
            //    if(contrato.CTR_VLR_PRODUTO == null)
            //    {
            //        return 12;
            //    }
            //    else
            //    {
            //        var notas = notaSRV.ListarNotasDeEntradaEnviada(itemPedido.IPE_ID);
            //        var notasServico = notaSRV.ListarNotasDeServicoDeEntradaEnviada(itemPedido.IPE_ID);

            //        if (notas != null && notasServico != null && notas.Count > 0 && notasServico.Count > 0)
            //        {
            //            return 12;
            //        }
            //        else
            //        if (servico)
            //        {
            //            if (notas == null && notas.Count <= 0)
            //                return 13;
            //        }
            //        else
            //        {
            //            if (notasServico == null && notasServico.Count <= 0)
            //                return 14;
            //        }
                    
            //    }
            //}
            
            return 12;
        }

        public void AlterarStatusPedidoNotaFiscalAntecipada(ItemPedidoDTO itemPedido, int? numeroNota)
        {
            AlterarStatusItemPedido(itemPedido, 12, "COADCORP");
            _historicoSRV.RegistrarHistoricoNotaFiscalAntecipada("COADCORP", 1, itemPedido.IPE_ID, numeroNota);
        }

        public void MarcarParcelasDoItemPedidoPago(ItemPedidoDTO itemPedido, string login, bool entrada = true)
        {
            if (itemPedido != null)
            {
                AlterarStatusItemPedido(itemPedido, 7, login);
                var pedidoPagamento = (entrada == true) ?
                        _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(itemPedido.IPE_ID) :
                        _pedidoPagamentoSRV.RetornarPagamentoTirandoAEntrada(itemPedido.IPE_ID);

                _pedidoPagamentoSRV.MarcarPedidoPagamentoPago(pedidoPagamento, null);

                if (!_parcelasSRV.ExisteParcelasPagas(itemPedido.IPE_ID))
                {
                    var lstParcelas = _parcelasSRV.ListarParcelaPorPedido(itemPedido.IPE_ID);

                    if (lstParcelas != null)
                    {
                        foreach (var parcela in lstParcelas)
                        {
                            _parcelasSRV.DarBaixaNaParcela(parcela, null, true, false);
                        }
                    }
                }
                var cliente = itemPedido.PEDIDO_CRM.CLIENTES;
                _assinaturaSRV.ConcederAcessosDoPedido(itemPedido, cliente, login);
            }
        }
        

        /// <summary>
        /// Envia a nota fiscal de Serviço Nota Carioca
        /// </summary>
        /// <param name="notaFiscalBatch"></param>
        /// <returns></returns>
        public IList<INFeLote> AdicionarVariasNotasAoLoteNFse(NotaFiscalBatchDTO notaFiscalBatch, BatchContext batchContext, bool levantarErroLoteVazio = false)
        {
            IList<INFeLote> retorno = new List<INFeLote>();
            if (batchContext == null)
                batchContext = new BatchContext();

            if (notaFiscalBatch != null &&
                notaFiscalBatch.ListCodPedidos != null &&
                levantarErroLoteVazio == false)
            {
                var lstCodPedidos = notaFiscalBatch.ListCodPedidos.Select(x => x.CodPedido).ToList();
                var lstContratos = _contratoSRV.ListarContratosQGeraNota(lstCodPedidos, 2);

                if (lstContratos == null || lstContratos.Count() <= 0)
                {
                    string chaveErro = "Contratos";
                    string mensagem = "Não foi encontrado nenhum contrato que gere nota de serviço. AS causas prováveis são. Os contratos não estão marcados para gerar nota fiscal. O pedidos não estão faturados. O contrato não é um contrato de serviço.";
                    batchContext.ListErros.Add(new ErroReportItemDTO()
                    {
                        Contexto = chaveErro,
                        Mensagem = mensagem
                    });
                    batchContext.TotalFalha++;
                    return retorno;
                }

                var lstEmpresas = lstContratos.Select(x => x.EMP_ID).Distinct();


                foreach (var emp in lstEmpresas)
                {
                    var requisicao = new RequisicaoNovoLote();
                    requisicao.EmpresaID = emp;
                    requisicao.TipoLote = TipoLoteEnum.ENVIO_LOTE_RPS_NFSE;
                    var contratosDaEmpresa = lstContratos.Where(x => x.EMP_ID == emp);
                    foreach (var con in contratosDaEmpresa)
                    {

                        if (ValidarContrato(con, batchContext))
                        {
                            ProcessarAdicaoLoteNf(requisicao, con, 2, batchContext);
                        }

                    }
                    if (requisicao != null && requisicao.LstRequisicoes.Count > 0)
                    {
                        var lote = ServiceFactory.RetornarServico<NotaFiscalSRV>().CriarNovoLote(requisicao);
                        if (lote != null)
                        {
                            retorno.Add(lote);
                        }
                    }
                    else
                    {
                        batchContext.TotalFalha++;
                        batchContext.ListErros.Add(new ErroReportItemDTO()
                        {
                            Contexto = string.Format("Cod. Empresa. {0}", requisicao.EmpresaID),
                            Mensagem = string.Format("Não é possível enviar um lote de NFe para a empresa {0}. Não á nenhum pedido válido para essa empresa.", requisicao.EmpresaID)
                        });

                    }
                }
                //ServiceFactory.RetornarServico<NotaFiscalSRV>().EnviarLoteVigente(notaFiscalBatch.Path, numeroTentativas);

            }
            return retorno;
        }

        public void ProcessarAdicaoLoteNf(RequisicaoNovoLote requisicao, 
            ContratoDTO contrato,
            int? nctId,
            BatchContext batchContext)
        {

            if(contrato != null && contrato.CMP_ID != null && contrato.IPE_ID != null) 
            {
                var itemPedido = FindById(contrato.IPE_ID);

                if (itemPedido != null && itemPedido.PEDIDO_CRM != null)
                {
                    var cemPorcentoFaturado = (itemPedido.PEDIDO_CRM.PED_CEM_POR_CENTO_FATURADO == true);
                    var lstConfigNota = _notaFiscalConfigSRV.ListarNotaFiscalConfig(contrato.CMP_ID, nctId, cemPorcentoFaturado);

                    if (lstConfigNota != null)
                    {
                        var notaFiscalLoteSRV = ServiceFactory.RetornarServico<NotaFiscalLoteItemSRV>();
                        var notaFiscalSRV = ServiceFactory.RetornarServico<NotaFiscalSRV>();

                        foreach (var conf in lstConfigNota)
                        {

                            ICollection<NotaFiscalDTO> notas = null;
                            ICollection<NotaFiscalLoteItemDTO> lotesItens = null;

                            if (nctId == 2)
                            {
                                lotesItens = notaFiscalLoteSRV.ListarItensDoLoteNFsePendentePorItemPedido(contrato.IPE_ID, conf.NFC_ID);
                                notas = notaFiscalSRV.ListarNotasDeServicoDeEntradaEnviada(contrato.IPE_ID, conf.NFC_ID);
                            }
                            else
                            {
                                lotesItens = notaFiscalLoteSRV.ListarItensDoLoteNFePendentePorItemPedido(contrato.IPE_ID, conf.NFC_ID);
                                notas = notaFiscalSRV.ListarNotasDeEntradaEnviada(contrato.IPE_ID, conf.NFC_ID);
                            }

                            if (lotesItens.Count > 0)
                            {
                                batchContext.TotalFalha++;
                                batchContext.ListErros.Add(new ErroReportItemDTO()
                                {
                                    Contexto = string.Format("Cod. Ped. {0}. NFC {1}", contrato.IPE_ID, conf.NFC_ID),
                                    Mensagem = string.Format("Esse pedido já possuí nota de serviço pendente de envio. Aguarde o final do processamento.", requisicao.EmpresaID)
                                });
                                return;
                            }

                            if (notas.Count > 0)
                            {
                                batchContext.TotalFalha++;
                                batchContext.ListErros.Add(new ErroReportItemDTO()
                                {
                                    Contexto = string.Format("Cod. Ped. {0}. NFC {1}", contrato.IPE_ID, conf.NFC_ID),
                                    Mensagem = string.Format("Esse pedido já possuí nota de serviço gerada.", requisicao.EmpresaID)
                                });
                                return;
                            }

                            requisicao.LstRequisicoes.Add(new RequisicaoNovoLoteItem()
                            {
                                CodContrato = contrato.CTR_NUM_CONTRATO,
                                CodPedido = contrato.IPE_ID,
                                NfConfigID = conf.NFC_ID
                            });
                        }
                    }
                }
            }
        }

        public void MudarItemPedidoDaPropostaAprovadoCliente(ItemPedidoDTO itemPedido, string login, bool entrada = true)
        {
            if (itemPedido != null)
            {
                AlterarStatusItemPedido(itemPedido, 4, login);
                var cliente = itemPedido.PEDIDO_CRM.CLIENTES;
                _assinaturaSRV.ConcederAcessosDoPedido(itemPedido, cliente, login);
            }
        }

    }
}