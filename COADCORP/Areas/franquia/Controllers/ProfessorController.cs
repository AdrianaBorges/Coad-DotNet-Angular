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
using COAD.PROXY.Service;
using COAD.PROXY.Model.DTO;

namespace COADCORP.Areas.franquia.Controllers
{
    [Autorizar(Departamento = "Franquiado, Franquiador, TI", PorMenu = false, PermitirNiveisPrivilegiosSuperiores = true)]
    public class ProfessorController : Controller
    {
        private ProfessorProxySRV _service = new ProfessorProxySRV();
        private RegiaoSRV _regiaoSRV = new RegiaoSRV();
        private PrioridadeAtendimentoSRV _prioridadeAtendimentoSRV = new PrioridadeAtendimentoSRV();
        private ClienteSRV _clienteSRV = new ClienteSRV();
        private AgendamentoSRV _agendamentoSRV = new AgendamentoSRV();
        private FilaCadastroSRV _filaService = new FilaCadastroSRV();
        private NivelRepresentanteSRV _nivelRepresentanteService = new NivelRepresentanteSRV();
        private ColecionadorProxySRV _colecionadorProxySRV = new ColecionadorProxySRV();
        private GrandeGrupoProxySRV _grandeGrupoProxySRV = new GrandeGrupoProxySRV();
        //
        // GET: /franquia/Representante/

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", PorMenu = false, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Index()
        {

            int? nivelAcesso = SessionUtil.GetNivelAcessoPerfil();
            ViewBag.lstUen = new UENSRV().FindAll();
            
            return View();
        }
       
        [Autorizar(GerenteDepartamento = "FRANQUIADO, FRANQUIADOR, TI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult ListarProfessoresComUsuario(string nome = null, int pagina = 1, int registroPorPagina = 5, int? UEN_ID = null, int? RG_ID = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (!SessionContext.IsAdmDepartamento("franquiador") && !SessionContext.IsAdmDepartamento("TI"))
                {
                    UEN_ID = SessionUtil.GetUenId();
                }

                int? nivelAcesso = SessionUtil.GetNivelAcessoPerfil();
                
                if(SessionContext.IsGerenteDepartamento("FRANQUIADO"))
                {
                    RG_ID = SessionUtil.GetRegiao();   
                }

                var professores = _service.ListarProfessorComUsuario(nome, RG_ID, UEN_ID, pagina, registroPorPagina, nivelAcesso);
                response.AddPage("professores", professores);
              
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
        public JsonResult RecuperarDadosDoProfessor(int REP_ID)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var professor = _service.FindByIdWithUser(REP_ID);
                response.Add("professor", professor);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Incluir(ProfessorProxyDTO representante)
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

                    _service.SalvarProfessorEUsuario(representante, false, RG_ID, uenId);
                                       

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

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Salvar(ProfessorProxyDTO professor)
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

                    _service.SalvarProfessorEUsuario(professor, true, RG_ID, uenId);
                    SysException.RegistrarLog("Dados do professor atualizados com sucesso!!", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do professor atualizados com sucesso!!");

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

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult LstColecionadores()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstColecionadores = _colecionadorProxySRV.ListarAreas();
                response.Add("lstColecionadores", lstColecionadores);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult LstGrandeGrupo(int? areConsId = null, string nome = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstGrandeGrupo = _grandeGrupoProxySRV.ListarGrandeGrupo(areConsId, nome);
                response.Add("lstGrandeGrupo", lstGrandeGrupo);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
