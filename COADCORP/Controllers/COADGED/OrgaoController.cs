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

    public class OrgaoController : Controller
    {
        private OrgaoSRV _service = new OrgaoSRV();

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
        public ActionResult Orgaos(int? orgaoId, string descricao = null, int ativoId = 1, int pagina = 0)
        {
            Pagina<OrgaoDTO> page = _service.Orgaos(orgaoId, descricao: descricao, ativoId: ativoId, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("orgaos", page);

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
        public ActionResult Editar(int orgaoId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            ViewBag.orgaoId = orgaoId;

            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(OrgaoDTO orgao)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarOrgao(orgao);
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

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Readorgao(int orgaoId)
        {
            var orgao = _service.FindById(orgaoId);
            JSONResponse response = new JSONResponse();
            response.Add("orgao", orgao);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int orgaoId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            ViewBag.orgaoId = orgaoId;

            return View("Detalhes");
        }
    }
}