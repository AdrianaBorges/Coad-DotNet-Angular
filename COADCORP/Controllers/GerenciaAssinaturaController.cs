using Coad.GenericCrud.ActionResultTools;
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
    public class GerenciaAssinaturaController : Controller
    {
        public AssinaturaSRV _service { get; set; }
        public ClienteSRV _clienteSRV { get; set; }
        public ContratoSRV _contratoSRV { get; set; }
        public ParcelasSRV _parcelasSRV { get; set; }

        //
        // GET: /GerenciaAssinatura/

        public ActionResult Index()
        {
            return View();
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
        public ActionResult ListarAssinaturas(int cliId, int pagina = 1, int registrosPorPagina = 7)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var lstAssinatura = _service.BuscarPorCliente(cliId, pagina, registrosPorPagina);
                response.AddPage("lstAssinatura", lstAssinatura);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
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


        [Autorizar(Departamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        public ActionResult MigrarAssinatura(ProcessoTransferenciaAssinaturaDTO transfAssinatura)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? REP_ID = SessionContext.GetIdRepresentante();                    
                    var login = SessionContext.autenticado.USU_LOGIN;
                    transfAssinatura.RepId = REP_ID;
                    transfAssinatura.Login = login;

                    string codNovaAssinatura = _service.MigrarAssinatura(transfAssinatura);

                    result.Add("codNovaAssinatura", codNovaAssinatura);
                    SysException.RegistrarLog("Assinatura migrada com sucesso!", "", SessionContext.autenticado);
                    result.success = true;                    
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

    }
}
