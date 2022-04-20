using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Coad.GenericCrud.Exceptions;
using GenericCrud.Exceptions.ErrorHandling;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.Filters;
using COAD.CORPORATIVO.Util;
using GenericCrud.Service;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service.Custons.Context;

namespace COADCORP.Areas.franquia.Controllers
{
    public class PedidoController : Controller
    {
        public const int UEN_ID = 1;
        public PedidoCRMSRV _service { get; set; }
        public ItemPedidoSRV _itemPedidoSRV { get; set; }
        public PedidoParticipanteSRV _pedidoParticipante { get; set; }
        public ImpostoSRV _impostoSRV { get; set; } //= new ImpostoSRV();
        public InfoFaturaSRV _infoFaturaSRV { get; set; } //= new InfoFaturaSRV();
        public AssinaturaSRV _assinaturaSRV { get; set; } //= new AssinaturaSRV();
        public ContratoSRV _contratoSRV { get; set; } //= new ContratoSRV();
        public ParcelasSRV _parcelasSRV { get; set; } //= new ParcelasSRV();
        public PedidoPagamentoSRV _pedidoPagamento { get; set; } //= new PedidoPagamentoSRV();
        public NFeXmlSRV _nfeXmlSRV { get; set; } //= new NFeXmlSRV();
        public NotaFiscalSRV _nfeSRV { get; set; } //= new NotaFiscalSRV();
        public HistoricoPedidoSRV _serviceHistPedido { get; set; } //= new HistoricoPedidoSRV();
        public NotaFiscalLoteItemSRV _loteItemSRV { get; set; }
        public NotaFiscalSRV _notaFiscalSRV { get; set; }
        public InfoFaturaItemSRV _infoFaturaItemSRV { get; set; }

        public TipoNegociacaoSRV _tipoNegociacaoSRV { get; set; }
        //
        // GET: /franquia/Pedido/

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Index(int? ipeId = null, int? pedCrmId = null)
        {
            ViewBag.ipeId = ipeId;
            ViewBag.pedCrmId = pedCrmId;

            int? uenId = SessionUtil.GetUenId();
            ViewBag.lstUen = new UENSRV().FindAll();

            if (SessionContext.IsGerenteDepartamento("franquiador") || SessionContext.IsGerenteDepartamento("ti"))
            {
                ViewBag.lstRegiao = ServiceFactory.RetornarServico<RegiaoSRV>().FindAllByUen(uenId);
            }
            else if (SessionContext.IsAdmDepartamento("franquiador") || SessionContext.IsAdmDepartamento("TI"))
            {
                ViewBag.lstRegiao = ServiceFactory.RetornarServico<RegiaoSRV>().FindAll();
            }
            return View();
        }

        [Autorizar(Departamento = "TI, franquiado, franquiador, controladoria", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult ListarPedidos(PesquisaPedidoDTO pesquisaDTO,
            int pagina = 1,
            int registrosPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? repreId = SessionContext.GetIdRepresentante();
                if (SessionContext.IsGerenteDepartamentoOR(true, "franquiado", "franquiador", "TI", "Assinatura", "VENDA_ASSI") || SessionUtil.PossuiPermissaoParaFaturar())
                {
                    repreId = pesquisaDTO.REP_ID;
                }

                pesquisaDTO.UEN_ID = SessionUtil.GetUenId();
                pesquisaDTO.REP_ID = repreId;
                                                   
                if (SessionContext.IsDepartamento("CONTROLADORIA"))
                {
                    pesquisaDTO.UEN_ID = null;
                }

                if (SessionContext.IsGerenteDepartamento("franquiado", true))
                {
                    pesquisaDTO.RG_ID = SessionUtil.GetRegiao();
                    repreId = null;
                }

                var lstPedidos = _service.ListarPedidos(pesquisaDTO,
                                                        pagina,
                                                        registrosPorPagina);

                response.AddPage("lstPedidos", lstPedidos);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult ListarPedidosVendaInformada(int? CLI_ID,
            string nomeCliente,
            string cpfCnpjCliente,
            DateTime? dataInicial,
            DateTime? dataFinal,
            int? RG_ID,
            int? UEN_ID,
            bool? vendaInformada = null,
            int pagina = 1,
            int registrosPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var lstPedidos = _service.ListPedidoCRM(REP_ID, CLI_ID, null, pagina, registrosPorPagina);
                    response.AddPage("lstPedidos", lstPedidos);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [Autorizar(PorMenu = false)]
        public ActionResult Emitir(int? CLI_ID = null)
        {
            var tipoPagamento = new TipoPagamentoSRV().FindAll();
            ViewBag.tipoPagamento = tipoPagamento;
            ViewBag.CLI_ID = CLI_ID;
            ViewBag.ExibirQuantidadeDeProdutos = true;
            new ClienteSRV().ChecaClientePodeEmitirPedido(CLI_ID);


            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Detalhes(int? PED_CRM_ID)
        {
            ViewBag.PED_CRM_ID = PED_CRM_ID;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult InformarVendaEfetuada(PedidoCRMDTO obsDTO)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? REP_ID = null;

                    if (AuthUtil.TryGetRepId(out REP_ID))
                    {
                        var login = SessionContext.autenticado.USU_LOGIN;
                        obsDTO.REP_ID = REP_ID;
                        _service.InformarVendaEfetuada(obsDTO, login);

                        SysException.RegistrarLog("Venda efetuada com realizado com sucesso!", "", SessionContext.autenticado);
                        result.success = true;
                    }
                    else
                    {
                        result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                        result.success = false;
                    }
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult GetCombos()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var uenId = SessionUtil.GetUenId();
                var condicaoPagamento = ServiceFactory.RetornarServico<CondicaoPagamentoSRV>().FindAll();
                var tipoPagamento = ServiceFactory.RetornarServico<TipoPagamentoSRV>().ListarTipoPagamentoCompostos();
                var lstRegioes = ServiceFactory.RetornarServico<RegiaoSRV>().ListarRegioesCombo(uenId);
                var lstBandeiraCartao = ServiceFactory.RetornarServico<BandeiraCartaoSRV>().FindAll();
                var lstUF = ServiceFactory.RetornarServico<UFSRV>().BuscarSomenteUF();
                var lstBancos = ServiceFactory.RetornarServico<BancosSRV>().FindAll();

                response.Add("lstCondicaoPagamento", condicaoPagamento);
                response.Add("lstTipoPagamento", tipoPagamento);
                response.Add("lstRegioes", lstRegioes);
                response.Add("lstBandeiraCartao", lstBandeiraCartao);
                response.Add("lstUF", lstUF);
                response.Add("lstBancos", lstBancos);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult SalvarPedido(PedidoCRMDTO pedidoDTO)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? REP_ID = null;

                    if (AuthUtil.TryGetRepId(out REP_ID))
                    {
                        var login = SessionContext.autenticado.USU_LOGIN;
                        pedidoDTO.REP_ID = REP_ID;
                        pedidoDTO.RG_ID = SessionUtil.GetRegiao();
                        pedidoDTO.UEN_ID = SessionUtil.GetUenId();
                        pedidoDTO.USU_LOGIN = login;

                        _service.SalvarPedido(pedidoDTO);

                        SysException.RegistrarLog("Pedido emitido com sucesso!", "", SessionContext.autenticado);
                        result.success = true;
                    }
                    else
                    {
                        result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                        result.success = false;
                    }
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState, false);
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregarDadosDoPedido(int? PED_CRM_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {            
                var pedido = _service.FindByIdFullLoaded(PED_CRM_ID, true, true, true, true);
                response.Add("pedido", pedido);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        //[Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        //public ActionResult CancelarItemPedido(AlteracaoStatusDTO cancelamentoDTO)
        //{
        //    JSONResponse result = new JSONResponse();
        //    try
        //    {
        //        int? REP_ID = null;

        //        if (AuthUtil.TryGetRepId(out REP_ID))
        //        {
        //            string login = SessionContext.login;

        //            cancelamentoDTO.USU_LOGIN = login;
        //            cancelamentoDTO.REP_ID = REP_ID;

        //            _itemPedidoSRV.CancelarItemPedido(cancelamentoDTO);
        //            SysException.RegistrarLog("Status do item pedido alterado para cancelado com sucesso!", "", SessionContext.autenticado);
        //            result.success = true;
        //        }
        //        else
        //        {
        //            result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
        //            result.success = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SessionUtil.HandleException(ex);

        //        result.success = false;
        //        result.message = Message.Fail(ex);
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult CancelarPedido(CancelamentoDTO cancelamentoDTO)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    string login = SessionContext.login;

                    cancelamentoDTO.USU_LOGIN = login;
                    cancelamentoDTO.REP_ID = REP_ID;

                    _service.CancelarPedido(cancelamentoDTO);
                    SysException.RegistrarLog("Status do item pedido alterado para cancelado com sucesso!", "", SessionContext.autenticado);
                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult AlterarStatusItemPedidoParaPagoComPendenciaDeConferencia(AlteracaoStatusDTO status)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    string login = SessionContext.login;
                    status.USU_LOGIN = login;
                    status.REP_ID = REP_ID;

                    _itemPedidoSRV.MarcarItemPedidoComoPagoComPendenciaDeConferencia(status);
                    SysException.RegistrarLog("Status alterado para Pago com Pendência de Conferência com sucesso!", "", SessionContext.autenticado);
                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult AlterarStatusItemPedidoParaPago(AlteracaoStatusDTO status)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    string login = SessionContext.login;
                    status.USU_LOGIN = login;
                    status.REP_ID = REP_ID;

                    _itemPedidoSRV.ConfimarPedidoPagoManualmente(status);
                    SysException.RegistrarLog("Status alterado para Pago com sucesso!", "", SessionContext.autenticado);
                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(GerenteDepartamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult AprovarDescontoNoPedido(AlteracaoStatusDTO status)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    string login = SessionContext.login;
                    status.USU_LOGIN = login;
                    status.REP_ID = REP_ID;

                    _itemPedidoSRV.AprovarDescontoNoPedido(status);
                    string msg = "O gerente {0} aprovou o desconto especial para o item no pedido de código: '{1}'.!";

                    msg = string.Format(msg, login, status.IPE_ID);
                    SysException.RegistrarLog(msg, "", SessionContext.autenticado);
                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarItemPedidoPorPedido(int? PED_CRM_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstItemPedido = _itemPedidoSRV.ListarItemPedidoDoPedido(PED_CRM_ID);
                response.Add("lstItemPedido", lstItemPedido);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListPedidoParticipanteByItemPedido(int? IPE_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstPedidoParticipante = _pedidoParticipante.ListPedidoParticipanteByItemPedido(IPE_ID);
                response.Add("lstPedidoParticipante", lstPedidoParticipante);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult CalcularDescontoDosImpostos(
                ItemPedidoDTO itemPedido,
                int? tipoCliId,
                decimal? valor,
                bool? empresaDoSimples = null,
                bool cemPorCentoFaturado = false)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    int? rgId = SessionUtil.GetRegiao();
                    var infoFatura = _impostoSRV.CalcularDescontosImposto(itemPedido, tipoCliId, rgId, valor, empresaDoSimples, cemPorCentoFaturado);
                    response.Add("infoFatura", infoFatura);
                    response.success = true;
                }
                else
                {
                    response.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    response.success = false;
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ObterInfoFatura(int? iffId)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var infoFatura = _infoFaturaSRV.FindByIdFullLoaded(iffId, true);
                response.Add("infoFatura", infoFatura);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult TesteEmitirPedido()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var pedido = _service.TestarEmissaoDePedido();
                    SysException.RegistrarLog("Pedido emitido com sucesso!", "", SessionContext.autenticado);
                    result.Add("pedido", pedido);

                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }

            }
            catch (ValidacaoException ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                result.SetMessageFromValidacaoException(ex);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        [ActionName("testar-pagamento-parcela")]
        public ActionResult TestePagamentoParcela(string numeroParcela)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var usuario = SessionContext.login;
                    var lstParcelas = numeroParcela.Split(',');
                    _parcelasSRV.PagarVariasParcelasPorCodigo(lstParcelas, null, null, true);
                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }

            }
            catch (ValidacaoException ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                result.SetMessageFromValidacaoException(ex);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult TestePagamentoPedido(int ipeId, bool pagar = false)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var usuario = SessionContext.login;
                    var requisicaoPagamento = _itemPedidoSRV.TestarPagamentoDoPedido(ipeId, REP_ID, usuario, pagar);
                    result.Add("requisicaoPagamento", requisicaoPagamento);

                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }

            }
            catch (ValidacaoException ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                result.SetMessageFromValidacaoException(ex);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [AjaxExceptionFilter]
        [AutorizarCustomAttribute("PossuiPermissaoParaFaturar", IsAjax = true)]
        public ActionResult FaturarPedido(RequisicaoFaturamentoDTO requisicaoFaturamento)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var regiao = SessionUtil.GetRegiaoDTO();
                    var path = HttpContext.Server.MapPath("~/");
                    var usuario = SessionContext.login;
                    var pedido = _service.FaturarPedido(requisicaoFaturamento, path, REP_ID, usuario);
                    SysException.RegistrarLog("Pedido faturado com sucesso!", "", SessionContext.autenticado);

                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }

            }
            catch (ValidacaoException ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.SetMessageFromValidacaoException(ex);
            }
            catch (Exception ex)
            {

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [Autorizar(IsAjax = true)]
        public ActionResult CarregarDadosDaAssinatura(string numeroAssinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var assinatura = _assinaturaSRV.FindByIdFullLoaded(numeroAssinatura, true, true);
                response.Add("assinatura", assinatura);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult CarregarDadosDoContratoGeradosNoPedido(string numeroAssinatura, int? IPE_ID = null, int pagina = 1, int registrosPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstContratos = _contratoSRV.ListarContratos(numeroAssinatura, IPE_ID, null, pagina, registrosPorPagina);
                response.AddPage("lstContratos", lstContratos);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult CarregarDadosDoContrato(string numeroAssinatura, int pagina = 1, int registrosPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstContratos = _contratoSRV.ListarContratos(numeroAssinatura, pagina, registrosPorPagina);
                response.AddPage("lstContratos", lstContratos);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregarDadosDaParcela(string numeroContrato, int pagina = 1, int registrosPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstParcelas = _parcelasSRV.ListarPorContratos(numeroContrato, pagina, registrosPorPagina); 
                response.AddPage("lstParcelas", lstParcelas);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarFormasDePagamento(int? ipeId)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstPedidoPagamento = _pedidoPagamento.ListarPedidoPagamentoPorItem(ipeId);
                response.Add("lstPedidoPagamento", lstPedidoPagamento);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult EnviarEmailPagamento(int? ipeId, string email)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ipeId != null)
                {
                    var usuLogin = SessionContext.login;
                    var repId = SessionContext.GetIdRepresentante();
                    _service.EnviarEmailConfirmaPagamento((int)ipeId, email, usuLogin, repId);

                }
                else
                {
                    throw new PedidoException("Não é possível enviar o email. O código do pedido não foi informado.");
                }
                SysException.RegistrarLog("Email de pagamento enviado com sucesso!", "", SessionContext.autenticado);
                result.success = true;

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        //[Autorizar(PorMenu = false)]
        //public ActionResult AbrirGatewayPagamento(string url, int? IPE_ID)
        //{
        //    JSONResponse result = new JSONResponse();
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(url))
        //        {
        //            throw new Exception("Não é possível achar a url");
        //        }

        //        if (IPE_ID != null)
        //        {
        //            var usuLogin = SessionContext.login;
        //            var repId = SessionContext.GetIdRepresentante();

        //            _service.LiberarParcelaERegistrarHistoricoAbrirGatewayPagamento(IPE_ID, usuLogin, repId);
        //        }
        //        else
        //        {
        //            throw new PedidoException("Não é possível enviar o email. O código do pedido não foi informado.");
        //        }

        //        SysException.RegistrarLog("Email de pagamento enviado com sucesso!", "", SessionContext.autenticado);
        //        result.success = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        SessionUtil.HandleException(ex);

        //        result.success = false;
        //        result.message = Message.Fail(ex);
        //        return Json(result);
        //    }

        //    url = Server.UrlEncode(url);
        //    return Redirect(url);

        //}

        [Autorizar(IsAjax = true)]
        public FileResult DownloadXmlNfe(int? nfxId)
        {
            var serverPath = Server.MapPath("~/");
            var path = _nfeXmlSRV.ChecarERetornarFileName(serverPath, nfxId);
            //byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string[] fileNames = path.Split('\\');

            var filename = fileNames[fileNames.Length - 1];

            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarNFeXmlPorItemPedido(int? IPE_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var requisicaoItensNota = _itemPedidoSRV.ChecarEListarItensNfeDoItemPedido(IPE_ID);
                response.Add("requisicaoItensNota", requisicaoItensNota);

                if (requisicaoItensNota.ExistemNotas)
                {
                    var possuiServico = _nfeXmlSRV.ChecarSeExisteNotaDeServico(requisicaoItensNota.LstNfeXml);
                    response.Add("possuiServico", possuiServico);
                }
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ReceberUploadXmlNfeServico()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                HttpPostedFileBase arquivo = Request.Files[0];
                UploadUtil.ArmazenarArquivoTemporario(arquivo);

                var path = Server.MapPath("~/");
                var chaveNota = _nfeXmlSRV.RetornarChaveDaNFe(arquivo);

                response.Add("chaveDaNota", chaveNota);


            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult SalvarArquivoUploadNFeServico(UploadNotaFiscalDTO uploadNota)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    var path = Server.MapPath("~/");
                    var arquivo = UploadUtil.RetornarArquivoDeUpload();

                    if (arquivo == null)
                    {
                        throw new Exception("Não é possível localizar o arquivo de NFe. Por favor repita o processo.");
                    }

                    _nfeSRV.SalvarNotaDeServico(arquivo, path, uploadNota.IPE_ID, uploadNota.chaveDaNota);
                }
                else
                {
                    response.success = false;
                    response.SetMessageFromModelState(ModelState, false);
                    response.AddValidationMessage("NFeXML", new List<string>() { "Selecione o arquivo xml da nota fiscal" });
                }
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult AlterarObservacoes(AlteracaoStatusDTO status)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                status.REP_ID = SessionContext.GetIdRepresentante();
                status.USU_LOGIN = SessionContext.login;

                _service.AlterarObservacoesItem(status);
                SysException.RegistrarLog("Campo de observação alterado com sucesso!", "", SessionContext.autenticado);
                result.success = true;

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ListarHistoricoDoPedido(int IPE_ID, DateTime? dataInicial = null, DateTime? dataFinal = null,
            int? PST_ID = null,
            int? pagina = 1,
            int? registrosPorPagina = 10,
            int? PPI_ID = null)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var lstHistorico = _serviceHistPedido.ListarHistoricoPorItemPedido(IPE_ID, dataInicial, dataFinal, PST_ID, (int)pagina, (int)registrosPorPagina, PPI_ID);
                result.AddPage("lstHistoricos", lstHistorico);

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [Autorizar(PorMenu = false)]
        public ActionResult Historico(int IPE_ID)
        {
            ViewBag.itemPedidoId = IPE_ID;
            ViewBag.mostraTudo = true;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarHistoricoPedidoCompleto(int IPE_ID, DateTime? dataInicial = null, DateTime? dataFinal = null, int? PST_ID = null)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var lstHistorico = _serviceHistPedido.ListarHistoricoPorItemPedidoSemPaginacao(IPE_ID, dataInicial, dataFinal, PST_ID);
                result.Add("lstHistoricos", lstHistorico);

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult ChecaSeLinkPagamentoPodeSerGerado(int? IPE_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var pedidoPagamento = _itemPedidoSRV.ObterPedidoPagamentoAtual(IPE_ID);
                response.Add("pedidoPagamentoItemPedido", pedidoPagamento);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult EnviarLinkBoletoPorEmail(int? IPE_ID, List<string> lstEmail)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (IPE_ID != null)
                {
                    var usuLogin = SessionContext.login;
                    var repId = SessionContext.GetIdRepresentante();
                    _itemPedidoSRV.EnviarLinkPorEmail(IPE_ID, repId, lstEmail, usuLogin);
                }
                else
                {
                    throw new PedidoException("Não é possível enviar o email. O código do pedido não foi informado.");
                }
                SysException.RegistrarLog("Email de pagamento enviado com sucesso!", "", SessionContext.autenticado);
                result.success = true;

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult RecusarPagamentoDoPedido(AlteracaoStatusDTO status)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    string login = SessionContext.login;
                    status.USU_LOGIN = login;
                    status.REP_ID = REP_ID;

                    _itemPedidoSRV.RecusarPagamentoDoPedido(status);
                    SysException.RegistrarLog("O pedido " + status.IPE_ID + " teve seu pagamento recusado!", "", SessionContext.autenticado);
                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult PrepararParcelaGateway(int? IPE_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    string login = SessionContext.login;

                    _parcelasSRV.PrepararParcelaGateway(IPE_ID);
                    SysException.RegistrarLog("A parcela foi liberada para pagar pelo gateway com sucesso!!", "", SessionContext.autenticado);
                    result.success = true;
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult EmitirPedidoRenovacaoDaMala(string num_assinatura, decimal valor, int qtd_parcela)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var lstUrls = _service.EmitirPedidoRenovacaoDaMala(num_assinatura, valor, qtd_parcela);
                result.Add("lstURLs", lstUrls);

                SysException.RegistrarLog("Pedido de mala gerado com sucesso!", "", SessionContext.autenticado);
                result.success = true;

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult RetornarParcelaParaPagamento(int IPE_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var parcela = _parcelasSRV.ObterProximaParcelaDoPedidoEmAberto(IPE_ID);
                result.Add("parcela", parcela);

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        [Obsolete("Esse processo foi substituído pelo envio direto para o SEFAZ")]
        public ActionResult GerarOuAtualizarNotaFiscal(int? ipeId, int? nfx_id)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var path = HttpContext.Server.MapPath("~/");
                _itemPedidoSRV.GerarOuAtualizarNotaFiscal(ipeId, path, nfx_id);

                SysException.RegistrarLog("Nota Regerado com sucesso!", "", SessionContext.autenticado);
                result.success = true;

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public JsonResult ListarPedidoStatus()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstPedidoStatus = ServiceFactory
                    .RetornarServico<PedidoStatusSRV>()
                    .FindAll();

                response.Add("lstPedidoStatus", lstPedidoStatus);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public JsonResult GerarRequisicaoFaturamento(int? prtId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var requisicaoFaturamento = _service.GerarRequisicaoFaturamento(prtId);
                response.Add("requisicaoFaturamento", requisicaoFaturamento);

            }
            catch (Exception e)
            {
                SessionUtil.HandleException(e);
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarEmpresas()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstEmpresas = ServiceFactory.RetornarServico<EmpresaSRV>().FindAll();
                response.Add("lstEmpresas", lstEmpresas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarRegistrosDeFaturamento(int? ipeId)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstRegistroFaturamento = ServiceFactory.RetornarServico<RegistroFaturamentoSRV>().RetornarRegistroDeFaturamentoPorItemDePedido(ipeId);
                response.Add("lstRegistroFaturamento", lstRegistroFaturamento);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RetornarProvavelSequencialNFEEmpresa(int? empId)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var proximoNumeroDaNota = ServiceFactory.RetornarServico<CustomEmpresaSRV>().RetornarProvavelSequencialNFEEmpresa(empId);
                response.Add("proximoNumeroDaNota", proximoNumeroDaNota);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RetornarPedidosNotaNaoGeradaPorData(DateTime? dataFaturamento, int? empId, int? ipeIdParaExcluir = null, int pagina = 1, int registrosPorPagina = 15)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstPedidosRetroativos = _itemPedidoSRV.RetornarPedidosNotaNaoGeradaPorData(dataFaturamento, empId, ipeIdParaExcluir, pagina, registrosPorPagina);
                response.AddPage("lstPedidosRetroativos", lstPedidosRetroativos);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ObterGruposDeFiltroDoPedido()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var gruposDeFiltro = _service.ObterGruposDeFiltroDoPedido();
                response.Add("gruposDeFiltro", gruposDeFiltro);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarAssinaturaAutocomplete(string assinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstAssinatura = _service.ListarAssinaturaDoPedidoAutoComplete(assinatura);
                response.Add("lstAssinatura", lstAssinatura);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult GerarOuAtualizarVariasNotasFiscais(NotaFiscalBatchDTO notaFiscalBatch)
        {
            JSONResponse result = new JSONResponse();
            try
            {

                var path = HttpContext.Server.MapPath("~/");
                notaFiscalBatch.Path = path;
                var batchResp = _itemPedidoSRV.GerarOuAtualizarVariasNotasFiscais(notaFiscalBatch);

                result.Add("batchResp", batchResp);
                SysException.RegistrarLog("Nota Regerado com sucesso!", "", SessionContext.autenticado);
                result.success = true;

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult DownloadVariasNotas(NotaFiscalBatchDTO notaFiscalBatch)
        {
            JSONResponse result = new JSONResponse();
            try
            {

                var path = HttpContext.Server.MapPath("~/");
                notaFiscalBatch.Path = path;
                var batchResp = _itemPedidoSRV.DownloadVariasNotas(notaFiscalBatch);

                result.Add("batchResp", batchResp);
                SysException.RegistrarLog("Nota Preparada para download!", "", SessionContext.autenticado);
                result.success = true;

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public FileResult DownloadZipXmlNfe(string fileName)
        {
            var serverPath = Server.MapPath("~/");
            var path = _nfeXmlSRV.ChecarERetornarZipFileName(serverPath, fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string[] fileNames = path.Split('\\');

            var filename = fileNames[fileNames.Length - 1];
            System.IO.File.Delete(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public ActionResult GerarDadosIniCancelamento(int? pedCrmId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var dadosCanc = _itemPedidoSRV.GerarDadosIniCancelamento(pedCrmId);
                response.Add("dadosCanc", dadosCanc);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GerarHTMLEmailCanc(CancelamentoDTO cancelamento)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var htmlEmail = _itemPedidoSRV.GerarPreviewHTMLEmailCanc(cancelamento);
                response.Add("htmlEmail", htmlEmail);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult AdicionarVariasNotasAoLoteNFe(NotaFiscalBatchDTO notaFiscalBatch)
        {
            JSONResponse result = new JSONResponse();
            try
            {

                notaFiscalBatch.Path = Server.MapPath("/");
                var batchContext = new BatchContext();
                var lotes = _itemPedidoSRV.AdicionarVariasNotasAoLote(notaFiscalBatch, batchContext);
                
                result.Add("batchResp", batchContext);

                if(lotes != null)
                {
                    result.Add("lotes", lotes);
                    result.Add("codLotes", lotes.Select(x => x.LoteID));
                }

                SysException.RegistrarLog("Envio Agendado!", "", SessionContext.autenticado);
                result.success = true;

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListarItensDeLote(int? ipeID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstLoteItem = _loteItemSRV.ListarItensDoLotePorPedidoItem(ipeID);
                response.Add("lstLoteItem", lstLoteItem);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarItensDeLoteServico(int? ipeID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstLoteItemServico = _loteItemSRV.ListarItensDoLoteServicoPorPedidoItem(ipeID);
                response.Add("lstLoteItemServico", lstLoteItemServico);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarNotasDoPedido(int? ipeID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstNFe = _notaFiscalSRV.RetornarNotasDoPedidoItem(ipeID);
                response.Add("lstNFe", lstNFe);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarNotasServicoDoPedido(int? ipeID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstNFse = _notaFiscalSRV.RetornarNotasServicoDoPedidoItem(ipeID);
                response.Add("lstNFse", lstNFse);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public JsonResult ListarTipoNegociacao()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstTipoNegociacao = _tipoNegociacaoSRV.FindAll();
                response.Add("lstTipoNegociacao", lstTipoNegociacao);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        
        
        [Autorizar(IsAjax = true)]
        public ActionResult ObterInfoFaturaItem(int? iffId)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var infoFaturaItem = _infoFaturaItemSRV.ListarInfoFaturaItemDaInfoFatura(iffId, true);
                response.Add("infoFaturaItem", infoFaturaItem);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }
    }
}
