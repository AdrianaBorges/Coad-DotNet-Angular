using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class TasksController : Controller
    {
        private HistoricoExecucaoSRV _serviceHistExe = new HistoricoExecucaoSRV();

        [Autorizar(IsAjax = true)]
        public ActionResult Run()
        {
            JSONResponse result = new JSONResponse();

            try
            {
                _serviceHistExe.Incluir("Teste de log de execução", "Esse é um teste de execução de tarefas agendadas", DateTime.Now, "TasksController", "COADCORP");
                result.message = Message.Success("Task executada com sucesso!!!");
            }
            catch(Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);

        }


    }
}
