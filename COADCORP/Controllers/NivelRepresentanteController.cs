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
    public class NivelRepresentanteController : Controller
    {
        //
        // GET: /franquia/Fila/

        private NivelRepresentanteSRV _service = new NivelRepresentanteSRV();

        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarNiveisRepresentante()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? nivelAcesso = SessionUtil.GetNivelAcessoPerfil();
                var lstNivelRepresentante = _service.ListarNivelRepresentante(nivelAcesso, true);
                response.Add("lstNivelRepresentante", lstNivelRepresentante);
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
