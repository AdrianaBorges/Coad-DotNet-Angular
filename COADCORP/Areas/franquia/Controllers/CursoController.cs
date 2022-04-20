using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Exceptions;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.PROXY.Service;
using COAD.PROXY.Model.DTO;
using Coad.GenericCrud.Exceptions;

namespace COADCORP.Areas.franquia.Controllers
{
    [Autorizar(Departamento = "Franquiado, Franquiador, TI", PorMenu = false, PermitirNiveisPrivilegiosSuperiores = true)]
    public class CursoController : Controller
    {
        public CursoProxySRV _service { get; set; }
        public LocalizacaoCursoSRV _localizacaoCurso { get; set; }

        private void _PreencherViewBagCombos()
        {
            IList<ProdutosDTO> produtos = new ProdutosSRV().FindAllValid(true);
            IList<TipoProdutoComposicaoDTO> tiposProduto = new TipoProdutoComposicaoSRV().FindAll();
            IList<TipoEnvioDTO> tipoEnvio = new TipoEnvioSRV().FindAll();
            IList<TipoPeriodoDTO> tipoPeriodo = new TipoPeriodoSRV().FindAll();
            IList<UnidadeNegocioDTO> unidadeNegocio = new UnidadeNegocioSRV().FindAll();

            ViewBag.produtos = produtos;
            ViewBag.tiposProduto = tiposProduto;
            ViewBag.tipoEnvio = tipoEnvio;
            ViewBag.tipoPeriodo = tipoPeriodo;
            ViewBag.unidadeNegocio = unidadeNegocio;
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Index()
        {
            IList<AreasCorpDTO> areas = new AreasSRV().FindAll();
            IList<GrupoDTO> grupos = new GrupoSRV().FindAll();
            IList<TipoProdutoDTO> tipoProd = new TipoProdutoSRV().FindAll();

            ViewBag.mostrarTudo = true;
            ViewBag.areas = areas;
            ViewBag.grupos = grupos;
            ViewBag.tipoProd = tipoProd;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarCursos(string descricao = null,
            int pagina = 1,
            int itensPorPaginas = 7,
            bool? produtoInteresse = null,
            int? empId = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstCursos = _service.ListarCursos(descricao, produtoInteresse, pagina, itensPorPaginas, empId);
                response.AddPage("lstCursos", lstCursos);
            }            
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(GerenteDepartamento = "TI, FRANQUIADOR, FRANQUIADO", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Novo()
        {
            _PreencherViewBagCombos();
            return View("Editar");
        }

        [Autorizar(GerenteDepartamento = "TI, FRANQUIADOR, FRANQUIADO", PermitirNiveisPrivilegiosSuperiores = true)]
        [HttpPost]
        public ActionResult Editar(int composicaoId)
        {
            _PreencherViewBagCombos();
            ViewBag.composicaoId = composicaoId;
            return View();
        }
        
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(CursoProxyDTO curso)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    var cursoSalvo = _service.SalvarCurso(curso);
                    result.Add("curso", cursoSalvo);
                    return Json(result);
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                }
            }
            catch (Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
            }
            return Json(result);
        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult RecuperarDadosDoCurso(int composicaoId)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var curso = _service.FindByIdFullLoad(composicaoId);
                response.Add("curso", curso);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }
            return Json(response);
        }


        //[Autorizar(TipoAcesso = NivelAcesso.Excluir)]
        //[HttpPost]
        //public ActionResult Remover(int composicaoId)
        //{
        //    try
        //    {
        //        _service.DeletarProdutoComposicao(composicaoId);
        //        TempData["message"] = Message.Success("Composição Deletada com sucesso!");
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["message"] = Message.Fail(e.Message);
        //    }
        //    return RedirectToAction("Index");

        //}

        //[Autorizar(TipoAcesso = NivelAcesso.Acesso)]
        //[HttpPost]
        //public ActionResult Detalhes(int composicaoId)
        //{
        //    _PreencherViewBagCombos();
        //    ViewBag.composicaoId = composicaoId;
        //    return View("Detalhes");
        //}

        [Autorizar(IsAjax = true)]
        public ActionResult ListarProdutosDeInteresse(string nome = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var produtosComposicao = _service.ProdutosDeInteresse(nome);
                response.Add("produtosComposicao", produtosComposicao);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarLocalizacoesDeCurso()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstLocalizacoes = _localizacaoCurso.FindAll();
                response.Add("lstLocalizacoes", lstLocalizacoes);
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
