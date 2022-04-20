using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.COADGED.Service;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Filter;
using COAD.COADGED.Model.DTO;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COADCORP.Controllers.COADGED
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PeriodicidadeController : Controller
    {
        private PeriodicidadeSRV _service = new PeriodicidadeSRV();

        [Autorizar]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Periodicidades(int? periodoId, string descricao, int pagina = 0)
        {
            Pagina<PeriodicidadeDTO> page = _service.Periodicidades(periodoId, descricao: descricao, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("periodicidades", page);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            return View("Edit");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int periodoId)
        {
            ViewBag.periodoId = periodoId;
            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(PeriodicidadeDTO periodicidade)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarPeriodicidade(periodicidade);
                    return Json(result);
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result);
            }
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Remover(int periodoId)
        {

            TempData["message"] = "A remoção deste registro não é permitida!";
            return RedirectToAction("Index");

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Readperiodicidade(int periodoId)
        {
            var periodicidade = _service.FindById(periodoId);
            JSONResponse response = new JSONResponse();
            response.Add("periodicidade", periodicidade);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int periodoId)
        {
            ViewBag.periodoId = periodoId;
            return View("Detalhes");
        }
    }
}