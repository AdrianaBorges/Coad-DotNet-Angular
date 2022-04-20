using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class TipoPeriodoController : Controller
    {
        private TipoPeriodoSRV _service = new TipoPeriodoSRV();

        

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult LstTipoPeriodo()
        {
            IList<TipoPeriodoDTO> produtos = _service.FindAll();
            JSONResponse response = new JSONResponse();
            response.Add("tiposPeriodo", produtos);

            return Json(response);
        }

    }
}
