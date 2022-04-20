using System;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using CONFIGSIS.Models;
using COAD.SEGURANCA.Filter;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Exceptions;
using COAD.CORPORATIVO.Service;

namespace CONFIGSIS.Controllers
{
    public class DepartamentoController : Controller
    {

        private DepartamentoSRV _service = new DepartamentoSRV();
        //
        // GET: /Departamento/

        [Autorizar]
        public ActionResult Index()
        {
            ViewBag.Title = "Departamentos";             
            return View();
            
        }

        [Autorizar(IsAjax = true)]
        public ActionResult Departamentos(string nome, int pagina = 1, int registroPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                Pagina<DepartamentoDTO> lstDepartamento = null;
                response.success = true;
                lstDepartamento = _service.Departamentos(nome, pagina, registroPorPagina);               
                             
                response.AddPage("lstDepartamentos", lstDepartamento);    

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(TipoAcesso = NivelAcesso.Editar)]
        public ActionResult Novo()
        {
             return View("Editar");
        }

        [Autorizar(TipoAcesso = NivelAcesso.Editar)]
        [HttpPost]
        public ActionResult Editar(string DP_ID)
        {
            ViewBag.DP_ID = DP_ID;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDoDepartamento(int DP_ID)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = _service.FindById(DP_ID);
                response.Add("departamento", usuario);                
                
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }      

        public ActionResult Salvar(DepartamentoDTO departamento)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SaveOrUpdate(departamento);
                    SysException.RegistrarLog("Dados do departamento atualizados com sucesso!!", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do departamento atualizados com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (EmailNaoEnviadoException ex)
            {
                SessionUtil.HandleException(ex);
                result.message = Message.Warning("O departamento foi salvo com successo. Porém não foi possível enviar o email.");
                result.success = false;
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
             
      
        [HttpPost]
        public ActionResult Excluir(int DP_ID)
        {

            JSONResponse result = new JSONResponse();
            try
            {
                _service.DeletarDepartamento(DP_ID);
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

    }
}
