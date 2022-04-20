using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Custons.Pesquisas;
using COAD.SEGURANCA.Model.Dto;
using COAD.SEGURANCA.Model.Dto.Custons.Pesquisas;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class FilaEmailController : Controller
    {
        public FilaEmailSRV _service { get; set; }
        //
        // GET: /Templates/

        [Autorizar(PorMenu = false)]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        public JsonResult PesquisarFilaEmail(PesquisarFilaEmailDTO pesquisaEmailDTO)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstFilaEmail = _service.PesquisarFilaEmail(pesquisaEmailDTO);
                response.AddPage("lstFilaEmail", lstFilaEmail);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {            
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Editar(int? tplId)
        {
            ViewBag.tplId = tplId;
            return View();
        }
        

        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDaFilaEmail(int? fleId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var filaEmail = _service.FindById(fleId);
                response.Add("filaEmail", filaEmail);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult CancelarEmailNaFila(int? fleId, string contexto = null)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;

                _service.CancelarEmailNaFila(fleId, contexto);
                SysException.RegistrarLog("E-Mail cancelado com sucesso!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("E-Mail cancelado com sucesso!!");

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
        public ActionResult AlterarEmail(ListaFilaEmailDTO filaEmail)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? repId = SessionContext.GetIdRepresentante();
                    string usuario = SessionContext.login;

                    _service.AlterarEnderecoEmail(filaEmail);
                    SysException.RegistrarLog("E-Mail alterado com sucesso!!", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("E-Mail cancelado com sucesso!!");

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

    }
}
