using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;

namespace COADCORP.Controllers.Plugins
{
    public class DadosParcelasController : Controller
    {
        public AssinaturaSRV _assinaturaSRV { get; set; }
        public ContratoSRV _contratoSRV { get; set; }
        public ParcelasSRV _parcelasSRV { get; set; }
        //
        // GET: /Comprovantes/

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
        public ActionResult CarregarDadosDaAssinatura(string numeroAssinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var assinatura = _assinaturaSRV.FindByIdFullLoaded(numeroAssinatura, true, true);
                response.Add("assinatura", assinatura);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarParcelasPagas(string contrato = null,
           int? ppiId = null,
           int? ipeId = null,
           int pagina = 1,
           int registrosPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstParcelas = _parcelasSRV.ListarParcelasPagas(contrato, ppiId, ipeId, pagina, registrosPorPagina);
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
        public ActionResult ExtornarVariasParcelas(ICollection<ItemParcelaExtornoDTO> LstParcelasSelecionadas)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var usuLogin = SessionContext.login;
                var repId = SessionContext.rep_id;
                _parcelasSRV.ExtornarVariasParcelas(LstParcelasSelecionadas, repId, usuLogin);

                SysException.RegistrarLog("Parcela(s) extornada com sucesso!", "", SessionContext.autenticado);
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

    }
}
