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

    public class TipoAtoController : Controller
    {
        private TipoAtoSRV _service = new TipoAtoSRV();

        [Autorizar]
        public ActionResult Index()
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");
            
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult TiposAtos(int? tipoAtoId, string descricao = null, int ativoId = 1, int pagina = 0)
        {
            Pagina<TipoAtoDTO> page = _service.TiposAtos(tipoAtoId, descricao: descricao, ativoId: ativoId, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("tiposAtos", page);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            return View("Edit");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int tipoAtoId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            ViewBag.tipoAtoId = tipoAtoId;

            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(TipoAtoDTO tipoAto)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarTipoAto(tipoAto);
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
        public ActionResult Remover(int tipoAtoId)
        {

            TempData["message"] = "A remoção deste registro não é permitida!";
            return RedirectToAction("Index");

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReadtipoAto(int tipoAtoId)
        {
            var tipoAto = _service.FindById(tipoAtoId);
            JSONResponse response = new JSONResponse();
            response.Add("tipoAto", tipoAto);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int tipoAtoId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            ViewBag.tipoAtoId = tipoAtoId;

            return View("Detalhes");
        }
    }
}