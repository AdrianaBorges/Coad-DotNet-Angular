using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Areas.franquia.Controllers
{
    public class ClienteTelefoneController : Controller
    {
        private ClienteTelefoneSRV _service = new ClienteTelefoneSRV();
        private TipoTelefoneSRV _tipoTelService = new TipoTelefoneSRV();
        //
        // GET: /franquia/Fila/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarTipoTelefone()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstTipoTelefone = _tipoTelService.FindAll();
                response.Add("lstTipoTelefone", lstTipoTelefone);
            }
            catch (Exception e)
            {

                response.message = Message.Fail(e);
            }

            return Json(response); 
        }
    }
}
