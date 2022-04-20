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
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect;
using GenericCrud.Service;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Model.Dto.Custons.EnvioEmail;

namespace COADCORP.Areas.Controllers.Cadastros
{
    public class PropostaController : Controller
    {
        public PropostaSRV _propostaSRV { get; set; }
        public PropostaItemSRV _propostaItemSRV { get; set; }
        public TipoPropostaSRV _tipoPropostaSRV { get; set; }
        public HistoricoPedidoSRV _serviceHistPedido { get; set; }
        public PropostaItemComprovanteSRV _propostaItemComprovanteSRV { get; set; }
        public EmpresaSRV _empresaSRV { get; set; }
        public UENSRV _uenSRV { get; set; }
        public ImpostoSRV _impostoSRV { get; set; }
        public InfoFaturaSRV _infoFaturaSRV { get; set; }
        public InfoFaturaItemSRV _infoFaturaItemSRV { get; set; }
        public RegiaoSRV _regiaoSRV { get; set; }
        public ClienteSRV _clienteSRV { get; set; }
        public ProdutoComposicaoSRV _produtoComposicaoSRV { get; set; }
        public ParcelasSRV _parcelaSRV { get; set; }
        public NotaFiscalLoteItemSRV _loteItemSRV { get; set; }
        public NotaFiscalSRV _notaFiscalSRV { get; set; }
        public TipoNegociacaoSRV _tipoNegociacaoSRV { get; set; }

        [Autorizar(Departamento = "TI, franquiado, franquiador, assinatura, VENDA_ASSI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Index(int? ppiId = null, int? prtId = null)
        {
            ViewBag.ppiId = ppiId;
            ViewBag.prtId = prtId;

            int? uenId = SessionUtil.GetUenId();
            ViewBag.lstUen = _uenSRV.FindAll();

            if (SessionContext.IsGerenteDepartamentoOR(false, "TI", "Franquiador", "Assinatura", "VENDA_ASSI")) // se ele for gerente lista as regiões da uen da qual ele pertence.
            {
                ViewBag.lstRegiao = _regiaoSRV.FindAllByUen(uenId);
            }
            else if (SessionContext.IsAdmDepartamentoOR("franquiador", "TI", "Assinatura", "VENDA_ASSI")) // se for uma administrador de departamento vê todas as regiões
            {
                ViewBag.lstRegiao = _regiaoSRV.FindAll();
            }
            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Detalhes(int? prtId)
        {
            ViewBag.prtId = prtId;
            return View();
        }


        [Autorizar(PorMenu = false)]
        public ActionResult Emitir()
        {
            var empId = SessionUtil.GetEmpIdDoRepresentante();
            var repLogado = SessionContext.GetIdRepresentante();
            var ehGerente = SessionUtil.FranquiadoOuGerenteOuTI();

            if (!ehGerente)
            {
                ViewBag.repId = repLogado;
            }
            ViewBag.empId = empId;
            ViewBag.ehGerente = (ehGerente) ? 1 : 0;
            return View();
        }
        //

        [Autorizar(PorMenu = false)]
        public ActionResult Editar(int? prtId)
        {
            var empId = SessionUtil.GetEmpIdDoRepresentante();
            var repLogado = SessionContext.GetIdRepresentante();
            var ehGerente = SessionUtil.FranquiadoOuGerenteOuTI();

            if (!ehGerente)
            {
                ViewBag.repId = repLogado;
            }
            ViewBag.empId = empId;
            ViewBag.ehGerente = (ehGerente) ? 1 : 0;
            ViewBag.prtId = prtId;
            return View("Emitir");
        }

        [Autorizar(IsAjax = true)]
        public JsonResult ListarPropostas(PesquisaPropostaDTO pesquisaPropostaDTO)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                int? repreId = SessionContext.GetIdRepresentante();
                if (SessionContext.IsGerenteDepartamentoOR(true, "franquiado", "franquiador", "TI", "Assinatura", "VENDA_ASSI") || SessionUtil.PossuiPermissaoParaFaturar())
                {
                    repreId = pesquisaPropostaDTO.REP_ID;
                }
                pesquisaPropostaDTO.UEN_ID = SessionUtil.GetUenId();
                pesquisaPropostaDTO.REP_ID = repreId;

                if (SessionContext.IsDepartamento("CONTROLADORIA"))
                {
                    pesquisaPropostaDTO.UEN_ID = null;
                }

                if (SessionContext.IsGerenteDepartamento("franquiado", true))
                {
                    pesquisaPropostaDTO.RG_ID = SessionUtil.GetRegiao();
                }

                var lstPropostas = _propostaSRV.ListarPropostas(pesquisaPropostaDTO);
                response.AddPage("lstPropostas", lstPropostas);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public JsonResult ListarTipoProposta()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstTipoProposta = _tipoPropostaSRV.FindAll();
                response.Add("lstTipoProposta", lstTipoProposta);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
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
        public JsonResult ListarPropostaItem(int? prtId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstPropostaItem = _propostaItemSRV.ListarPropostaItemPorProposta(prtId);
                response.Add("lstPropostaItem", lstPropostaItem);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDaProposta(int? prtId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var proposta = _propostaSRV.FindByIdFullLoaded(prtId, true, true, true, true);
                response.Add("proposta", proposta);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult SalvarProposta(PropostaDTO proposta)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                //var asdf = ModelState.Values.Where(v => v.Errors.Count > 0);
                if (ModelState.IsValid)
                {
                    int? repId = SessionContext.GetIdRepresentante();
                    int? uenId = SessionUtil.GetUenId();
                    string usuario = SessionContext.login;
                    int? rgId = SessionUtil.GetRegiao();

                    if (rgId == null)
                    {
                        var nomeRepresentante = SessionUtil.GetNomeRepresentante();
                        throw new PedidoException(string.Format("Não é possível gerar proposta. O representante logado {0} não possui nenhuma região", nomeRepresentante));
                    }

                    var propostaSalvaResult = _propostaSRV.SalvarProposta(proposta, repId, usuario, rgId, uenId);
                    result.Add("propostaSalvaResult", propostaSalvaResult);

                    if (propostaSalvaResult != null && 
                        propostaSalvaResult.EhValido && 
                        propostaSalvaResult.PropostaSalva != null &&  
                        propostaSalvaResult.PropostaSalva.PRT_ID != null)
                    {
                        result.Add("prtId", propostaSalvaResult.PropostaSalva.PRT_ID);
                        result.message = Message.Info("Dados do prospects atualizados com sucesso!!");
                        SysException.RegistrarLog("Dados do cliente atualizados com sucesso!!", "", SessionContext.autenticado);
                    }

                    result.success = true;

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ListarHistoricoDaPropostaItem(int PPI_ID, DateTime? dataInicial = null, DateTime? dataFinal = null,
            int? PST_ID = null,
            int? pagina = 1,
            int? registrosPorPagina = 10)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var lstHistorico = _serviceHistPedido.ListarHistoricoPorItemPedido(null, dataInicial, dataFinal, PST_ID, (int)pagina, (int)registrosPorPagina, PPI_ID);
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
        
        [Autorizar(IsAjax = true)]
        public ActionResult EnviarLinkBoletoPorEmail(int? PPI_ID, string email)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (PPI_ID != null)
                {
                    var usuLogin = SessionContext.login;
                    var repId = SessionContext.GetIdRepresentante();
                    //_propostaItemSRV.EnviarLinkPorEmail(PPI_ID, repId, email, usuLogin);
                }
                else
                {
                    throw new PedidoException("Não é possível enviar o email. O código da proposta não foi informado.");
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

        [Autorizar(IsAjax = true)]
        [Funcionalidade(1, "Envio de Boleto Primeira parcela", Order = 2)]
        public ActionResult EnviarBoletoPorEmail(
            EnvioEmailDTO emailEnvioDTO)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (emailEnvioDTO.PPI_ID != null)
                {
                    emailEnvioDTO.USU_LOGIN = SessionContext.login;
                    emailEnvioDTO.REP_ID = SessionContext.GetIdRepresentante();
                    _propostaItemSRV.EnviarBoletoPorEmail(emailEnvioDTO);
                }
                else
                {
                    throw new PedidoException("Não é possível enviar o email. O código da proposta não foi informado.");
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

        [Autorizar(IsAjax = true)]
        [Funcionalidade(2, "Envio de Dados de Nogociações", Order = 2)]
        public ActionResult EnviarResumoDaProposta(
            EnvioEmailDTO envioEmail)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (envioEmail != null && envioEmail.PPI_ID != null)
                {
                    envioEmail.USU_LOGIN = SessionContext.login;
                    envioEmail.REP_ID = SessionContext.GetIdRepresentante();
                    _propostaItemSRV.EnviarResumoDaProposta(envioEmail);
                }
                else
                {
                    throw new PedidoException("Não é possível enviar o email. O código da proposta não foi informado.");
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


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult MarcarPropostaComoPaga(int? PPI_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;

                _propostaItemSRV.MarcarManualmentePropostaComoPaga(PPI_ID, repId, usuario);
                SysException.RegistrarLog("Proposta marcada manualmente como paga!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Proposta marcada manualmente como paga!!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult InformarPedidoPagoComPendenciaDeConferencia(AlteracaoStatusDTO alteracaoStatus)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;

                _propostaItemSRV.InformarPedidoPagoComPendenciaDeConferencia(alteracaoStatus.PPI_ID, usuario, repId, alteracaoStatus.OBSERVACOES);
                SysException.RegistrarLog("Proposta marcada manualmente como paga!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Proposta marcada manualmente como paga!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [Autorizar(Departamento = "TI, franquiado, franquiador, VENDA_ASSI", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
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

                    _propostaItemSRV.RecusarPagamentoDoPedido(status);
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


        [AutorizarCustomAttribute("PossuiPermissaoParaFaturar", IsAjax = true)]
        public ActionResult EmitirPedidoDaProposta(int? prtId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;

                _propostaSRV.EmitirPedidoDaProposta(prtId, usuario, repId);
                SysException.RegistrarLog("Pedido gerado com successo!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Pedido gerado com successo!!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ListarEmpresas()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstEmpresas = _empresaSRV.FindAll();
                response.Add("lstEmpresas", lstEmpresas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public JsonResult ChecarERetornarDadosDeCliente(int? clienteId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                bool podeEditar = false;
                int? repId = SessionUtil.FranquiadoOuGerenteOuTI() ? null : SessionContext.GetIdRepresentante();

                var cliente = _clienteSRV.ChecarERetornarDadosDeCliente(clienteId, repId, true, true, true);
                response.Add("cliente", cliente);


                int? REP_ID = null;
                int? uenId = SessionUtil.GetUenId();
                if (AuthUtil.TryGetRepId(out REP_ID))
                {

                    if (cliente != null && cliente.ClienteExisteNaAgenda)
                    {
                        if (_clienteSRV.RepresentantePodeEditarCliente(clienteId, REP_ID, uenId))
                        {
                            podeEditar = true;
                        }
                    }
                }

                response.Add("podeEditar", podeEditar);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(SessionUtil.RecursiveShowExceptionsMessage(e));
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(Departamento = "TI, franquiado, franquiador, VENDA_ASSI", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult CancelarPropostaItem(AlteracaoStatusDTO cancelamentoDTO)
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

                    _propostaItemSRV.CancelarPropostaItem(cancelamentoDTO);
                    SysException.RegistrarLog("Status da proposta alterado para cancelado com sucesso!", "", SessionContext.autenticado);
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
        public ActionResult RetornarProdutoRenovacao(string codAssinatura, int? empId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var infoProdutoRenovacao = _produtoComposicaoSRV.RetornarDadosDeRenovacaoDoProdutoComposicao(codAssinatura, empId);
                result.Add("infoProdutoRenovacao", infoProdutoRenovacao);

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
        public ActionResult RetornarPeriodoFaturamento()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var lstDatasFaturamento = ServiceFactory.RetornarServico<DatasFaturamentoSRV>().ListarDataFaturamentoUltimos2Meses();
                result.Add("lstDatasFaturamento", lstDatasFaturamento);

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
        public ActionResult ChecarSeAhParcelaPaga(int ppiId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var existeParcelaPaga = _parcelaSRV.ExisteParcelasPagas(null, ppiId);
                result.Add("existeParcelaPaga", existeParcelaPaga);

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AutorizarCustom("PossuiPermissaoParaFaturar", IsAjax = true)]
        public ActionResult ForcarBaixaAutomatica(int? prtId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;

                _propostaItemSRV.ForcarBaixaAutomatica(prtId, repId, usuario);
                SysException.RegistrarLog("Baixa forçada com com successo!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("A baixa foi bem sucedida!!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ListarAssinaturaAutocomplete(string assinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstAssinatura = _propostaSRV.ListarAssinaturaDaPropostaAutoComplete(assinatura);
                response.Add("lstAssinatura", lstAssinatura);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ObterGruposDeFiltroDaProposta()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var gruposDeFiltro = _propostaSRV.ObterGruposDeFiltroDoPedido();
                response.Add("gruposDeFiltro", gruposDeFiltro);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public JsonResult ListarPropostaItemDeBoleto(int? prtId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstPropostaItemBoleto = _propostaItemSRV.ListarPropostaItemDeBoleto(prtId);
                response.Add("lstPropostaItemBoleto", lstPropostaItemBoleto);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public JsonResult RetornarCodPedidoDaProposta(int? prtId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var pedCrmId = _propostaSRV.RetornarCodPedidoDaProposta(prtId);
                response.Add("pedCrmId", pedCrmId);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListarItensDeLote(int? ppiID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstLoteItem = _loteItemSRV.ListarItensDoLotePorPropostaItem(ppiID);
                response.Add("lstLoteItem", lstLoteItem);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarItensDeLoteServico(int? ppiID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstLoteItemServico = _loteItemSRV.ListarItensDoLoteServicoPorPropostaItem(ppiID);
                response.Add("lstLoteItemServico", lstLoteItemServico);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
               

        public ActionResult ListarNotasDaProposta(int? ppiID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstNFe = _notaFiscalSRV.RetornarNotasDaPropostaItem(ppiID);
                response.Add("lstNFe", lstNFe);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult AdicionarVariasNotasAoLoteNFe(int? ppiID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;

                _propostaItemSRV.AdicionarVariasNotasAntecipacaoAoLoteNFe(ppiID, repId);
                SysException.RegistrarLog("A nota foi adicionada ao lote com sucesso!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("A nota foi adicionada ao lote com sucesso!!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [Autorizar(IsAjax = true)]
        public ActionResult PesquisarPorPropostaItem(int? PPI_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstPedidoParticipante = ServiceFactory.RetornarServico<PedidoParticipanteSRV>().PesquisarPorPropostaItem(PPI_ID);
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
                PropostaItemDTO propostaItem,
                PropostaDTO proposta)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var resultadoCalc = _impostoSRV.CalcularDescontos(propostaItem, proposta);
                    response.Add("resultadoCalc", resultadoCalc);
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
        [HttpPost]
        public ActionResult InformarAceiteVendaAPrazo(int? PPI_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;

                _propostaItemSRV.InformarAceiteVendaAPrazo(PPI_ID, repId, usuario);
                SysException.RegistrarLog("Proposta marcada manualmente como paga!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Proposta marcada manualmente como paga!!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ListarNotasServicoDaProposta(int? ppiID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstNFse = _notaFiscalSRV.RetornarNotasServicoDaPropostaItem(ppiID);
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
