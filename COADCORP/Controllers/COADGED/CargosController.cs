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

    public class CargosController : Controller
    {
        private CargosSRV _service = new CargosSRV();

        [Autorizar]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Cargos(int? cargoId, string descricao, int pagina = 0)
        {
            Pagina<CargosDTO> page = _service.Cargos(cargoId, descricao: descricao, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("cargos", page);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            return View("Edit");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int cargoId)
        {
            ViewBag.cargoId = cargoId;
            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(CargosDTO cargos)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarCargo(cargos);
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
        public ActionResult Remover(int cargoId)
        {

            TempData["message"] = "A remoção deste registro não é permitida!";
            return RedirectToAction("Index");

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Readcargo(int cargoId)
        {
            var cargos = _service.FindById(cargoId);
            JSONResponse response = new JSONResponse();
            response.Add("cargos", cargos);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int cargoId)
        {
            ViewBag.cargoId = cargoId;
            return View("Detalhes");
        }
    }
}