using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class LoginUnicoController : Controller
    {
        public AssinaturaSRV _assinaturaSRV { get; set; }
        public ClienteSRV _clienteSRV { get; set; }
        public ClienteUsuarioSRV _clienteUsuarioSRV { get; set; }
        
   
        //
        // GET: /LoginUnico/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Configurar(string assinatura, string returnURL = null, string semanticaReturnURL = null)
        {
            
            ViewBag.assinatura = assinatura;
            ViewBag.returnURL = returnURL;
            ViewBag.semanticaReturnURL = semanticaReturnURL;

            if(!_clienteUsuarioSRV.ChecarClienteJaPossuiLogin(assinatura))
                return View();
            return View("LoginCriado");
        }

        public JsonResult ObterDadosDaAssinaturaECliente(string codAssinatura)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                AssinaturaDTO assinatura = _assinaturaSRV.FindByIdFullLoaded(codAssinatura, true, true, false, true);
                response.Add("assinatura", assinatura);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestarSenhaDaAssinatura(string codAssinatura, string senha)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                bool senhaValida = _assinaturaSRV.TestarSenhaDaAssinatura(codAssinatura, senha);
                response.Add("senhaValida", senhaValida);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ChecarUsuarioJaExiste(string usuario)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                bool usuarioExiste = _clienteUsuarioSRV.ChecarUsuarioJaExiste(usuario);
                response.Add("usuarioExiste", usuarioExiste);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SugerirUsuario(string codAssinatura)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                string login = _clienteUsuarioSRV.SugerirUsuario(codAssinatura);
                response.Add("login", login);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarResumoAssinatura(string codAssinatura)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                LoginUnicoAssinaturaDTO logUnicoAssinatura = _assinaturaSRV.BuscarResumoAssinatura(codAssinatura);
                response.Add("logUnicoAssinatura", logUnicoAssinatura);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarResumosDeAssinaturasDoCliente(int? cliId, string excetoAssinatura = null, bool marcarAssinaturasComoNativa = false)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                IList<LoginUnicoAssinaturaDTO> lstLogUnicoAssinatura = _assinaturaSRV.BuscarResumosDeAssinaturasDoCliente(cliId, excetoAssinatura, marcarAssinaturasComoNativa);
                response.Add("lstLogUnicoAssinatura", lstLogUnicoAssinatura);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ValidarCliente(ClienteDto cliente)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    result.success = true;
                    
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState, false);                    
                }

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

        [HttpPost]
        public ActionResult SalvarConfiguracaoLoginUnico(LoginUnicoRequestDTO loginUnicoRequest)
        {
            JSONResponse result = new JSONResponse();
            try
            {   _clienteUsuarioSRV.CriarLoginUnico(loginUnicoRequest);
                    
                    result.success = true;
                    result.message = Message.Info("Dados do prospects atualizados com sucesso!!");

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

    }
}
