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
using COAD.CORPORATIVO.Service.Custons;
using System.Threading.Tasks;
using System.Web.SessionState;
using COAD.CORPORATIVO.Service.SqlDinamico;
using COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery;
using GenericCrud.Exceptions.ErrorHandling;
using GenericCrud.Service;

namespace COADCORP.Areas.franquia.Controllers
{
    //[SessionState(SessionStateBehavior.ReadOnly)]
    public class SqlDinamicoController : Controller
    {
        public SqlDinamicoSRV _sqlDinamico { get; set; }
        public RelatorioPersonalizadoSRV _relPersonalizado { get; set; }
        public RelatorioOperadorLogicoSRV _operadorLogicoSRV { get; set; }
        public RelatorioOperadorCondicionalSRV _operadorCondicional { get; set; }
        public RelatorioTabelaColunasSRV _relatorioColunas { get; set; }

        [Autorizar(PorMenu = false)]
        public ActionResult RelatorioBase()
        {
            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult Editar(int? relId)
        {
            ViewBag.relId = relId;
            ViewBag.useNewAngular = true;
            var rel = _relPersonalizado.FindById(relId);

            if (rel != null && rel.RET_RELATORIO_BASE == true)
                return View();
            else
                return View("EditarRelDerivado");         
            
        }

        [Autorizar(PorMenu = false)]
        public ActionResult NovoPersonalizado()
        {
            ViewBag.useNewAngular = true;
            return View("EditarRelDerivado");
        }

        [Autorizar(PorMenu = false)]
        public ActionResult NovoBase()
        {
            ViewBag.useNewAngular = true;
            return View("Editar");
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarOperadorLogico()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstOperadoresLogicos = _operadorLogicoSRV.FindAll();
                response.Add("lstOperadoresLogicos", lstOperadoresLogicos);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarOperadorCondicional()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstOperadoresCondicionais = _operadorCondicional.FindAll();
                response.Add("lstOperadoresCondicionais", lstOperadoresCondicionais);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        [Autorizar(IsAjax = true)]
        public ActionResult ListarTabelasDoSistema()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstTabelas = _relPersonalizado.ListarTabelas();
                response.Add("lstTabelas", lstTabelas);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescreverColunasDaTabela(string nomeTabela)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstColunas = _relPersonalizado.DescreverColunasDaTabela(nomeTabela);
                response.Add("lstColunas", lstColunas);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        

        [Autorizar(IsAjax = true)]
        public ActionResult SalvarRelatorioPersonalizado(RelatorioPersonalizadoDTO relatorioPersonalizado)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? REP_ID = SessionContext.GetIdRepresentante();
                    var login = SessionContext.autenticado.USU_LOGIN;
                    relatorioPersonalizado.REP_ID = REP_ID;
                    //relatorioPersonalizado.RG_ID = SessionUtil.GetRegiao();
                    //relatorioPersonalizado.UEN_ID = SessionUtil.GetUenId();
                    relatorioPersonalizado.USU_LOGIN = login;

                    _relPersonalizado.SalvarRelatorioPersonalizado(relatorioPersonalizado);

                    SysException.RegistrarLog("Relatório personalizado salvo com sucesso!", "", SessionContext.autenticado);
                    result.success = true;
                    
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState, true);
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
        public ActionResult SalvarRelatorioPersonalizadoDerivado(RelatorioPersonalizadoDTO relatorioPersonalizado)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? REP_ID = SessionContext.GetIdRepresentante();
                    var login = SessionContext.autenticado.USU_LOGIN;
                    relatorioPersonalizado.REP_ID = REP_ID;
                    //relatorioPersonalizado.RG_ID = SessionUtil.GetRegiao();
                    //relatorioPersonalizado.UEN_ID = SessionUtil.GetUenId();
                    relatorioPersonalizado.USU_LOGIN = login;

                    _relPersonalizado.SalvarRelatorioPersonalizadoDerivado(relatorioPersonalizado);

                    SysException.RegistrarLog("Relatório personalizado salvo com sucesso!", "", SessionContext.autenticado);
                    result.success = true;

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState, true);
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
        public ActionResult ExcluirRelatorioPersonalizado(int? REL_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _relPersonalizado.ExcluirRelatorioPersonalizado(REL_ID);
                SysException.RegistrarLog("Relatório personalizado excluído com sucesso!", "", SessionContext.autenticado);
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

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult RecuperarDadosDoRelatorioPersonalizado(int relId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var relPersonalizado = _relPersonalizado.FindByIdFullLoaded(relId, true, true, true, true, true);
                response.Add("relPersonalizado", relPersonalizado);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult RecuperarDadosDoRelatorioPersonalizadoDerivado(int relId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var relPersonalizado = _relPersonalizado.FindByIdFullLoaded(relId, true, true, true, true, true);
                response.Add("relPersonalizado", relPersonalizado);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult MostrarPreviewQuery(int relId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var montagemQuery = _relPersonalizado.GerarQueryDoRelatorio(relId);
                response.Add("montagemQuery", montagemQuery.ToString());

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult MostrarPreviewResultado(int relId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var resultado = _relPersonalizado.ExecutarQueryDinamica(relId, null, 5);
                response.Add("resultado", resultado);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AjaxExceptionFilter]
        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult RodarResultadoRelatorio(int relId, IEnumerable<DadosDeFiltroDTO> lstFiltros = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var resultado = _relPersonalizado.ExecutarQueryDinamica(relId, lstFiltros, 5);
                response.Add("resultado", resultado);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult ListarRelatorioPersonalizadoBase(int pagina = 1, int registrosPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var ehTI = SessionContext.HasDepartamento("TI");
                string usuario = null;
                if (!ehTI)
                {
                    usuario = SessionContext.login;
                }

                var lstDadosRelatorio = _relPersonalizado.ListarRelatorioPersonalizadoBase(usuario, pagina, registrosPorPagina);
                response.AddPage("lstDadosRelatorio", lstDadosRelatorio);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult ListarRelatorioPersonalizado(int pagina = 1, int registrosPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var ehTI = SessionContext.HasDepartamento("TI");
                string usuario = null;
                if (!ehTI)
                {
                    usuario = SessionContext.login;
                }
                var lstRelatorios = _relPersonalizado.ListarRelatorioPersonalizado(usuario, pagina, registrosPorPagina);
                response.AddPage("lstRelatorios", lstRelatorios);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult listarColunasDoRelatorioFormatado(int? relId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstColunasFormatada = _relatorioColunas.ListarColunasDoRelatorioFormatado(relId);
                response.Add("lstColunasFormatada", lstColunasFormatada);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult ObterMetaDadoDoRelatorio(int relId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var metadado = _relPersonalizado.ObterMetaDadoDoRelatorio(relId);
                response.Add("metadado", metadado);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AjaxExceptionFilter]
        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult RodarResultadoRelatorioPlanilha(int relId, IEnumerable<DadosDeFiltroDTO> lstFiltros = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var serverPath = Server.MapPath("~/");
                var fileName = _relPersonalizado.ExecutarQueryDinamicaPlanilha(serverPath, relId, lstFiltros);
                response.Add("fileName", fileName);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public FileResult BaixarPlanilha(string fileName)
        {

            var downloadInfo = _relPersonalizado.RetornarPlanilha(fileName);
            System.IO.File.Delete(downloadInfo.Path);

            return File(downloadInfo.Bytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadInfo.FileName);
        }

        [Autorizar(IsAjax = true)]
        public JsonResult ListarTipoJoin()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstTipoJoin = ServiceFactory.RetornarServico<TipoJoinSRV>().FindAll();
                response.Add("lstTipoJoin", lstTipoJoin);
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
