using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Exceptions;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Service;
using GenericCrud.Controllers;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto.FiltersInfo;

namespace COADCORP.Areas.franquia.Controllers
{
    [Autorizar(Departamento = "Franquiado, Franquiador, TI, VENDA_ASSI", PorMenu = false, PermitirNiveisPrivilegiosSuperiores = true)]
    public class RepresentanteController : GenericController<REPRESENTANTE, RepresentanteDTO, int>
    {
        public RepresentanteController() : base()
        {

        }

        public RepresentanteController(RepresentanteSRV _service) : base(_service)
        {
            this._service = _service;
        }

        private RepresentanteSRV _service { get; set; } //= new RepresentanteSRV();
        public RegiaoSRV _regiaoSRV { get; set; } //= new RegiaoSRV();
        public PrioridadeAtendimentoSRV _prioridadeAtendimentoSRV { get; set; } //= new PrioridadeAtendimentoSRV();
        public ClienteSRV _clienteSRV { get; set; } //= new ClienteSRV();
        public AgendamentoSRV _agendamentoSRV { get; set; } //= new AgendamentoSRV();
        public FilaCadastroSRV _filaService { get; set; } //= new FilaCadastroSRV();
        public NivelRepresentanteSRV _nivelRepresentanteService { get; set; } //= new NivelRepresentanteSRV();
        public EmpresaSRV _empresaSRV { get; set; }
        //
        // GET: /franquia/Representante/

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI, VENDA_ASSI", PorMenu = false, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Listar()
        {

            int? nivelAcesso = SessionUtil.GetNivelAcessoPerfil();
            ViewBag.lstNivelRepresentante = _nivelRepresentanteService.ListarNivelRepresentante(nivelAcesso);
            ViewBag.lstsup = new RepresentanteSRV().BuscarSupervisores();
            ViewBag.lstUen = new UENSRV().FindAll();
            
            return View();
        }

        public ActionResult Home()
        {
            string _home_perfil = "";//SessionContext.perfis_usuario.Where(x => x.perId == SessionContext.per_id).FirstOrDefault().PERFIL.PER_PATH_HOME;

            if (_home_perfil != null && _home_perfil != "")
                return Redirect(_home_perfil);
            else
                return View();
        }

        public ActionResult ListarDadosHome(int pagina = 1, int registrosPorPagina = 5, int? REP_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (REP_ID != null || AuthUtil.TryGetRepId(out REP_ID))
                {
                    var uenId = SessionUtil.GetUenId();
                    var rgId = SessionUtil.GetRegiao();
                    var lstPrioridades = _prioridadeAtendimentoSRV.GetPrioridadesByRepresentante(REP_ID, pagina, registrosPorPagina, rgId);
                    var quantidadeTipoCliente = _clienteSRV.QtdClientesRepresentante(REP_ID, uenId);

                    response.AddPage("lstPrioridades", lstPrioridades);
                    response.Add("quantidadeTipoCliente", quantidadeTipoCliente);
                }               
                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
           
        }

        public ActionResult QuantidadeClientesPorTipo()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var uenId = SessionUtil.GetUenId();
                    var quantidadeTipoCliente = _clienteSRV.QtdClientesRepresentante(REP_ID, uenId);
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

        public ActionResult ListarAgendamentoDoDia(DateTime? data = null, int pagina = 1, int registrosPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    if (data == null)
                    {
                        data = DateTime.Now;
                    }

                    int? rgId = SessionUtil.GetRegiao();
                    var lstAgendamentoDoDia = _agendamentoSRV.ListarAgendamentosDoDia(data, REP_ID, pagina, registrosPorPagina, rgId);
                    response.AddPage("lstAgendamentoDoDia", lstAgendamentoDoDia);
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarAgendamentoAtrasado(DateTime? data = null, int pagina = 1, int registrosPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    if (data == null)
                    {
                        data = DateTime.Now;
                    }
                    
                    int? rgId = SessionUtil.GetRegiao();
                    var lstAgendamentoAtrasado = _agendamentoSRV.ListarAgendamentosAtrasados(data, REP_ID, pagina, registrosPorPagina, rgId);
                    response.AddPage("lstAgendamentoAtrasado", lstAgendamentoAtrasado);
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarAgendamentoVindouro(DateTime? data = null, int pagina = 1, int registrosPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    if (data == null)
                    {
                        data = DateTime.Now;
                    }

                    int? rgId = SessionUtil.GetRegiao();
                    var lstAgendamentoVindouro = _agendamentoSRV.ListarAgendamentosVindouros(data, REP_ID, pagina, registrosPorPagina, rgId);
                    response.AddPage("lstAgendamentoVindouro", lstAgendamentoVindouro);
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Representantes(string nome = null, string descricaoUf = null, bool? trazUsuario = null, int pagina = 1, int registroPorPagina = 5)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                int? uenId = SessionUtil.GetUenId();
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var representantes = _service.RepresentantesPorRegiaoDoRepresentante(nome, REP_ID, pagina, registroPorPagina, uenId, nivelRepresentanteId: 4);
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

        [Autorizar(GerenteDepartamento = "FRANQUIADO, FRANQUIADOR, TI, VENDA_ASSI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult RepresentantesComUsuario(string nome = null, int pagina = 1, int registroPorPagina = 5, int? UEN_ID = null, int? NRP_ID = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                    if (!SessionContext.IsAdmDepartamento("franquiador") && !SessionContext.IsAdmDepartamento("TI"))
                    {
                        UEN_ID = SessionUtil.GetUenId();
                    }

                    int? nivelAcesso = SessionUtil.GetNivelAcessoPerfil();
                    int? RG_ID = (SessionContext.IsGerenteDepartamento("FRANQUIADO")) ? SessionUtil.GetRegiao() : null;
                    var representantes = _service.RepresentantesComUsuario(nome, RG_ID, UEN_ID, null, pagina, registroPorPagina, nivelAcesso, NRP_ID);
                    response.AddPage("representantes", representantes);
              
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            ViewBag.USU_LOGIN = SessionContext.login;
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int REP_ID)
        {
            ViewBag.USU_LOGIN = SessionContext.login;
            ViewBag.REP_ID = REP_ID;
            return View();
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult RecuperarDadosDoRepresentante(int REP_ID)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var representante = _service.FindByIdWithUser(REP_ID, false, true);
                response.Add("representante", representante);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI, VENDA_ASSI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Incluir(RepresentanteDTO representante)
        {
            JSONResponse result = new JSONResponse();
            
            try
            {
                if (ModelState.IsValid)
                {
                    int? RG_ID = null;
                    int? uenId = null;
                    
                    if (!SessionContext.IsAdmDepartamento("Franquiador") && !SessionContext.IsAdmDepartamento("TI"))
                    {
                        uenId = SessionUtil.GetUenId();                        
                    }

                    if (!SessionContext.IsGerenteDepartamento("franquiado"))
                    {
                        RG_ID = (SessionContext.IsGerenteDepartamento("FRANQUIADO")) ? SessionUtil.GetRegiao() : null;
                    }

                    _service.SalvarRepresentanteEUsuario(representante, false, RG_ID, uenId);
                                       

                    SysException.RegistrarLog("Dados do representante atualizados com sucesso!!", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do representante atualizados com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI, VENDA_ASSI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public override ActionResult Salvar(RepresentanteDTO representante)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? RG_ID = null;
                    int? uenId = null;

                    if (!SessionContext.IsAdmDepartamento("Franquiador") && !SessionContext.IsAdmDepartamento("TI"))
                    {
                        uenId = SessionUtil.GetUenId();
                    }

                    if (SessionContext.IsGerenteDepartamento("franquiado"))
                    {
                        RG_ID = (SessionContext.IsGerenteDepartamento("FRANQUIADO")) ? SessionUtil.GetRegiao() : null;
                    }

                    _service.SalvarRepresentanteEUsuario(representante, true, RG_ID, uenId);
                    SysException.RegistrarLog("Dados do representante atualizados com sucesso!!", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do representante atualizados com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RepresentantesDaRegiaoDoRepresentanteLogado(int? CLI_ID, string nome = null, string descricaoUf = null, int pagina = 1, int registroPorPagina = 5)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    var RG_ID = SessionUtil.GetRegiao();
                    int? uenId = SessionUtil.GetUenId();
                    var representantes = _service.RepresentantesDoCliente(CLI_ID, nome, descricaoUf, RG_ID, uenId, false, pagina, registroPorPagina);
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

        [Autorizar(PorMenu = false)]
        public ActionResult RepresentantesDaRegiao(
            bool cpfExato = true,
            int? UEN_ID = null,
            string REP_NOME = null,
            string login = null,
            string nome = null,
            string cpf = null,
            string email = null,
            int? REP_ID_SUP = null,
            int pagina = 1,
            int registrosPorPagina = 12)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? uenId = SessionUtil.GetUenId();
                var representantes = _service.RepresentantesDaRegiaoPaginado(false, cpfExato, uenId, REP_NOME, login, nome, cpf, email, REP_ID_SUP, pagina, registrosPorPagina);
                response.AddPage("representantes", representantes);
               
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult BuscarRepresentante(
           RepresentanteFiltrosDTO filtros)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                filtros.UEN_ID = SessionUtil.FranquiadoOuGerente() ? filtros.UEN_ID : SessionUtil.GetUenId();
                var representantes = _service.RepresentantesDaRegiaoPaginado(filtros);
                response.AddPage("representantes", representantes);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult ListarRepresentantesP(
            bool cpfExato = true,
            int? UEN_ID = null,
            string REP_NOME = null,
            string login = null,
            string nome = null,
            string cpf = null,
            string email = null,
            int? REP_ID_SUP = null,
            int pagina = 1,
            int registrosPorPagina = 12)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? uenId = SessionUtil.GetUenId();
                var representantes = _service.RepresentantesDaRegiaoPaginado(false, cpfExato, uenId, REP_NOME, login, nome, cpf, email, REP_ID_SUP, pagina, registrosPorPagina);
                response.AddPage("representantes", representantes);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RepresentantesDoCliente(int? CLI_ID, int? RG_ID, int pagina = 1, int registroPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? uenId = SessionUtil.GetUenId();
                var representantes = _service.RepresentantesDoCliente(CLI_ID, null, null, RG_ID, uenId, false , pagina, registroPorPagina);
                response.AddPage("lstRepresentanteDoCliente", representantes);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RepresentantesDoClienteExcetoOLogado(int? CLI_ID, int? RG_ID, int pagina = 1, int registroPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    int? uenId = SessionUtil.GetUenId();
                    var representantes = _service.RepresentantesDoCliente(CLI_ID, null, null, RG_ID, uenId, false, pagina, registroPorPagina, REP_ID);
                    response.AddPage("lstRepresentanteDoCliente", representantes); 
                }
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Retorna uma lista de representante que não possuiem em sua(s) carteira(s) o cliente passado pelo id
        /// Traz apenas da região do representante está logado
        /// </summary>
        /// <param name="CLI_ID"></param>
        /// <param name="nome"></param>
        /// <param name="descricaoUf"></param>
        /// <param name="pagina"></param>
        /// <param name="registroPorPagina"></param>
        /// <returns></returns>
        public ActionResult RepresentantesQueNaoSaoDoCliente(int? CLI_ID, string nome = null, int? RG_ID = null, int pagina = 1, int registroPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    if(!SessionContext.IsGerenteDepartamento("franquiador"))
                    {
                        RG_ID = SessionUtil.GetRegiao();
                    }
                    else if (RG_ID == null)
                    {
                        throw new Exception("Selecione uma região.");
                    }

                    int? uenId = SessionUtil.GetUenId();
                    var representantes = _service.RepresentantesQueNaoSaoDoCliente(CLI_ID, nome, RG_ID, uenId, false, pagina, registroPorPagina);
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


        /// <summary>
        /// Retorna todos os representantes para popular lista para 
        /// autocomplete
        /// </summary>
        /// <param name="RG_ID">Id da região selecionada</param>
        /// <returns></returns>
        public ActionResult ListarRepresentantesAutoComplete(int ? RG_ID)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    if (RG_ID != null)
                    {
                        RG_ID = (!SessionContext.IsGerenteDepartamento("Franquiador")) ? SessionUtil.GetRegiao() : null;
                    }
                    
                    int? uenId = SessionUtil.GetUenId();
                    var lstRepresentanteAutocompleteDTO = _service.RepresentantesDeFranquiaAutoCompleteDTO(RG_ID, uenId);
                    response.Add("lstRepresentanteAutocompleteDTO", lstRepresentanteAutocompleteDTO);
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

        [HttpPost]
        public ActionResult Excluir(int REP_ID)
        {

            JSONResponse result = new JSONResponse();
            try
            {
                _service.DesativarRepresentante(REP_ID);
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


        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true, PorMenu = false)]
        public ActionResult Passivos()
        {
            var uenId = SessionUtil.GetUenId();
            ViewBag.regioes = _regiaoSRV.ListarRegioes(uenId);
            return View();
        }

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult RelatorioDePassivos(DateTime data, DateTime? dataFinal, int? RG_ID = null, int pagina = 1, int registroPorPagina = 100)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    if (SessionContext.IsGerenteDepartamento("Franquiado"))
                    {
                        if (RG_ID == null)
                        {
                            RG_ID = SessionUtil.GetRegiao();
                        }                    
                    }

                    int? uenId = SessionUtil.GetUenId();
                    var relatorioPassivos = _service.RelatorioDePassivos(uenId, RG_ID, data, dataFinal, pagina, registroPorPagina);
                    response.AddPage("relatorioPassivos", relatorioPassivos);
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

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult RelatorioDePassivosRepresentanteLogados(DateTime data, DateTime? dataFinal = null, int? RG_ID = null, int pagina = 1, int registroPorPagina = 100)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    if (SessionContext.IsGerenteDepartamento("Franquiado"))
                    {
                        if (RG_ID == null)
                        {
                            RG_ID = SessionUtil.GetRegiao();
                        }
                    }

                    int? uenId = SessionUtil.GetUenId();
                    var relatorioPassivosRepresentanteLogados = _service.RelatorioDePassivosRepresentantesLogados(uenId, RG_ID, data, dataFinal, pagina, registroPorPagina);
                    response.AddPage("relatorioPassivosRepresentanteLogados", relatorioPassivosRepresentanteLogados);
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

        public ActionResult ListarRepresentanteAutocomplete(string nome)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstRep = _service.ListarRepresentanteAutocomplete(nome);
                response.Add("lstRepresentante", lstRep);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarTodosOsRepresentantesDoCliente(int? CLI_ID, string nome = null, string descricaoUf = null, int? empId = null, bool uenLogada = false, int pagina = 1, int registroPorPagina = 5)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                int? uenID = null;
                if (uenLogada)
                    uenID = SessionUtil.GetUenId();
                var representantes = _service.ListarTodosOsRepresentantesDoCliente(CLI_ID, nome, descricaoUf, null, uenID, false, empId, pagina, registroPorPagina);
                response.AddPage("representantes", representantes);
                                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
