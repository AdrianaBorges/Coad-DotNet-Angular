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

    public class GrupoConsultoriaController : Controller
    {
        //private GrupoConsultoriaSRV _service = new GrupoConsultoriaSRV();

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
        public ActionResult GruposConsultoria(int? grupoConsultoriaId, string descricao = null, int ativoId = 1, int pagina = 0)
        {
            //Pagina<GrupoConsultoriaDTO> page = _service.GruposConsultoria(grupoConsultoriaId, descricao: descricao, ativoId: ativoId, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            //response.AddPage("gruposConsultoria", page);

            return Json(response);
        }

        [Autorizar]
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

        [Autorizar]
        [HttpPost]
        public ActionResult Editar(int grupoConsultoriaId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            ViewBag.grupoConsultoriaId = grupoConsultoriaId;

            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(GrupoConsultoriaDTO grupoConsultoria)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    //_service.SalvarGrupoConsultoria(grupoConsultoria);
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

        [Autorizar]
        [HttpPost]
        public ActionResult Remover(int grupoConsultoriaId)
        {
            try
            {
                //_service.DeletarGrupoConsultoria(grupoConsultoriaId);
                TempData["message"] = Message.Success("Registro excluído com sucesso!");
            }
            catch (Exception e)
            {
                TempData["message"] = Message.Fail(e.Message);
            }
            return RedirectToAction("Index");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReadgrupoConsultoria(int grupoConsultoriaId)
        {
            //var grupoConsultoria = _service.FindById(grupoConsultoriaId);
            JSONResponse response = new JSONResponse();
            //response.Add("grupoConsultoria", grupoConsultoria);

            return Json(response);
        }

        [Autorizar]
        [HttpPost]
        public ActionResult Detalhes(int grupoConsultoriaId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            ViewBag.grupoConsultoriaId = grupoConsultoriaId;

            return View("Detalhes");
        }
    }
}