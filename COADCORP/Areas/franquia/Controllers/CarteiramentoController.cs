using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Transferencia;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericCrud.Service;

namespace COADCORP.Areas.franquia.Controllers
{
    public class CarteiramentoController : Controller
    {
        //private const int uenId = 1;
        private CarteiramentoSRV _service = new CarteiramentoSRV();
        private EmpresaSRV _empresaSRV = new EmpresaSRV();
        private RegiaoSRV _regiaoService = new RegiaoSRV();

        private UFSRV _ufService = new UFSRV();
        private ClienteSRV _clienteSRV = new ClienteSRV();
        private RepresentanteSRV _representanteSRV = new RepresentanteSRV();

        private void _carregaCombos()
        {
            var uenId = SessionUtil.GetUenId();
            ViewBag.Ufs = _regiaoService.ListarRegioes(uenId);
            ViewBag.UfsRepresentante = _ufService.GetUfsNoRepresentante();
        }

        [Autorizar(GerenteDepartamento = "Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Index()
        {
            _carregaCombos();
           return View();
        }

        [Autorizar(GerenteDepartamento = "Franquiado, franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Gerenciar()
        {
            _carregaCombos();
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult Carteiramentos(string siglaRegiao = null, string representante = null, 
            int areaId = 1, 
            int? RG_ID = null, 
            int? REP_TO_IGNORE = null, 
            int pagina = 1, 
            int numeroPagina = 7)
        {
            int? uenId = SessionUtil.GetUenId();
            var carteiramentos = _service.Carteiramentos(siglaRegiao, representante, areaId, pagina, numeroPagina, RG_ID, UEN_ID: uenId);
            
            JSONResponse result = new JSONResponse();
            result.AddPage("carteiramentos", carteiramentos);

            return Json(result);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarteiramentosFranquia(string siglaRegiao = null, string representante = null,
            int areaId = 1,
            int? RG_ID = null,
            int? REP_TO_IGNORE = null,
            int pagina = 1,
            int numeroPagina = 7)
        {
            int? uenId = SessionUtil.GetUenId();
            var carteiramentos = _service.Carteiramentos(siglaRegiao, representante, areaId, pagina, numeroPagina, RG_ID, UEN_ID: uenId);

            JSONResponse result = new JSONResponse();
            result.AddPage("carteiramentos", carteiramentos);

            return Json(result);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult TrocarRepresentante(string carteiraId = null, int? representanteId = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                //_service.TrocarRepresentante(carteiraId, (int) representanteId);
                response.success = true;
                return Json(response);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar]
        public ActionResult Config(string carteiraId)
        {
            _carregaCombos();
            ViewBag.empresas = _empresaSRV.ListarEmpresasParaPrePedido();
            ViewBag.carteiraId = carteiraId;
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReadCarteiramento(string carteiraId)
        {
            var carteira = _service.FindByIdFullLoaded(carteiraId);
            JSONResponse response = new JSONResponse();
            response.Add("carteira", carteira);

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarteiramentosDaRepresentante()
        {
            JSONResponse response = new JSONResponse();

            if (SessionContext.PossuiRepresentante())
            {
                var REP_ID = (int) SessionContext.GetIdRepresentante();
                var carteiramentos = _service.GetCarteirasDoRepresentante(REP_ID);
                response.Add("carteiramentos", carteiramentos);


            }
            else
            {
                response.success = false;
                response.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
            }

            return Json(response);

        }
           
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult SalvarConfig(CarteiraDTO carteira)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarConfig(carteira);
                    return Json(result);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result);
            }
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarCarteiramentoParaEscolha(int? REP_ID, int pagina = 1, int registrosPorPagina = 5)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                int? uenId = SessionUtil.GetUenId();
                var carteiramentos = _service.ListarCarteiramemtoParaEscolha(REP_ID,uenId , pagina, registrosPorPagina);
                response.AddPage("carteiramentos", carteiramentos);
            }
            catch (Exception e)
            {
                SessionUtil.HandleException(e);
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult TrocarCarteiramento(int REP_ID, string CAR_ID_ANTIGO, string CAR_ID_NOVO)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.TrocarCarteiramento(REP_ID, CAR_ID_ANTIGO, CAR_ID_NOVO);
                SysException.RegistrarLog("Carteira trocada com sucesso!!", "", SessionContext.autenticado);
                result.success = true;
                result.message = Message.Info("Carteira trocada com sucesso!!");

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
        public ActionResult AdicionarCarteiramento(int REP_ID, string CAR_ID_NOVO)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.AdicionarCarteiramento(REP_ID, CAR_ID_NOVO);
                SysException.RegistrarLog("Carteira adicionada com sucesso!!", "", SessionContext.autenticado);
                result.success = true;
                result.message = Message.Info("Carteira adicionada com sucesso!!");

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
        public ActionResult RemoverCarteiramento(int REP_ID, string CAR_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.RemoverCarteiraDoRepresentante(CAR_ID, REP_ID);
               result.success = true;               

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

        [Autorizar(GerenteDepartamento = "Franquiado, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Transferencia()
        {
            int? REP_ID = null;
            ViewBag.franquiador = 0;
            if (AuthUtil.TryGetRepId(out REP_ID))
            {
                _carregaCombos();
                RepresentanteDTO representante = _representanteSRV.FindById(REP_ID);
                ViewBag.RG_ID = representante.RG_ID;
            }
            
            return View();
        }

        [Autorizar(GerenteDepartamento = "Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult TransferenciaFranquiador()
        {
            _carregaCombos();
            ViewBag.RG_ID = SessionUtil.GetRegiao();
            ViewBag.franquiador = 1;
            return View("Transferencia");
        }

        [Autorizar(Departamento = "Franquiado", IsAjax = true)]
        public ActionResult TransferirSuspects(TransferirSuspectDTO dto)
        { 
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    if (SessionContext.PossuiRepresentante())
                    {
                        if (dto != null && dto.RG_ID == null && !string.IsNullOrWhiteSpace(dto.Tipo) && dto.Tipo != "representante")
                        {
                            int? RG_ID = SessionUtil.GetRegiao();
                            dto.RG_ID = RG_ID;
                        }

                        var resp = _clienteSRV.TransferirSuspects(dto);

                        result.success = true;
                        result.message = Message.Success("Suspects transferidos com sucesso");

                        if (resp != null && resp.Count() > 0)
                        {
                            result.Add("resumo", resp);
                        }
                    }
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result);
                }                
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result);
        }

        [Autorizar(Departamento = "Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult GerarCarteira(int RG_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? uenId = SessionUtil.GetUenId();
                int? ID = _service.GerarCarteiraFranquia(uenId, RG_ID);

                result.success = true;
                Message message = Message.Success("Carteira gerada com sucesso. Código: '{0}'");
                message.message = string.Format(message.message, ID);
                result.message = message;
                
           }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result);
        }

        [Autorizar(Departamento = "Franquiado, TI", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult ExcluirCarteira(CarteiraDTO carteira)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.ExcluirCarteira(carteira);
                result.success = true;
                result.message = Message.Success("Carteira excluída com sucesso.");
                
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result);
        }

        [Autorizar(Departamento = "Franquiado, Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult ReencarteirarPorRodizio(int? CLI_ID, int? REP_ID, int? REP_ANTERIOR, int? RG_ID = null)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID_LOGADO = null;
                if (AuthUtil.TryGetRepId(out REP_ID_LOGADO))
                {
                    REP_ID_LOGADO = SessionContext.GetIdRepresentante();
                    string nomeUsuario = SessionContext.login;
                                        
                    if(!SessionContext.IsGerenteDepartamento("franquiador"))
                    {
                        RG_ID = SessionUtil.GetRegiao();
                    }
                    else if (RG_ID == null)
                    {
                        throw new Exception("Selecione uma região.");
                    }

                    _service.ReencarteiraPorRodizio(CLI_ID, RG_ID, REP_ID_LOGADO, nomeUsuario);
                    result.success = true;
                    result.message = Message.Success("Suspects transferidos com sucesso");
                }                
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result);
        }

        [Autorizar(Departamento = "Franquiado, Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult ReencarteirarParaRepresentante(int? CLI_ID, int? REP_ID, int? RG_ID = null)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID_LOGADO = null;
                if (AuthUtil.TryGetRepId(out REP_ID_LOGADO))
                {
                    REP_ID_LOGADO = SessionContext.GetIdRepresentante();
                    string nomeUsuario = SessionContext.login;

                    if (!SessionContext.IsGerenteDepartamento("franquiador"))
                    {
                        RG_ID = SessionUtil.GetRegiao();
                    }
                    else if(RG_ID == null)
                    {
                        throw new Exception("Selecione uma região.");
                    }

                    _service.ReencarteiraParaRepresentante(CLI_ID, REP_ID, REP_ID_LOGADO, RG_ID, nomeUsuario);
                    result.success = true;
                    result.message = Message.Success("Cliente reencarteirado com sucesso !!!");


                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result);
        }

        [Autorizar(Departamento = "Franquiado, Franquiador", IsAjax = true)]
        public ActionResult AdicionarRegiao(int? CLI_ID, int? RG_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID_LOGADO = null;
                if (AuthUtil.TryGetRepId(out REP_ID_LOGADO))
                {
                    REP_ID_LOGADO = SessionContext.GetIdRepresentante();
                    string nomeUsuario = SessionContext.login;
                    _service.AdicionarRegiao(CLI_ID, RG_ID, REP_ID_LOGADO, nomeUsuario);
                    result.success = true;
                    result.message = Message.Success("Cliente adicionado a região com sucesso !!!");


                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result);
        }


        [Autorizar(Departamento = "Franquiado, Franquiador", IsAjax = true)]
        public ActionResult RemoverRegiao(int? CLI_ID, int? RG_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID_LOGADO = null;
                if (AuthUtil.TryGetRepId(out REP_ID_LOGADO))
                {
                    REP_ID_LOGADO = SessionContext.GetIdRepresentante();
                    string nomeUsuario = SessionContext.login;
                    _service.RemoverRegiao(CLI_ID, RG_ID, REP_ID_LOGADO, nomeUsuario);
                    result.success = true;
                    result.message = Message.Success("Cliente removido a região com sucesso !");


                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult QtdClienteCarteiramento(string CAR_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var qtdClientes = _service.QtdClienteCarteiramento(CAR_ID);
                response.Add("qtdClientes", qtdClientes);
            }
            catch (Exception e)
            {
                SessionUtil.HandleException(e);
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, GerenteDepartamento = "Franquiador, Franquiado, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Representantes(string nome = null, int? RG_ID_REPRESENTANTE = null, int pagina = 1, int registroPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    if (RG_ID_REPRESENTANTE == null && SessionContext.IsGerenteDepartamento("Franquiado", true))
                    {
                        RG_ID_REPRESENTANTE = SessionUtil.GetRegiao();
                    }

                    var uenId = SessionUtil.GetUenId();
                    var representantes = _representanteSRV.Representantes(nome, RG_ID_REPRESENTANTE, uenId, false, pagina, registroPorPagina, nivelRepresentanteId: 4);
                    response.AddPage("representantes", representantes);
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

        [Autorizar(GerenteDepartamento = "franquiado, TI", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult ClientesPorRepresentante(int? REP_ID, string cnpj = null, string nome = null, int? classificacaoClienteId = null, string CAR_ID = null, int? RG_ID = null, int pagina = 1, int registroPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (REP_ID != null)
                {
                    var uenId = SessionUtil.GetUenId();
                    var lstClientes = _clienteSRV.Clientes(cnpj, nome, pagina, registroPorPagina, REP_ID, uenId, classificacaoClienteId, CAR_ID, RG_ID);
                    response.AddPage("clientes", lstClientes);
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

        [Autorizar(IsAjax = true)]
        public ActionResult CarteiramentosDaRepresentanteParaTransferencia(int REP_ID)
        {
            JSONResponse response = new JSONResponse();

            if (SessionContext.PossuiRepresentante())
            {
                var carteiramentos = _service.GetCarteirasDoRepresentante(REP_ID);
                response.Add("carteiramentos", carteiramentos);
            }
            else
            {
                response.success = false;
                response.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult TransferirClientes()
        {
            return View();
        }

        public ActionResult RepresentantesLogados()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    int? RG_ID = SessionUtil.GetRegiao();
                    var representantes = _representanteSRV.GetRepresentantesLogados(DateTime.Now, RG_ID, null);
                    response.Add("representantes", representantes);
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

        public ActionResult QuantidadeClientesPorTipo(int? REP_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? GERENTE_REP_ID = null;
                if (REP_ID != null && AuthUtil.TryGetRepId(out GERENTE_REP_ID))
                {
                    var uenId = SessionUtil.GetUenId();
                    var quantidadeTipoCliente = _clienteSRV.QtdClientesRepresentanteGerente(GERENTE_REP_ID, REP_ID, uenId);
                    response.Add("resumoQuantidadeTipoCliente", quantidadeTipoCliente);
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RepresentantesLogadosTransferencia(int? RG_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    if (RG_ID == null && SessionContext.IsGerenteDepartamento("FRANQUEIDO"))
                    {
                        RG_ID = SessionUtil.GetRegiao();
                    }
                    
                    var representantes = _representanteSRV.GetRepresentantesLogados(DateTime.Now, RG_ID, null);
                    response.Add("representantes", representantes);
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

       public ActionResult BuscarCarteiras(
            string CAR_ID = null,
            int? rgId = null,
            int? uenId = null,
            int? repId = null,
            int pagina = 1,
            int registosPorPagina = 7,
            bool associadoARepresentante = false)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstCarteiras = _service.BuscarCarteiras(CAR_ID, rgId, uenId, repId, pagina, registosPorPagina, associadoARepresentante);
                response.AddPage("lstCarteiras", lstCarteiras);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChecarCarteiraExiste(string carId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var valida = ServiceFactory
                    .RetornarServico<CarteiramentoSRV>()
                    .ChecarCarteiraExiste(carId);
                response.Add("valida", valida);

            }
            catch (Exception e)
            {
                response.Add("valida", false);
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarCarteirasDoRepreECliente(int? cliId, int? repId)
        {
            JSONResponse response = new JSONResponse();

            if (SessionContext.PossuiRepresentante())
            {
                var REP_ID = (int)SessionContext.GetIdRepresentante();
                var lstCarteiramento = _service.ListaCarteiraByCliIdAndRepId(cliId, repId);
                response.Add("lstCarteiramento", lstCarteiramento);

            }
            else
            {
                response.success = false;
                response.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
            }

            return Json(response);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarCarteirasDoRepreClienteEAssi(int? cliId, int? repId)
        {
            JSONResponse response = new JSONResponse();

            if (SessionContext.PossuiRepresentante())
            {
                var REP_ID = (int)SessionContext.GetIdRepresentante();
                var lstCarteiramento = _service.ListaCarteirasDoClienteERepresentante(cliId, repId);
                response.Add("lstCarteiramento", lstCarteiramento);

            }
            else
            {
                response.success = false;
                response.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
            }

            return Json(response);

        }


    }


}
