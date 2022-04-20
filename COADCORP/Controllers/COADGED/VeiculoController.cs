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

    public class VeiculoController : Controller
    {
        private VeiculoSRV _service = new VeiculoSRV();
        private PeriodicidadeSRV _servicePeriodicidade = new PeriodicidadeSRV();

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


            // periodicidade...................................................
            var periodo = _servicePeriodicidade.FindAll();
            ViewBag.periodo = periodo.Select(c => new SelectListItem() { Text = c.PRD_DESCRICAO, Value = c.PRD_ID.ToString() });

            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Veiculos(int? veiculoId, string descricao = null, int? periodoId = null, int? ativoId = null, int pagina = 0)
        {
            Pagina<VeiculoDTO> page = _service.Veiculos(veiculoId, descricao: descricao, periodoId: periodoId, ativoId: ativoId, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("veiculos", page);

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


            // periodicidade...................................................
            var periodo = _servicePeriodicidade.FindAll();
            ViewBag.periodo = periodo.Select(c => new SelectListItem() { Text = c.PRD_DESCRICAO, Value = c.PRD_ID.ToString() });

            return View("Edit");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int veiculoId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");


            // periodicidade...................................................
            var periodo = _servicePeriodicidade.FindAll();
            ViewBag.periodo = periodo.Select(c => new SelectListItem() { Text = c.PRD_DESCRICAO, Value = c.PRD_ID.ToString() });

            ViewBag.veiculoId = veiculoId;
            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(VeiculoDTO veiculo)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarVeiculo(veiculo);
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
        public ActionResult Remover(int veiculoId)
        {

            TempData["message"] = "A remoção deste registro não é permitida!";
            return RedirectToAction("Index");

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Readveiculo(int veiculoId)
        {
            var veiculo = _service.FindById(veiculoId);
            JSONResponse response = new JSONResponse();
            response.Add("veiculo", veiculo);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int veiculoId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");


            // periodicidade...................................................
            var periodo = _servicePeriodicidade.FindAll();
            ViewBag.periodo = periodo.Select(c => new SelectListItem() { Text = c.PRD_DESCRICAO, Value = c.PRD_ID.ToString() });

            ViewBag.veiculoId = veiculoId;
            return View("Detalhes");
        }
    }
}