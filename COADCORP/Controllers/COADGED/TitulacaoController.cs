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

    public class TitulacaoController : Controller
    {
        private TitulacaoSRV _service = new TitulacaoSRV();
        private AreasSRV _serviceArea = new AreasSRV();

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Todos(int colecionadorId)
        {
            var tit = _service.Titulacoes(null, colecionadorId).lista;
            var titulacao = tit.Where(x => x.TIT_TIPO == "G" || x.TIT_TIPO == "V").Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO.ToString() + " (" + c.TIT_TIPO + ")", Value = c.TIT_ID.ToString() });
            
            JSONResponse response = new JSONResponse();
            response.Add("titulacao", titulacao);

            return Json(response);
        }


        public ActionResult titulacoesInferiores(int? titulacaoId)
        {
            // tipo = (G)rande grupo (V)erbete (S)ubverbete.....................................................
            string[] texto = { "Grande grupo", "Verbete", "Subverbete", "" };
            string[] valor = { "", "G", "V", "S" };
            
            var i = 0;
            var x = 1;

            if (titulacaoId != null)
            {
                var titulacao = _service.FindById(titulacaoId);
                i = Array.IndexOf(valor, titulacao.TIT_TIPO);
                x = i + 1;
            }

            List<SelectListItem> tipo = new List<SelectListItem>();

            if (x <= valor.Length)
            {
                tipo.AddRange(new[] { new SelectListItem() { Text = texto[i], Value = valor[x] } });
            }

            var inferiores = new SelectList(tipo, "Value", "Text");            
            
            JSONResponse response = new JSONResponse();
            response.Add("inferiores", inferiores);

            return Json(response);
        }

        [Autorizar]
        public ActionResult Index()
        {
            // ativo = "Sim" ou "Não"..........................................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");


            // tipo = (G)rande grupo (V)erbete (S)ubverbete.....................................................
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.AddRange(new[]{
                           new SelectListItem() { Text = "Grande grupo", Value = "G" },
                           new SelectListItem() { Text = "Verbete", Value = "V" },
                           new SelectListItem() { Text = "Subverbete", Value = "S" }
            });
            ViewBag.tipo = new SelectList(tipo, "Value", "Text");

            // titulação no nível maior ........................................................................
            var superior = _service.FindAll();
            ViewBag.superior = superior.Where(x => x.TIT_TIPO == "G" || x.TIT_TIPO == "V").Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO.ToString() + " (" + c.TIT_TIPO + ")", Value = c.TIT_ID.ToString() });

            // area ou colecionador ............................................................................
            var area = _serviceArea.FindAll();
            ViewBag.area = area.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Titulacoes(int? titulacaoId, int? areaId, string descricao = null, int ativoId = 1, string tipo = null, int? superiorId = null, int pagina = 0)
        {
            Pagina<TitulacaoDTO> page = _service.Titulacoes(titulacaoId, areaId, descricao: descricao, ativo: ativoId, tipo: tipo, superiorId: superiorId, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("titulacoes", page);

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult filtrarTitulacoes(int? areaId, int? ggId, int? vbId, int pagina = 0)
        {
            Pagina<TitulacaoDTO> page = _service.FiltrarTitulacoes(areaId, ggId, vbId, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("titulacoes", page);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            // ativo = "Sim" ou "Não"..........................................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");


            // tipo = (G)rande grupo (V)erbete (S)ubverbete.....................................................
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.AddRange(new[]{
                           new SelectListItem() { Text = "Grande grupo", Value = "G" },
                           new SelectListItem() { Text = "Verbete", Value = "V" },
                           new SelectListItem() { Text = "Subverbete", Value = "S" }
            });
            ViewBag.tipo = new SelectList(tipo, "Value", "Text");

            // titulação no nível maior ........................................................................
            var superior = _service.FindAll();
            ViewBag.superior = superior.Where(x => x.TIT_TIPO == "G" || x.TIT_TIPO == "V").Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO.ToString()+" ("+c.TIT_TIPO+")", Value = c.TIT_ID.ToString() });

            // area ou colecionador ............................................................................
            var area = _serviceArea.FindAll();
            ViewBag.area = area.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            return View("Edit");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int titulacaoId)
        {
            // ativo = "Sim" ou "Não"..........................................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");


            // tipo = (G)rande grupo (V)erbete (S)ubverbete.....................................................
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.AddRange(new[]{
                           new SelectListItem() { Text = "Grande grupo", Value = "G" },
                           new SelectListItem() { Text = "Verbete", Value = "V" },
                           new SelectListItem() { Text = "Subverbete", Value = "S" }
            });
            ViewBag.tipo = new SelectList(tipo, "Value", "Text");

            // titulação no nível maior ........................................................................
            var superior = _service.FindAll();
            ViewBag.superior = superior.Where(condicao => condicao.TIT_ID != titulacaoId).Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO.ToString() + " (" + c.TIT_TIPO + ")", Value = c.TIT_ID.ToString() });

            ViewBag.titulacaoId = titulacaoId;

            // area ou colecionador ............................................................................
            var area = _serviceArea.FindAll();
            ViewBag.area = area.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(TitulacaoDTO titulacao)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarTitulacao(titulacao);
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
        public ActionResult Remover(int titulacaoId)
        {

            TempData["message"] = "A remoção deste registro não é permitida!";
            return RedirectToAction("Index");

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Readtitulacao(int titulacaoId)
        {
            var titulacao = _service.FindById(titulacaoId);
            JSONResponse response = new JSONResponse();
            response.Add("titulacao", titulacao);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int titulacaoId)
        {
            // ativo = "Sim" ou "Não"..........................................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");


            // tipo = (G)rande grupo (V)erbete (S)ubverbete.....................................................
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.AddRange(new[]{
                           new SelectListItem() { Text = "Grande grupo", Value = "G" },
                           new SelectListItem() { Text = "Verbete", Value = "V" },
                           new SelectListItem() { Text = "Subverbete", Value = "S" }
            });
            ViewBag.tipo = new SelectList(tipo, "Value", "Text");

            // titulação no nível maior ........................................................................
            var superior = _service.FindAll();
            ViewBag.superior = superior.Where(x => x.TIT_TIPO == "G" || x.TIT_TIPO == "V").Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO.ToString() + " (" + c.TIT_TIPO + ")", Value = c.TIT_ID.ToString() });

            // area ou colecionador ............................................................................
            var area = _serviceArea.FindAll();
            ViewBag.area = area.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            ViewBag.titulacaoId = titulacaoId;

            return View("Detalhes");
        }
    }
}