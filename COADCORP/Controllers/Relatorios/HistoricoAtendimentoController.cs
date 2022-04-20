using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Relatorios
{
    public class HistoricoAtendimentoController : Controller
    {
        //
        // GET: /HistoricoAtendimento/

        HistAtendSRV _histsrv = new HistAtendSRV();
        public ActionResult Index()
        {
            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarAtendimentoPorPeriodo(DateTime _dtini, DateTime _dtfim)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var _listahistatend = _histsrv.BuscarAtendimentoPorTipo(_dtini, _dtfim, 3);

                result.Add("listahistatend", _listahistatend);
                result.success = true;
                result.message = Message.Info("Ok");
                return Json(result, JsonRequestBehavior.AllowGet);
              
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
