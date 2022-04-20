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

namespace COADCORP.Areas.franquia.Controllers
{
    public class HistoricoController : Controller
    {
        //
        // GET: /franquia/Fila/

        private HistAtendSRV _service = new HistAtendSRV();
        private HistoricoPedidoSRV _serviceHistPedido = new HistoricoPedidoSRV();

        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ListarHistoricoCliente(int CLI_ID, DateTime? dataInicial = null, DateTime? dataFinal = null, 
            int? pagina = 1, 
            int? registrosPorPagina = 10)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var uenId = SessionUtil.GetUenId();

                var lstHistorico = _service.FindHistoricoByCliId(CLI_ID, dataInicial, dataFinal, (int) pagina, (int) registrosPorPagina, uenId);
                result.AddPage("lstHistoricos", lstHistorico);
                
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [Autorizar(PorMenu = false)]
        public ActionResult MostrarTudo(int CLI_ID)
        {
            ViewBag.clienteId = CLI_ID;
            ViewBag.mostraTudo = true;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarHistoricoClienteCompleto(int CLI_ID, 
            DateTime? dataInicial = null, 
            DateTime? dataFinal = null)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var UEN_ID = SessionUtil.GetUenId();
                var lstHistorico = _service.FindHistoricoByCliIdSemPaginacao(CLI_ID, dataInicial, dataFinal, UEN_ID);
                result.Add("lstHistoricos", lstHistorico);

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
