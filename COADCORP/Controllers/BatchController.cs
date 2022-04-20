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
using COAD.SEGURANCA.Service.Custons;
using COAD.FISCAL.Service.Integracoes.Interfaces;

namespace COADCORP.Areas.franquia.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BatchController : Controller
    {
        private BatchCustomSRV _batchSRV = new BatchCustomSRV();
        private ImportacaoSuspectSRV _importacaoSuspect = new ImportacaoSuspectSRV();
        public ILoteNFeSRV LoteSRV { get; set; }

        //[Autorizar(IsAjax = true)]
        public ActionResult RetornarStatusDeBatchImportacaoSuspect()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                string sessionId = HttpContext.Session.SessionID;
                var batchStatus = _batchSRV.RetornarStatusDeBatchImportacaoSuspect(sessionId);
                response.Add("batchStatus", batchStatus);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        //[Autorizar(IsAjax = true)]
        //public void ExecutarImportacaoDeSuspectAsync()
        //{
        //    var sessionId = HttpContext.Session.SessionID;
        //    int? REP_ID = SessionContext.GetIdRepresentante();
        //    AsyncManager.OutstandingOperations.Increment();

        //    Task.Factory.StartNew(taskId => { 
            
        //        JSONResponse response = new JSONResponse();
        //        try
        //        {
        //            var contextoImportacao = _importacaoSuspect.ImportarClientes(sessionId, REP_ID);
        //            response.Add("contextoImportacao", contextoImportacao);                   
              
        //        }
        //        catch (Exception e)
        //        {
        //            response.success = false;
        //            response.message = Message.Fail(e);
        //        }

        //        AsyncManager.OutstandingOperations.Decrement();
        //        AsyncManager.Parameters["result"] = response;
        //        return response;

        //    }, "importacaoSuspect");
        //}

        //public ActionResult ExecutarImportacaoDeSuspectCompleted(JsonResult result)
        //{
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //[Autorizar(IsAjax = true)]
        public ActionResult ExecutarImportacaoDeSuspect()
        {
            var sessionId = HttpContext.Session.SessionID;
            int? REP_ID = SessionContext.GetIdRepresentante();
            JSONResponse response = new JSONResponse();
            try
            {
                var contextoImportacao = _importacaoSuspect.ImportarClientes(sessionId, REP_ID);
                response.Add("contextoImportacao", contextoImportacao);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RetornarStatusDoLote(List<int> lstCod)
        {
            JSONResponse response = new JSONResponse();
            try {

                var lstLoteBatch = LoteSRV.RetornarStatusDoLote(lstCod);
                response.Add("lstLoteBatch", lstLoteBatch);
                

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
