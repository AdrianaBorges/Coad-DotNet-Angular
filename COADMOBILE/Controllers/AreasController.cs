
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Filter;
using COAD.PORTAL.Service.CalendarioObrigacoes;
using Coad.GenericCrud.ActionResultTools;

namespace COADMOBILE.Controllers
{
    public class AreasController : Controller
    {
        private CoAreasSRV _service = new CoAreasSRV();

        public ActionResult Index()
        {
            return View();
        }


        [Autorizar(PorMenu = false)]
        public ActionResult Areas(string codigoArea, string nomeArea ,int pagina = 1, int nLinha = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstAreas = _service.Areas(codigoArea, nomeArea, 1, nLinha);
                response.AddPage("areas", lstAreas);               
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
