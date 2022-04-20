using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Util;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coad.GenericCrud.Exceptions;
using COAD.SEGURANCA.Service;

namespace COADCORP.Areas.franquia.Controllers
{
    public class RegiaoController : Controller
    {
        //
        // GET: /franquia/Fila/

        public RegiaoSRV _regiaoSRV { get; set; }
        public EmpresaSRV _empresaSRV { get; set; }
        public UENSRV _uenSRV { get; set; }
        public UFSRV _ufSRV { get; set; }
        public MunicipioSRV _municipioSRV { get; set; }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Index()
        {
            var lstUEN = _uenSRV.FindAll();
            var lstUF = _ufSRV.BuscarSomenteUF();
            var lstMunicipio = _municipioSRV.FindAll();

            ViewBag.lstUEN = lstUEN;
            ViewBag.lstUF = lstUF;
            ViewBag.lstMunicipio = lstMunicipio;

            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult Regioes(int? UEN_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (!SessionContext.IsAdmDepartamento("Franquiador") && !SessionContext.IsAdmDepartamento("Franquiado") && UEN_ID != null)
                {
                    throw new AcessoException("Seu usuário não tem permissão para informar o grupo da região");
                }
                else if(SessionContext.IsGerenteDepartamento("Franquiador", true) && UEN_ID == null)
                {
                    UEN_ID = SessionUtil.GetUenId();
                }
                
                if (UEN_ID == null)
                {
                    UEN_ID = 1;
                }

                var lstRegiao = _regiaoSRV.ListarRegioes(UEN_ID);
                response.Add("lstRegiao", lstRegiao);
                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult RegioesExcluindoRegiaoDoRepresentante()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID_PARA_EXCLUIR = null;

                if (AuthUtil.TryGetRepId(out REP_ID_PARA_EXCLUIR))
                {
                    var lstRegiao = _regiaoSRV.FindAll(REP_ID_PARA_EXCLUIR);
                    response.Add("lstRegiao", lstRegiao);
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
        public ActionResult RegioesDoCliente(int? CLI_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var uenId = SessionUtil.GetUenId();
                var lstRegioesDoCliente = _regiaoSRV.FindRegioesDoCliente(CLI_ID, uenId);
                response.Add("lstRegioesDoCliente", lstRegioesDoCliente);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }


        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int RG_ID)
        {
            ViewBag.USU_LOGIN = SessionContext.login;
            ViewBag.RG_ID = RG_ID;
            return View();
        }


        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult RecuperarDadosDaRegiao(int regiaoId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var regiao = _regiaoSRV.FindByIdFullLoaded(regiaoId, true, true);
                response.Add("regiao", regiao);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListarRegiao(string descricao, int? empId, string uf, int? munId, string empRazaoSocial = null, int pagina = 1, int registrosPorPagina = 8, int? uenId = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var lstRegioes = _regiaoSRV.ListarRegiao(descricao, empId, uf, munId, empRazaoSocial, pagina, registrosPorPagina, uenId);
                    response.AddPage("lstRegioes", lstRegioes);
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


        public ActionResult ListarComboRegiao(int? uenId = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstRegioes = _regiaoSRV.ListarRegioesCombo( uenId);
                response.Add("lstRegioes", lstRegioes);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Salvar(RegiaoDTO regiao)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _regiaoSRV.SalvarRegiao(regiao);
                    SysException.RegistrarLog("Dados da região atualizados com sucesso!!", "", SessionContext.autenticado);

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

        public ActionResult ListarUfs()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listUfs = _ufSRV.BuscarSomenteUF();
                response.Add("listUfs", listUfs);
                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarEmpresa(string razaoSocial = null, string nomeFantasia = null, int pagina = 1, int registroPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstEmpresa = _empresaSRV.ListarEmpresa(razaoSocial, nomeFantasia, pagina, registroPorPagina);
                response.AddPage("lstEmpresa", lstEmpresa);
                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarRegioesPorNome(string nome = null, int? uenId = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstRegioes = _regiaoSRV.ListarRegioesPorNome(nome, uenId);
                response.Add("lstRegioes", lstRegioes);

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
